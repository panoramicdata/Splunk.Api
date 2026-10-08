using Refit;
using Splunk.Api.Handlers;
using Splunk.Api.Interfaces;
using Splunk.Api.Models.Inputs;
using Splunk.Api.Serialization;

namespace Splunk.Api;

/// <summary>
/// Client for the Splunk HTTP Event Collector (HEC): sends events, raw data and MINT data with an HEC token, queries
/// indexer acknowledgements and checks the collector's health. The collector listens on its own port (8088 by default),
/// separately from the management port <see cref="SplunkClient"/> uses; create and manage tokens with
/// <see cref="SplunkClient.HecTokens"/>.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="Collector"/> exposes every collector endpoint; <see cref="SendAsync(IEnumerable{HecEvent}, CancellationToken)"/>,
/// <see cref="SendRawAsync(string, CancellationToken)"/> and <see cref="QueryAcksAsync(IEnumerable{long}, CancellationToken)"/>
/// cover the common cases. Requests retry 429 and 503 ("server busy") as <see cref="SplunkHecClientOptions.MaxRetries"/>
/// allows; rejections raise <see cref="SplunkHecException"/>.
/// </para>
/// <para>A client is thread-safe and intended to be long-lived. Dispose it when done.</para>
/// </remarks>
public sealed class SplunkHecClient : IDisposable
{
	private static readonly RefitSettings Settings = new()
	{
		ContentSerializer = new HecContentSerializer(),
		UrlResolution = UrlResolutionMode.Rfc3986,
		// Buffered bodies stay replayable, so a request the collector answered "server busy" can be retried.
		Buffered = true,
		UrlParameterFormatter = new SplunkUrlParameterFormatter(),
		ExceptionFactory = response => new ValueTask<Exception?>(HecErrorMapper.CreateAsync(response)),
		// Surface transport failures (TimeoutException, HttpRequestException...) as themselves, not wrapped by Refit.
		TransportExceptionFactory = static (_, exception, _) => exception
	};

	private readonly HttpMessageHandler _pipeline;
	private readonly HttpClient _httpClient;

	/// <summary>Creates a client.</summary>
	/// <param name="options">Connection options.</param>
	public SplunkHecClient(SplunkHecClientOptions options) : this(options, CreateTransport(options))
	{
	}

	/// <summary>
	/// Creates a client that sends requests through <paramref name="innerHandler"/>, which it takes ownership of and
	/// disposes. Certificate settings in <paramref name="options"/> are ignored: configure them on the handler.
	/// </summary>
	/// <param name="options">Connection options.</param>
	/// <param name="innerHandler">The handler that sends requests to the network.</param>
	public SplunkHecClient(SplunkHecClientOptions options, HttpMessageHandler innerHandler)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(innerHandler);
		options.Validate();
		BaseAddress = new Uri(options.BaseUrl.EndsWith('/') ? options.BaseUrl : options.BaseUrl + "/");
		var retry = new RetryHandler(options.Logger, options.Timeout, options.MaxRetries, options.RetryBaseDelay, options.MaxRetryDelay)
		{
			InnerHandler = innerHandler
		};
		_pipeline = new HecAuthenticationHandler(options.Token, options.Channel) { InnerHandler = retry };
		_httpClient = new HttpClient(_pipeline, disposeHandler: false)
		{
			BaseAddress = BaseAddress,
			Timeout = System.Threading.Timeout.InfiniteTimeSpan
		};
		Collector = RestService.For<IHttpEventCollector>(_httpClient, Settings);
	}

	/// <summary>The base address every collector path is appended to, always ending in <c>/</c>.</summary>
	public Uri BaseAddress { get; }

	/// <summary>Every HTTP Event Collector endpoint (<c>services/collector/...</c>).</summary>
	public IHttpEventCollector Collector { get; }

	/// <summary>Sends events in one request (<c>POST services/collector/event</c>).</summary>
	/// <param name="events">The events; several are sent as one batch.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply, with an acknowledgement ID when the token uses indexer acknowledgement.</returns>
	public Task<HecResponse> SendAsync(IEnumerable<HecEvent> events, CancellationToken cancellationToken)
		=> Collector.SendEventsAsync(events, null, cancellationToken);

	/// <summary>Sends raw text, broken into events by the token's sourcetype (<c>POST services/collector/raw</c>).</summary>
	/// <param name="data">The raw text.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	/// <remarks>With a token that uses indexer acknowledgement, set <see cref="SplunkHecClientOptions.Channel"/>.</remarks>
	public Task<HecResponse> SendRawAsync(string data, CancellationToken cancellationToken)
		=> Collector.SendRawAsync(data, null, cancellationToken);

	/// <summary>
	/// Asks whether the events behind acknowledgement IDs have been indexed (<c>POST services/collector/ack</c>), on the
	/// default channel.
	/// </summary>
	/// <param name="ackIds">The acknowledgement IDs returned when the events were sent.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>For each ID, <see langword="true"/> once its events are indexed.</returns>
	public async Task<IReadOnlyDictionary<long, bool>> QueryAcksAsync(IEnumerable<long> ackIds, CancellationToken cancellationToken)
	{
		var reply = await Collector.QueryAcksAsync(new HecAckRequest { Acks = [.. ackIds] }, null, cancellationToken).ConfigureAwait(false);
		return reply.Acks;
	}

	private static HttpClientHandler CreateTransport(SplunkHecClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		return SplunkClient.CreateTransport(options.ServerCertificateValidationCallback, options.TrustedServerCertificateThumbprint);
	}

	/// <inheritdoc />
	public void Dispose()
	{
		_httpClient.Dispose();
		_pipeline.Dispose();
	}
}
