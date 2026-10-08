using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>Activates a license group (<c>POST licenser/groups/{name}</c>).</summary>
public sealed class LicenseGroupUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether to make the group the active one; the previously active group is deactivated.</summary>
	[JsonPropertyName("is_active")]
	public required bool IsActive { get; init; }
}
