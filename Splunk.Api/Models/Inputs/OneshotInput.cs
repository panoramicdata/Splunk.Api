using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// A file being indexed once (<c>data/inputs/oneshot</c>). The entry name is the file's path. Splunk lists a oneshot
/// input only while it is in progress.
/// </summary>
public sealed class OneshotInput : SplunkContent
{
	/// <summary>The bytes read so far, uncompressed (<c>Bytes Indexed</c>).</summary>
	[JsonPropertyName("Bytes Indexed")]
	public long? BytesIndexed { get; init; }

	/// <summary>The current position in the file (<c>Offset</c>).</summary>
	[JsonPropertyName("Offset")]
	public long? Offset { get; init; }

	/// <summary>The file's size in bytes (<c>Size</c>).</summary>
	[JsonPropertyName("Size")]
	public long? Size { get; init; }

	/// <summary>The number of sources read from an archive; 0 for a plain file (<c>Sources Indexed</c>).</summary>
	[JsonPropertyName("Sources Indexed")]
	public int? SourcesIndexed { get; init; }

	/// <summary>When the file was queued, as Splunk formats it, for example <c>Thu Oct  8 14:02:38 UTC 2026</c> (<c>Spool Time</c>).</summary>
	[JsonPropertyName("Spool Time")]
	public string? SpoolTime { get; init; }
}
