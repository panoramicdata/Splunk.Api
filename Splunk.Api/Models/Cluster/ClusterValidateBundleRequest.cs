using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Options of a configuration bundle validation (<c>POST cluster/manager/control/default/validate_bundle</c>).</summary>
public sealed class ClusterValidateBundleRequest : SplunkFormRequest
{
	/// <summary>Whether to also check if applying the bundle would restart the peers.</summary>
	[JsonPropertyName("check-restart")]
	public bool? CheckRestart { get; init; }
}
