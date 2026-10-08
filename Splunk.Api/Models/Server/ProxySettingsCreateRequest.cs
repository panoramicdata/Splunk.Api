using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>Creates splunkd's HTTP proxy configuration (<c>POST server/httpsettings/proxysettings</c>).</summary>
public sealed class ProxySettingsCreateRequest : ProxySettingsUpdateRequest
{
	/// <summary>The configuration name (<c>name</c>). Splunk accepts only <c>proxyConfig</c>, the default.</summary>
	[JsonPropertyName("name")]
	public string Name { get; init; } = "proxyConfig";
}
