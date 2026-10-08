using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A lookup table file (<c>data/lookup-table-files</c>).</summary>
public sealed class LookupTableFile : SplunkContent
{
	/// <summary>The file's path on the Splunk server (<c>eai:data</c>).</summary>
	[JsonPropertyName("eai:data")]
	public string? Path { get; init; }

	/// <summary>The file's column names (<c>fields_array</c>); empty when Splunk reports <c>null</c>.</summary>
	[JsonPropertyName("fields_array")]
	public IReadOnlyList<string> Fields { get; init => field = value ?? []; } = [];

	/// <summary>The file's size in bytes.</summary>
	[JsonPropertyName("size")]
	public long? Size { get; init; }

	/// <summary>When the file was last modified (<c>lastModifiedTime</c>).</summary>
	[JsonPropertyName("lastModifiedTime")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastModifiedTime { get; init; }
}
