using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>An index with bucket-level size information (<c>data/indexes-extended</c>).</summary>
public sealed class SplunkIndexExtended : SplunkIndex
{
	/// <summary>The index name (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>The total size of the index's buckets in megabytes (<c>total_size</c>).</summary>
	[JsonPropertyName("total_size")]
	public double? TotalSizeMB { get; init; }
}
