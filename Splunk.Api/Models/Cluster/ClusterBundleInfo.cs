using Splunk.Api.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A configuration bundle of the cluster.</summary>
public sealed class ClusterBundleInfo
{
	/// <summary>The bundle file.</summary>
	[JsonPropertyName("bundle_path")]
	public string? BundlePath { get; init; }

	/// <summary>The bundle checksum.</summary>
	[JsonPropertyName("checksum")]
	public string? Checksum { get; init; }

	/// <summary>When the bundle was created.</summary>
	[JsonPropertyName("timestamp")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? Timestamp { get; init; }

	/// <summary>Whether the bundle passed validation, where reported.</summary>
	[JsonPropertyName("is_valid_bundle")]
	public bool? IsValidBundle { get; init; }

	/// <summary>Every property Splunk returned that this type does not model, keyed by its wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
