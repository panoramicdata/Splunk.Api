using Microsoft.Extensions.Logging;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Splunk.Api;

/// <summary>
/// Configuration for <see cref="SplunkClient"/>. The values are read once when the client is constructed; changing this
/// object afterwards does not affect an existing client.
/// </summary>
/// <remarks>
/// Supply exactly one way to authenticate: a <see cref="Token"/> (a Splunk authentication token, sent as a bearer token,
/// which Splunk recommends), or a <see cref="Username"/> and <see cref="Password"/>. With a username and password the client
/// logs in through <c>services/auth/login</c>, sends the session key it receives, and logs in again once if Splunk answers
/// 401 because the session expired; set <see cref="UseBasicAuthentication"/> to send HTTP basic credentials on every
/// request instead.
/// </remarks>
public class SplunkClientOptions
{
	/// <summary>
	/// Absolute URL of the Splunk management port, e.g. <c>https://splunk.example.com:8089</c>. A path prefix (for a
	/// reverse proxy) is kept: every endpoint is appended to it. It must not contain credentials (<c>user:password@</c>).
	/// </summary>
	public string BaseUrl { get; set; } = string.Empty;

	/// <summary>A Splunk authentication token, sent as <c>Authorization: Bearer</c>. Mutually exclusive with <see cref="Username"/>.</summary>
	public string? Token { get; set; }

	/// <summary>The Splunk user name for session or basic authentication. Requires <see cref="Password"/>.</summary>
	public string? Username { get; set; }

	/// <summary>The password for <see cref="Username"/>.</summary>
	public string? Password { get; set; }

	/// <summary>
	/// Send HTTP basic credentials on every request instead of logging in for a session key. Requires
	/// <see cref="Username"/> and <see cref="Password"/>. Defaults to <see langword="false"/>.
	/// </summary>
	public bool UseBasicAuthentication { get; set; }

	/// <summary>
	/// The default namespace (user and app context) for namespaced endpoints. When set, requests to <c>services/...</c>
	/// are sent to <c>servicesNS/{owner}/{app}/...</c>. Leave <see langword="null"/> to use the global
	/// <c>services/...</c> context. A different namespace can be used per call through <see cref="SplunkClient.InNamespace(SplunkNamespace)"/>.
	/// </summary>
	public SplunkNamespace? Namespace { get; set; }

	/// <summary>
	/// When <see langword="true"/>, the client refuses (with <see cref="SplunkReadOnlyException"/>, before anything is
	/// sent) every request that could change Splunk: any DELETE, and any POST except the read-only operations on an
	/// allow-list (logging in, creating, controlling and exporting search jobs, and parsing or previewing searches).
	/// </summary>
	/// <remarks>
	/// This guards the REST surface, not the SPL inside a search: a search that runs <c>| delete</c>, <c>| outputlookup</c>
	/// or <c>| collect</c> still runs. Use a Splunk role without those capabilities for a truly read-only identity.
	/// </remarks>
	public bool ReadOnly { get; set; }

	/// <summary>
	/// The SHA-256 thumbprint (hex, case and separators ignored) of a server certificate to trust even when it fails
	/// normal validation, such as Splunk's default self-signed certificate. Other certificates are still validated
	/// normally. Ignored when <see cref="ServerCertificateValidationCallback"/> is set, and when an inner
	/// <see cref="HttpMessageHandler"/> is supplied to the client.
	/// </summary>
	public string? TrustedServerCertificateThumbprint { get; set; }

	/// <summary>
	/// Full control over server certificate validation. Takes precedence over <see cref="TrustedServerCertificateThumbprint"/>.
	/// Ignored when an inner <see cref="HttpMessageHandler"/> is supplied to the client.
	/// </summary>
	public Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? ServerCertificateValidationCallback { get; set; }

	/// <summary>
	/// HTTP timeout per attempt, covering sending the request and receiving the response headers. It does not include
	/// retry back-off, nor reading a streamed body after the headers arrive. An attempt that exceeds it raises a
	/// <see cref="TimeoutException"/>; caller cancellation still raises <see cref="OperationCanceledException"/>.
	/// </summary>
	public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);

	/// <summary>
	/// Maximum retries of a transient failure. Any verb is retried on 429 and 503; other 5xx responses are retried only for
	/// idempotent verbs (GET, HEAD, PUT, DELETE), never POST. Requests with a stream body are never retried.
	/// </summary>
	public int MaxRetries { get; set; } = 3;

	/// <summary>Initial back-off, doubled on each retry (up to <see cref="MaxRetryDelay"/>). Must not be negative.</summary>
	public TimeSpan RetryBaseDelay { get; set; } = TimeSpan.FromSeconds(1);

	/// <summary>The longest single wait before a retry, also capping a server-supplied <c>Retry-After</c>. Must be greater than zero.</summary>
	public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);

	/// <summary>
	/// Optional logger. Credentials, session keys and query strings (which can carry search text) are never logged; only
	/// the method and path are.
	/// </summary>
	public ILogger? Logger { get; set; }

	internal AuthenticationKind AuthenticationKind
		=> !string.IsNullOrWhiteSpace(Token)
			? AuthenticationKind.Token
			: UseBasicAuthentication ? AuthenticationKind.Basic : AuthenticationKind.Session;

	internal void Validate()
	{
		// Absolute alone is not enough: on Linux a rooted path such as "/relative/path" parses as a file:// URI.
		if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out var baseUri)
			|| (baseUri.Scheme != Uri.UriSchemeHttp && baseUri.Scheme != Uri.UriSchemeHttps))
		{
			throw new ArgumentException("BaseUrl must be an absolute http or https URL.", nameof(BaseUrl));
		}

		// HttpClient never sends user info, and it would appear in logs and exception messages that show the URL.
		if (baseUri.UserInfo.Length > 0)
		{
			throw new ArgumentException("BaseUrl must not contain credentials: set Token, or Username and Password.", nameof(BaseUrl));
		}

		ValidateCredentials();
		_ = SplunkClient.NormalizeThumbprint(TrustedServerCertificateThumbprint);
		ArgumentOutOfRangeException.ThrowIfNegative(MaxRetries);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(Timeout, TimeSpan.Zero);
		ArgumentOutOfRangeException.ThrowIfLessThan(RetryBaseDelay, TimeSpan.Zero);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(MaxRetryDelay, TimeSpan.Zero);
	}

	private void ValidateCredentials()
	{
		var hasToken = !string.IsNullOrWhiteSpace(Token);
		var hasUser = !string.IsNullOrWhiteSpace(Username);
		if (hasToken && (hasUser || UseBasicAuthentication))
		{
			throw new ArgumentException("Set either Token, or Username and Password, not both.", nameof(Token));
		}

		if (!hasToken && (!hasUser || string.IsNullOrEmpty(Password)))
		{
			throw new ArgumentException("Set Token, or both Username and Password.", nameof(Username));
		}
	}

	/// <inheritdoc />
	public override string ToString()
		=> $"SplunkClientOptions {{ BaseUrl = {MaskUserInfo(BaseUrl)}, Token = {Mask(Token)}, Username = {Username}, Password = {Mask(Password)}, Namespace = {Namespace}, ReadOnly = {ReadOnly} }}";

	private static string Mask(string? secret) => string.IsNullOrEmpty(secret) ? "(none)" : "***";

	/// <summary>
	/// Masks any <c>user:password@</c> in a URL. Done on the text rather than a parsed <see cref="Uri"/>, since this must
	/// also redact a URL that fails to parse (such as one whose password contains an unescaped <c>@</c>).
	/// </summary>
	private static string MaskUserInfo(string url)
	{
		var start = url.IndexOf("://", StringComparison.Ordinal);
		if (start < 0)
		{
			return url;
		}

		start += 3;
		var end = url.IndexOfAny(['/', '?', '#'], start);
		var authority = end < 0 ? url[start..] : url[start..end];
		var at = authority.LastIndexOf('@');
		return at < 0 ? url : $"{url[..start]}***{url[(start + at)..]}";
	}
}
