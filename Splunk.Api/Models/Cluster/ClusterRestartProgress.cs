using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The progress of a rolling restart: the peers in each state.</summary>
public sealed class ClusterRestartProgress
{
	/// <summary>Peers that restarted.</summary>
	[JsonPropertyName("done")]
	public IReadOnlyList<string> Done { get; init; } = [];

	/// <summary>Peers that failed to restart.</summary>
	[JsonPropertyName("failed")]
	public IReadOnlyList<string> Failed { get; init; } = [];

	/// <summary>Peers restarting now.</summary>
	[JsonPropertyName("in_progress")]
	public IReadOnlyList<string> InProgress { get; init; } = [];

	/// <summary>Peers still to restart.</summary>
	[JsonPropertyName("to_be_restarted")]
	public IReadOnlyList<string> ToBeRestarted { get; init; } = [];
}
