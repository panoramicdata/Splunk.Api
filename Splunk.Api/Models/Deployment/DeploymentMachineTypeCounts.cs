using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The number of deployment clients per machine type (<c>deployment/server/clients/countClients_by_machineType</c>).</summary>
public sealed class DeploymentMachineTypeCounts : SplunkContent
{
	/// <summary>The number of clients by machine type, for example <c>linux-x86_64</c>; <see langword="null"/> when there are no clients.</summary>
	[JsonPropertyName("counts")]
	public IReadOnlyDictionary<string, int>? Counts { get; init; }
}
