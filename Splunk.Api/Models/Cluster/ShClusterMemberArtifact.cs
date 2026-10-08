using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A search artifact on this search head cluster member (<c>shcluster/member/artifacts</c>).</summary>
public sealed class ShClusterMemberArtifact : SplunkContent
{
	/// <summary>The artifact status, for example <c>Complete</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }
}
