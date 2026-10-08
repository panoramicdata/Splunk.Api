using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A knowledge bundle of a search head (<c>search/distributed/bundle-replication-files</c>).</summary>
public sealed class BundleReplicationFile : SplunkContent
{
	/// <summary>The bundle checksum, which names the entry.</summary>
	[JsonPropertyName("checksum")]
	public string? Checksum { get; init; }

	/// <summary>The bundle file name.</summary>
	[JsonPropertyName("filename")]
	public string? FileName { get; init; }

	/// <summary>The bundle path.</summary>
	[JsonPropertyName("location")]
	public string? Location { get; init; }

	/// <summary>The bundle size, in bytes.</summary>
	[JsonPropertyName("size")]
	public long? Size { get; init; }

	/// <summary>When the bundle was created.</summary>
	[JsonPropertyName("timestamp")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? Timestamp { get; init; }
}
