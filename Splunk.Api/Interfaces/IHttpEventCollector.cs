using Refit;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>
/// The HTTP Event Collector endpoints (<c>services/collector/...</c>), served on the collector's own port (8088 by
/// default). Use through <see cref="SplunkHecClient.Collector"/>, which authenticates with an HEC token.
/// </summary>
public interface IHttpEventCollector
{
	/// <summary>Sends events in the JSON event format (<c>POST services/collector</c>).</summary>
	/// <param name="events">The events; several are sent as one batch.</param>
	/// <param name="options">The channel and default metadata, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply, with an acknowledgement ID when the token uses indexer acknowledgement.</returns>
	[Post("services/collector")]
	Task<HecResponse> SendAsync([Body] IEnumerable<HecEvent> events, [Query] HecRequestOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Sends events in the JSON event format (<c>POST services/collector/event</c>). With
	/// <see cref="HecRequestOptions.AutoExtractTimestamp"/>, events without a time take it from their text.
	/// </summary>
	/// <param name="events">The events; several are sent as one batch.</param>
	/// <param name="options">The channel and default metadata, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	[Post("services/collector/event")]
	Task<HecResponse> SendEventsAsync([Body] IEnumerable<HecEvent> events, [Query] HecRequestOptions? options, CancellationToken cancellationToken);

	/// <summary>Sends events through the versioned path (<c>POST services/collector/event/1.0</c>); otherwise the same as <see cref="SendEventsAsync"/>.</summary>
	/// <param name="events">The events.</param>
	/// <param name="options">The channel and default metadata, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	[Post("services/collector/event/1.0")]
	Task<HecResponse> SendEventsV1Async([Body] IEnumerable<HecEvent> events, [Query] HecRequestOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Sends raw text, broken into events by the sourcetype's rules (<c>POST services/collector/raw</c>). Requires a
	/// channel, from the options or <see cref="SplunkHecClientOptions.Channel"/>.
	/// </summary>
	/// <param name="data">The raw text, sent as the whole body.</param>
	/// <param name="options">The channel and default metadata, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	[Post("services/collector/raw")]
	Task<HecResponse> SendRawAsync([Body] string data, [Query] HecRequestOptions? options, CancellationToken cancellationToken);

	/// <summary>Sends raw text through the versioned path (<c>POST services/collector/raw/1.0</c>); otherwise the same as <see cref="SendRawAsync"/>.</summary>
	/// <param name="data">The raw text.</param>
	/// <param name="options">The channel and default metadata, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	[Post("services/collector/raw/1.0")]
	Task<HecResponse> SendRawV1Async([Body] string data, [Query] HecRequestOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Sends data in the MINT format (<c>POST services/collector/mint</c>). Splunk 10.6 requires a channel here as on
	/// the raw endpoint.
	/// </summary>
	/// <param name="data">The MINT payload, sent as the whole body.</param>
	/// <param name="options">The channel and default metadata, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	[Post("services/collector/mint")]
	Task<HecResponse> SendMintAsync([Body] string data, [Query] HecRequestOptions? options, CancellationToken cancellationToken);

	/// <summary>Sends MINT data through the versioned path (<c>POST services/collector/mint/1.0</c>).</summary>
	/// <param name="data">The MINT payload.</param>
	/// <param name="options">The channel and default metadata, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	[Post("services/collector/mint/1.0")]
	Task<HecResponse> SendMintV1Async([Body] string data, [Query] HecRequestOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Asks whether the events behind acknowledgement IDs have been indexed (<c>POST services/collector/ack</c>). The
	/// token must use indexer acknowledgement, and the channel must be the one the events were sent on.
	/// </summary>
	/// <remarks>
	/// The reference documents this as a GET with a JSON body; Splunk 10.6 answers GET with 405 and takes POST.
	/// </remarks>
	/// <param name="request">The acknowledgement IDs.</param>
	/// <param name="options">The channel, or <see langword="null"/> for the client's default channel.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>For each ID, whether its events are indexed.</returns>
	[Post("services/collector/ack")]
	Task<HecAckResponse> QueryAcksAsync([Body] HecAckRequest request, [Query] HecChannelOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Checks that the collector can accept data (<c>GET services/collector/health</c>). An unhealthy collector answers
	/// 503, which raises <see cref="SplunkHecException"/> once retries are exhausted.
	/// </summary>
	/// <param name="options">Whether to include the acknowledgement service, and a token to check, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply, <c>HEC is healthy</c> (code 17).</returns>
	[Get("services/collector/health")]
	Task<HecResponse> GetHealthAsync([Query] HecHealthOptions? options, CancellationToken cancellationToken);

	/// <summary>Checks the collector's health through the versioned path (<c>GET services/collector/health/1.0</c>).</summary>
	/// <param name="options">Whether to include the acknowledgement service, and a token to check, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	[Get("services/collector/health/1.0")]
	Task<HecResponse> GetHealthV1Async([Query] HecHealthOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Sends Splunk-to-Splunk (S2S) protocol data over HTTP, as a universal forwarder does
	/// (<c>POST services/collector/s2s</c>). The stream is sent as-is and is not retried.
	/// </summary>
	/// <param name="data">The S2S payload.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The collector's reply.</returns>
	[Post("services/collector/s2s")]
	Task<HecResponse> SendS2SAsync([Body] Stream data, CancellationToken cancellationToken);
}
