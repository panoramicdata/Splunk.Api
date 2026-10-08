using Refit;
using Splunk.Api.Handlers;
using Splunk.Api.Serialization;
using System.Security.Cryptography;

namespace Splunk.Api;

/// <summary>
/// Client for the Splunk Enterprise REST API. Each group of endpoints is a property (for example
/// <see cref="ServerInfo"/>); the groups are created on first use.
/// </summary>
/// <remarks>
/// <para>
/// Every request asks for JSON (<c>output_mode=json</c>), authenticates as <see cref="SplunkClientOptions"/> describes,
/// and retries transient failures. Non-success responses raise <see cref="SplunkApiException"/>; a failure to send raises
/// the transport's own exception (such as <see cref="HttpRequestException"/>, or <see cref="TimeoutException"/> when an
/// attempt exceeds <see cref="SplunkClientOptions.Timeout"/>).
/// </para>
/// <para>
/// A client is thread-safe and intended to be long-lived: create one per Splunk instance and identity, and dispose it
/// when done. <see cref="InNamespace(SplunkNamespace)"/> returns a lightweight view in another user/app context that shares
/// this client's connections and session; a view must not be used after this client is disposed.
/// </para>
/// </remarks>
public sealed partial class SplunkClient : IDisposable
{
	private readonly HttpMessageHandler _pipeline;
	private readonly HttpClient _httpClient;
	private readonly bool _ownsPipeline;

	/// <summary>Creates a client.</summary>
	/// <param name="options">Connection options.</param>
	public SplunkClient(SplunkClientOptions options) : this(options, CreateTransport(Validated(options)))
	{
	}

	/// <summary>
	/// Creates a client that sends requests through <paramref name="innerHandler"/> (for example a proxy-aware or
	/// instrumented handler). The client takes ownership of the handler and disposes it.
	/// <see cref="SplunkClientOptions.TrustedServerCertificateThumbprint"/> and
	/// <see cref="SplunkClientOptions.ServerCertificateValidationCallback"/> are ignored: configure certificate
	/// validation on the handler instead.
	/// </summary>
	/// <param name="options">Connection options.</param>
	/// <param name="innerHandler">The handler that sends requests to the network.</param>
	public SplunkClient(SplunkClientOptions options, HttpMessageHandler innerHandler)
	{
		ArgumentNullException.ThrowIfNull(innerHandler);
		Validated(options);
		BaseAddress = CreateBaseAddress(options.BaseUrl);
		_pipeline = CreatePipeline(options, BaseAddress, innerHandler);
		_ownsPipeline = true;
		Namespace = options.Namespace;
		_httpClient = CreateHttpClient(_pipeline, BaseAddress, Namespace);
	}

	private SplunkClient(SplunkClient parent, SplunkNamespace splunkNamespace)
	{
		BaseAddress = parent.BaseAddress;
		_pipeline = parent._pipeline;
		_ownsPipeline = false;
		Namespace = splunkNamespace;
		_httpClient = CreateHttpClient(_pipeline, BaseAddress, splunkNamespace);
	}

	/// <summary>The base address every endpoint path is appended to, always ending in <c>/</c>.</summary>
	public Uri BaseAddress { get; }

	/// <summary>
	/// The namespace requests to <c>services/...</c> are sent in, or <see langword="null"/> for the global context.
	/// </summary>
	public SplunkNamespace? Namespace { get; }

	/// <summary>The Refit settings shared by every endpoint group.</summary>
	internal static RefitSettings Settings { get; } = new()
	{
		ContentSerializer = new SplunkContentSerializer(),
		// Interface paths are relative (no leading slash) so they append to a path-prefixed BaseUrl.
		UrlResolution = UrlResolutionMode.Rfc3986,
		UrlParameterFormatter = new SplunkUrlParameterFormatter(),
		// Unbuffered, Refit sends a serialized body as a read-once push stream, which can be neither retried nor resent
		// after a re-login.
		Buffered = true,
		ExceptionFactory = response => new ValueTask<Exception?>(SplunkErrorMapper.CreateAsync(response)),
		// Refit would wrap every exception thrown while sending in its ApiRequestException; surface them as themselves
		// (TimeoutException, HttpRequestException, ObjectDisposedException...), as documented.
		TransportExceptionFactory = static (_, exception, _) => exception
	};

	/// <summary>
	/// Returns a view of this client that sends requests for <c>services/...</c> to
	/// <c>servicesNS/{owner}/{app}/...</c>. The view shares this client's connections and session.
	/// </summary>
	/// <param name="splunkNamespace">The user and app context.</param>
	/// <returns>A client in that namespace. Disposing it does not affect this client.</returns>
	public SplunkClient InNamespace(SplunkNamespace splunkNamespace)
	{
		ArgumentNullException.ThrowIfNull(splunkNamespace);
		_ = splunkNamespace.PathPrefix;
		return new(this, splunkNamespace);
	}

	/// <summary>Returns a view of this client in <c>{owner}/{app}</c>. See <see cref="InNamespace(SplunkNamespace)"/>.</summary>
	/// <param name="owner">The user context, <c>nobody</c>, or <c>-</c> for all users.</param>
	/// <param name="app">The app context, or <c>-</c> for all apps.</param>
	/// <returns>A client in that namespace.</returns>
	public SplunkClient InNamespace(string owner, string app) => InNamespace(new SplunkNamespace(owner, app));

	internal T For<T>() => RestService.For<T>(_httpClient, Settings);

	/// <inheritdoc />
	public void Dispose()
	{
		_httpClient.Dispose();
		if (_ownsPipeline)
		{
			_pipeline.Dispose();
		}
	}

	// Validation comes before the transport is created, so invalid options do not leave an undisposed HttpClientHandler.
	private static SplunkClientOptions Validated(SplunkClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		options.Validate();
		return options;
	}

	private static Uri CreateBaseAddress(string baseUrl) => new(baseUrl.EndsWith('/') ? baseUrl : baseUrl + "/");

	private static HttpMessageHandler CreatePipeline(SplunkClientOptions options, Uri baseAddress, HttpMessageHandler innerHandler)
	{
		HttpMessageHandler handler = new RetryHandler(options) { InnerHandler = innerHandler };
		handler = new AuthenticationHandler(options, baseAddress) { InnerHandler = handler };
		handler = new OutputModeHandler { InnerHandler = handler };
		if (options.ReadOnly)
		{
			handler = new ReadOnlyHandler(baseAddress) { InnerHandler = handler };
		}

		return handler;
	}

	private static HttpClient CreateHttpClient(HttpMessageHandler pipeline, Uri baseAddress, SplunkNamespace? splunkNamespace)
	{
		// A namespace handler wraps the shared pipeline; the HttpClient never disposes the pipeline, which the owning client does.
		var handler = splunkNamespace is null ? pipeline : new NamespaceHandler(baseAddress, splunkNamespace) { InnerHandler = pipeline };
		return new HttpClient(handler, disposeHandler: false)
		{
			BaseAddress = baseAddress,
			// The per-attempt timeout is applied inside RetryHandler so retries and Retry-After waits are not cut short.
			Timeout = System.Threading.Timeout.InfiniteTimeSpan
		};
	}

	internal static HttpClientHandler CreateTransport(SplunkClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		return CreateTransport(options.ServerCertificateValidationCallback, options.TrustedServerCertificateThumbprint);
	}

	/// <summary>
	/// Creates the network handler: <paramref name="validationCallback"/> when set, else trust for the certificate with
	/// <paramref name="trustedThumbprint"/> (SHA-256) on top of normal validation.
	/// </summary>
	internal static HttpClientHandler CreateTransport(
		Func<HttpRequestMessage, System.Security.Cryptography.X509Certificates.X509Certificate2?, System.Security.Cryptography.X509Certificates.X509Chain?, System.Net.Security.SslPolicyErrors, bool>? validationCallback,
		string? trustedThumbprint)
	{
		var handler = new HttpClientHandler();
		if (validationCallback is { } callback)
		{
			handler.ServerCertificateCustomValidationCallback = callback;
		}
		else if (NormalizeThumbprint(trustedThumbprint) is { } pinned)
		{
			handler.ServerCertificateCustomValidationCallback = (_, certificate, _, errors)
				=> errors == System.Net.Security.SslPolicyErrors.None
					|| (certificate is not null
						&& string.Equals(certificate.GetCertHashString(HashAlgorithmName.SHA256), pinned, StringComparison.OrdinalIgnoreCase));
		}

		return handler;
	}

	internal static string? NormalizeThumbprint(string? thumbprint)
	{
		if (string.IsNullOrWhiteSpace(thumbprint))
		{
			return null;
		}

		var hex = new string([.. thumbprint.Where(Uri.IsHexDigit)]);
		return hex.Length == 64
			? hex
			: throw new ArgumentException("TrustedServerCertificateThumbprint must be a SHA-256 thumbprint (64 hex digits).", nameof(thumbprint));
	}
}
