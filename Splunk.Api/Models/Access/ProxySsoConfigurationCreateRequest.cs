using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates a ProxySSO configuration (<c>POST admin/ProxySSO-auth</c>).</summary>
public sealed class ProxySsoConfigurationCreateRequest : ProxySsoSettings
{
	/// <summary>The configuration name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
