using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Options of a configuration bundle push (<c>POST cluster/manager/control/default/apply</c>).</summary>
public sealed class ClusterApplyBundleRequest : SplunkFormRequest
{
	/// <summary>Whether to skip bundle validation (default <c>false</c>).</summary>
	[JsonPropertyName("skip-validation")]
	public bool? SkipValidation { get; init; }

	/// <summary>Whether to skip the push when the bundle matches the active one (default <c>true</c>).</summary>
	[JsonPropertyName("ignore_identical_bundle")]
	public bool? IgnoreIdenticalBundle { get; init; }
}
