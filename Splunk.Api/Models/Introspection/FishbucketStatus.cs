using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The fishbucket, the database tracking how far each monitored file has been read (<c>server/status/fishbucket</c>).</summary>
public sealed class FishbucketStatus : SplunkContent
{
	/// <summary>The number of tracked files (<c>key_count</c>).</summary>
	[JsonPropertyName("key_count")]
	public long KeyCount { get; init; }

	/// <summary>The database size in megabytes (<c>total_size</c>).</summary>
	[JsonPropertyName("total_size")]
	public double TotalSizeMB { get; init; }
}
