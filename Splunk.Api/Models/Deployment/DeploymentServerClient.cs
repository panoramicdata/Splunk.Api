using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A deployment client known to the deployment server (<c>deployment/server/clients</c>).</summary>
public sealed class DeploymentServerClient : SplunkContent
{
	/// <summary>The apps deployed to the client, by app name.</summary>
	[JsonPropertyName("applications")]
	public IReadOnlyDictionary<string, DeploymentClientApplication> Applications { get; init; } = new Dictionary<string, DeploymentClientApplication>();

	/// <summary>The average phone home interval, in seconds.</summary>
	[JsonPropertyName("averagePhoneHomeInterval")]
	public int? AveragePhoneHomeInterval { get; init; }

	/// <summary>The Splunk build of the client.</summary>
	[JsonPropertyName("build")]
	public string? Build { get; init; }

	/// <summary>The DNS name of the client.</summary>
	[JsonPropertyName("dns")]
	public string? Dns { get; init; }

	/// <summary>The identifier of the client record.</summary>
	[JsonPropertyName("guid")]
	public string? ClientGuid { get; init; }

	/// <summary>The client name (<c>clientName</c> in deploymentclient.conf).</summary>
	[JsonPropertyName("clientName")]
	public string? ClientName { get; init; }

	/// <summary>Whether a deployment to the client failed.</summary>
	[JsonPropertyName("hasDeploymentError")]
	public bool? HasDeploymentError { get; init; }

	/// <summary>The host name of the client.</summary>
	[JsonPropertyName("hostname")]
	public string? Hostname { get; init; }

	/// <summary>The connection identifier, built from the client name and address.</summary>
	[JsonPropertyName("id")]
	public string? ConnectionId { get; init; }

	/// <summary>The IP address of the client.</summary>
	[JsonPropertyName("ip")]
	public string? Ip { get; init; }

	/// <summary>When the client last phoned home.</summary>
	[JsonPropertyName("lastPhoneHomeTime")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastPhoneHomeTime { get; init; }

	/// <summary>The management port of the client.</summary>
	[JsonPropertyName("mgmt")]
	public int? ManagementPort { get; init; }

	/// <summary>The name of the client.</summary>
	[JsonPropertyName("name")]
	public string? ClientDisplayName { get; init; }

	/// <summary>The server classes the client belongs to, by name.</summary>
	[JsonPropertyName("serverClasses")]
	public IReadOnlyDictionary<string, DeploymentClientServerClass> ServerClasses { get; init; } = new Dictionary<string, DeploymentClientServerClass>();

	/// <summary>The machine type of the client, for example <c>linux-x86_64</c>.</summary>
	[JsonPropertyName("utsname")]
	public string? UtsName { get; init; }
}
