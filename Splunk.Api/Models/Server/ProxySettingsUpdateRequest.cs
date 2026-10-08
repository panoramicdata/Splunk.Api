using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>Changes splunkd's HTTP proxy configuration (<c>POST server/httpsettings/proxysettings/proxyConfig</c>).</summary>
public class ProxySettingsUpdateRequest : SplunkFormRequest
{
	/// <summary>The proxy for HTTP requests, for example <c>http://proxy.example.com:3128</c> (<c>http_proxy</c>).</summary>
	[JsonPropertyName("http_proxy")]
	public string? HttpProxy { get; init; }

	/// <summary>The proxy for HTTPS requests (<c>https_proxy</c>).</summary>
	[JsonPropertyName("https_proxy")]
	public string? HttpsProxy { get; init; }

	/// <summary>Hosts reached without the proxy, comma-separated (<c>no_proxy</c>).</summary>
	[JsonPropertyName("no_proxy")]
	public string? NoProxy { get; init; }
}
