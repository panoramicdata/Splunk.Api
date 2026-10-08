using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Replaces the Splunk roles mapped to a ProxySSO group (<c>POST admin/ProxySSO-groups/{group_name}</c>).</summary>
public sealed class RoleMappingUpdateRequest : SplunkFormRequest
{
	/// <summary>The Splunk roles to map, sent once per role.</summary>
	[JsonPropertyName("roles")]
	public required IReadOnlyList<string> Roles { get; init; }
}
