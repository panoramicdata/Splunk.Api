using Refit;

namespace Splunk.Api.Models.Deployment;

/// <summary>Filters on <c>GET deployment/server/serverclasses/{name}</c>. Leave a property <see langword="null"/> to not filter on it.</summary>
public sealed class DeploymentServerClassFilter
{
	/// <summary>Describe the server class with respect to this member client (its GUID).</summary>
	[AliasAs("clientId")]
	public string? ClientId { get; init; }

	/// <summary>Only a server class with (or without) a deployment error.</summary>
	[AliasAs("hasDeploymentError")]
	public bool? HasDeploymentError { get; init; }
}
