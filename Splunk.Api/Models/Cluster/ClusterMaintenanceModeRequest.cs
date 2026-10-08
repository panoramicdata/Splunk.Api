using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Turns maintenance mode on or off (<c>POST cluster/manager/control/default/maintenance</c>).</summary>
public sealed class ClusterMaintenanceModeRequest : SplunkFormRequest
{
	/// <summary><see langword="true"/> to enter maintenance mode, <see langword="false"/> to leave it.</summary>
	[JsonPropertyName("mode")]
	public required bool Mode { get; init; }
}
