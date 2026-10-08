using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>A list of capabilities (<c>authorization/capabilities</c> and <c>authorization/grantable_capabilities</c>).</summary>
public sealed class CapabilityList : SplunkContent
{
	/// <summary>The capability names, for example <c>admin_all_objects</c> or <c>search</c>.</summary>
	[JsonPropertyName("capabilities")]
	public IReadOnlyList<string> Capabilities { get; init; } = [];
}
