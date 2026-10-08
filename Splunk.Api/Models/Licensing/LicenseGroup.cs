using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>A license group (<c>licenser/groups</c>), such as <c>Enterprise</c> or <c>Free</c>.</summary>
public sealed class LicenseGroup : SplunkContent
{
	/// <summary>Whether this is the active group.</summary>
	[JsonPropertyName("is_active")]
	public bool IsActive { get; init; }

	/// <summary>The license stacks in the group.</summary>
	[JsonPropertyName("stack_ids")]
	public IReadOnlyList<string> StackIds { get; init; } = [];
}
