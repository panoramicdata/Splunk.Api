using Refit;

namespace Splunk.Api.Models.Deployment;

/// <summary>Filters on the deployment clients of <c>GET deployment/server/clients/{name}</c>. Leave a property <see langword="null"/> to not filter on it.</summary>
public sealed class DeploymentServerClientFilter
{
	/// <summary>A comma-separated list of server classes; only clients that would receive an app of one of them.</summary>
	[AliasAs("serverclasses")]
	public string? ServerClasses { get; init; }

	/// <summary>Only clients that tried to download this app.</summary>
	[AliasAs("application")]
	public string? Application { get; init; }

	/// <summary>Only clients with (or without) a deployment error.</summary>
	[AliasAs("hasDeploymentError")]
	public bool? HasDeploymentError { get; init; }

	/// <summary>Only clients that phoned home at or after this time, in epoch seconds.</summary>
	[AliasAs("minLatestPhonehomeTime")]
	public long? MinLatestPhoneHomeTime { get; init; }

	/// <summary>Only clients whose phone home latency to average interval ratio is above this.</summary>
	[AliasAs("minPhonehome_latency_to_avgInterval_ratio")]
	public double? MinPhoneHomeLatencyRatio { get; init; }

	/// <summary>Only clients whose phone home latency to average interval ratio is below this.</summary>
	[AliasAs("maxPhonehome_latency_to_avgInterval_ratio")]
	public double? MaxPhoneHomeLatencyRatio { get; init; }
}
