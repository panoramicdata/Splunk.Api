using Refit;

namespace Splunk.Api.Models.Deployment;

/// <summary>The parameters of <c>GET deployment/server/applications</c>.</summary>
public sealed class DeploymentApplicationListOptions : ListOptions
{
	/// <summary>Only apps that match this deployment client.</summary>
	[AliasAs("clientId")]
	public string? ClientId { get; init; }

	/// <summary>Only apps with (or without) a deployment error on some client.</summary>
	[AliasAs("hasDeploymentError")]
	public bool? HasDeploymentError { get; init; }
}
