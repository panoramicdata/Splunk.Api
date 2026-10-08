using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Queues a file on the Splunk server for indexing once, in full (<c>POST data/inputs/oneshot</c>).</summary>
public sealed class OneshotInputCreateRequest : SplunkFormRequest
{
	/// <summary>The path of a file the Splunk server can read: plain, compressed or an archive.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The host field for events.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>A regular expression with one capture group that sets the host field from the path (<c>host_regex</c>).</summary>
	[JsonPropertyName("host_regex")]
	public string? HostRegex { get; init; }

	/// <summary>The slash-separated path segment that sets the host field (<c>host_segment</c>).</summary>
	[JsonPropertyName("host_segment")]
	public int? HostSegment { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The source field for events (<c>rename-source</c>).</summary>
	[JsonPropertyName("rename-source")]
	public string? RenameSource { get; init; }

	/// <summary>The sourcetype for events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }
}
