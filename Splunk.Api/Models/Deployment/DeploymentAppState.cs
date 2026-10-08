using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The state a deployed app gets on deployment clients.</summary>
public enum DeploymentAppState
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>Enabled on the client, whatever its state on the deployment server (the default).</summary>
	[JsonStringEnumMemberName("enabled")]
	Enabled,

	/// <summary>Disabled on the client, whatever its state on the deployment server.</summary>
	[JsonStringEnumMemberName("disabled")]
	Disabled,

	/// <summary>The same state on the client as on the deployment server.</summary>
	[JsonStringEnumMemberName("noop")]
	Noop
}
