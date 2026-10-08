using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A server class a deployment client belongs to.</summary>
public sealed class DeploymentClientServerClass
{
	/// <summary>When the server class was loaded.</summary>
	[JsonPropertyName("loadTime")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LoadTime { get; init; }

	/// <summary>Where the deployment server keeps the server class content.</summary>
	[JsonPropertyName("repositoryLocation")]
	public string? RepositoryLocation { get; init; }

	/// <summary>Whether the client restarts Splunk Web when an app changes.</summary>
	[JsonPropertyName("restartSplunkWeb")]
	public bool? RestartSplunkWeb { get; init; }

	/// <summary>Whether the client restarts splunkd when an app changes.</summary>
	[JsonPropertyName("restartSplunkd")]
	public bool? RestartSplunkd { get; init; }

	/// <summary>The state apps get on the client.</summary>
	[JsonPropertyName("stateOnClient")]
	public DeploymentAppState? StateOnClient { get; init; }
}
