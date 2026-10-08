using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>splunkd's outbound HTTP proxy configuration (<c>server/httpsettings/proxysettings/proxyConfig</c>).</summary>
public sealed class ProxySettings : SplunkContent
{
	/// <summary>The proxy for HTTP requests (<c>http_proxy</c>).</summary>
	[JsonPropertyName("http_proxy")]
	public string? HttpProxy { get; init; }

	/// <summary>The proxy for HTTPS requests (<c>https_proxy</c>).</summary>
	[JsonPropertyName("https_proxy")]
	public string? HttpsProxy { get; init; }

	/// <summary>Hosts reached without the proxy (<c>no_proxy</c>), comma-separated.</summary>
	[JsonPropertyName("no_proxy")]
	public string? NoProxy { get; init; }

	/// <summary>Whether TLS to the proxy itself is enabled (<c>enable_tls_proxy</c>); <see langword="null"/> when not set.</summary>
	[JsonPropertyName("enable_tls_proxy")]
	public bool? EnableTlsProxy { get; init; }
}
