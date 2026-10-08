using Refit;

namespace Splunk.Api.Models.Deployment;

/// <summary>The parameters of <c>GET deployment/server/clients</c>.</summary>
public sealed class DeploymentServerClientListOptions : ListOptions
{
	/// <summary>Only clients with an app in this state: <c>phonehome</c>, <c>unknown</c>, <c>download</c>, <c>install</c> or <c>uninstall</c>.</summary>
	[AliasAs("action")]
	public string? Action { get; init; }

	/// <summary>Only clients that tried to download this app.</summary>
	[AliasAs("application")]
	public string? Application { get; init; }

	/// <summary>Only clients with (or without) a deployment error.</summary>
	[AliasAs("hasDeploymentError")]
	public bool? HasDeploymentError { get; init; }

	/// <summary>Only clients whose phone home latency to average interval ratio is below this.</summary>
	[AliasAs("maxPhonehome_latency_to_avgInterval_ratio")]
	public double? MaxPhoneHomeLatencyRatio { get; init; }

	/// <summary>Only clients that phoned home at or after this time, in epoch seconds.</summary>
	[AliasAs("minLatestPhonehomeTime")]
	public long? MinLatestPhoneHomeTime { get; init; }

	/// <summary>Only clients whose phone home latency to average interval ratio is above this.</summary>
	[AliasAs("minPhonehome_latency_to_avgInterval_ratio")]
	public double? MinPhoneHomeLatencyRatio { get; init; }

	/// <summary>A comma-separated list of server classes; only clients that would receive an app of one of them.</summary>
	[AliasAs("serverclasses")]
	public string? ServerClasses { get; init; }
}
