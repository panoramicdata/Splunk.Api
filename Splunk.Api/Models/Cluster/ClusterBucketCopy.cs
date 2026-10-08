using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A copy of a clustered bucket on one peer.</summary>
public sealed class ClusterBucketCopy
{
	/// <summary>The bucket flags, in hexadecimal.</summary>
	[JsonPropertyName("bucket_flags")]
	public string? BucketFlags { get; init; }

	/// <summary>The copy's checksum.</summary>
	[JsonPropertyName("checksum")]
	public string? Checksum { get; init; }

	/// <summary>The checksum state, for example <c>StableCksum</c>.</summary>
	[JsonPropertyName("checksum_state")]
	public string? ChecksumState { get; init; }

	/// <summary>The search state, for example <c>Searchable</c> or <c>Unsearchable</c>.</summary>
	[JsonPropertyName("search_state")]
	public string? SearchState { get; init; }

	/// <summary>The copy status, for example <c>Complete</c>, <c>StreamingSource</c> or <c>StreamingTarget</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>Every property Splunk returned that this type does not model, keyed by its wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
