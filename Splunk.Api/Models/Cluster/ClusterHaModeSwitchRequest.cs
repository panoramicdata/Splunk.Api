using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Switches the high availability mode of the cluster manager (<c>POST cluster/manager/redundancy</c>).</summary>
public sealed class ClusterHaModeSwitchRequest : SplunkFormRequest
{
	/// <summary>The mode to switch to.</summary>
	[JsonPropertyName("ha_mode")]
	public required ClusterHaMode HaMode { get; init; }

	/// <summary>The action, always <c>switch_mode</c>.</summary>
	[JsonPropertyName("_action")]
	public string Action { get; } = "switch_mode";
}
