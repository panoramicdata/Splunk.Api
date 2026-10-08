using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes an HTTP Event Collector token (<c>POST data/inputs/http/{name}</c>). Unset properties are left unchanged.</summary>
public class HecTokenUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether the token is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The default host field for events sent with the token.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The default index for events sent with the token.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The indexes events sent with the token may name (sent as repeated <c>indexes</c> fields).</summary>
	[JsonPropertyName("indexes")]
	public IReadOnlyList<string>? Indexes { get; init; }

	/// <summary>The default source field for events sent with the token.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The default sourcetype for events sent with the token.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }

	/// <summary>Whether indexer acknowledgement is on (<c>useACK</c>).</summary>
	[JsonPropertyName("useACK")]
	public bool? UseAck { get; init; }

	/// <summary>A description of the token.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }
}
