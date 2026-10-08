using Microsoft.Extensions.Logging;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Splunk.Api;

/// <summary>
/// Configuration for <see cref="SplunkHecClient"/>. The values are read once when the client is constructed; changing this
/// object afterwards does not affect an existing client.
/// </summary>
public class SplunkHecClientOptions
{
	/// <summary>
	/// Absolute URL of the HTTP Event Collector, e.g. <c>https://splunk.example.com:8088</c>. This is a different port
	/// (and often a different host, such as a load balancer) from the management port <see cref="SplunkClient"/> uses.
	/// </summary>
	public string BaseUrl { get; set; } = string.Empty;

	/// <summary>The HTTP Event Collector token, sent as <c>Authorization: Splunk {token}</c>.</summary>
	public string Token { get; set; } = string.Empty;

	/// <summary>
	/// The default channel (a GUID) sent as <c>X-Splunk-Request-Channel</c> on every request. Tokens with indexer
	/// acknowledgement (<c>useACK</c>) require a channel on every data request; a request whose options set a
	/// <c>channel</c> query parameter uses that instead. Leave <see langword="null"/> to send no channel header.
	/// </summary>
	public string? Channel { get; set; }

	/// <summary>
	/// The SHA-256 thumbprint (hex, case and separators ignored) of a server certificate to trust even when it fails
	/// normal validation, such as Splunk's default self-signed certificate. Ignored when
	/// <see cref="ServerCertificateValidationCallback"/> is set or an inner handler is supplied.
	/// </summary>
	public string? TrustedServerCertificateThumbprint { get; set; }

	/// <summary>Full control over server certificate validation. Ignored when an inner handler is supplied.</summary>
	public Func<X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? ServerCertificateValidationCallback { get; set; }

	/// <summary>HTTP timeout per attempt; see <see cref="SplunkClientOptions.Timeout"/>.</summary>
	public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);

	/// <summary>
	/// Maximum retries of a transient failure: any request on 429 or 503 (the collector's "server busy"), idempotent
	/// ones on other 5xx. Stream bodies are never retried.
	/// </summary>
	public int MaxRetries { get; set; } = 3;

	/// <summary>Initial back-off, doubled on each retry (up to <see cref="MaxRetryDelay"/>). Must not be negative.</summary>
	public TimeSpan RetryBaseDelay { get; set; } = TimeSpan.FromSeconds(1);

	/// <summary>The longest single wait before a retry. Must be greater than zero.</summary>
	public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);

	/// <summary>Optional logger. The token and query strings are never logged; only the method and path are.</summary>
	public ILogger? Logger { get; set; }

	internal void Validate()
	{
		if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out var baseUri)
			|| (baseUri.Scheme != Uri.UriSchemeHttp && baseUri.Scheme != Uri.UriSchemeHttps))
		{
			throw new ArgumentException("BaseUrl must be an absolute http or https URL.", nameof(BaseUrl));
		}

		if (string.IsNullOrWhiteSpace(Token))
		{
			throw new ArgumentException("Set Token to an HTTP Event Collector token.", nameof(Token));
		}

		if (Channel is not null && !Guid.TryParse(Channel, out _))
		{
			throw new ArgumentException("Channel must be a GUID.", nameof(Channel));
		}

		_ = SplunkClient.NormalizeThumbprint(TrustedServerCertificateThumbprint);
		ArgumentOutOfRangeException.ThrowIfNegative(MaxRetries);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(Timeout, TimeSpan.Zero);
		ArgumentOutOfRangeException.ThrowIfLessThan(RetryBaseDelay, TimeSpan.Zero);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(MaxRetryDelay, TimeSpan.Zero);
	}

	/// <inheritdoc />
	public override string ToString()
		=> $"SplunkHecClientOptions {{ BaseUrl = {BaseUrl}, Token = {(string.IsNullOrEmpty(Token) ? "(none)" : "***")}, Channel = {Channel ?? "(none)"} }}";
}
