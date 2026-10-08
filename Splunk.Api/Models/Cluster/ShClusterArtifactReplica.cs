using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A replica of a search artifact on one search head cluster member.</summary>
public sealed class ShClusterArtifactReplica
{
	/// <summary>The dispatch directory of the replica.</summary>
	[JsonPropertyName("directory_path")]
	public string? DirectoryPath { get; init; }

	/// <summary>The replica status, for example <c>Complete</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }
}
