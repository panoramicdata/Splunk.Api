using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The state of one app on a deployment client.</summary>
public sealed class DeploymentClientApplication
{
	/// <summary>The last action, for example <c>Install</c>, <c>Uninstall</c> or <c>Unknown</c>.</summary>
	[JsonPropertyName("action")]
	public string? Action { get; init; }

	/// <summary>The path of the app bundle on the deployment server.</summary>
	[JsonPropertyName("archive")]
	public string? Archive { get; init; }

	/// <summary>Whether the client restarts Splunk Web when the app changes.</summary>
	[JsonPropertyName("restartSplunkWeb")]
	public bool? RestartSplunkWeb { get; init; }

	/// <summary>Whether the client restarts splunkd when the app changes.</summary>
	[JsonPropertyName("restartSplunkd")]
	public bool? RestartSplunkd { get; init; }

	/// <summary>The result of the last action, for example <c>Ok</c>.</summary>
	[JsonPropertyName("result")]
	public string? Result { get; init; }

	/// <summary>The server classes that deliver the app.</summary>
	[JsonPropertyName("serverclasses")]
	public IReadOnlyList<string> ServerClasses { get; init; } = [];

	/// <summary>The size of the bundle, in bytes.</summary>
	[JsonPropertyName("size")]
	public long? Size { get; init; }

	/// <summary>The state the app gets on the client.</summary>
	[JsonPropertyName("stateOnClient")]
	public DeploymentAppState? StateOnClient { get; init; }

	/// <summary>When the action happened, as Splunk formats it.</summary>
	[JsonPropertyName("timestamp")]
	public string? Timestamp { get; init; }
}
