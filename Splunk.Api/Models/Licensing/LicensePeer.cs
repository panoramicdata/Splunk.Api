using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>A license peer registered with this license manager (<c>licenser/peers</c>); the entry name is its GUID.</summary>
public sealed class LicensePeer : SplunkContent
{
	/// <summary>The peer's name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The peer's management URI, for example <c>https://10.0.0.5:8089</c> (Splunk 10.4 and later).</summary>
	[JsonPropertyName("uri")]
	public string? Uri { get; init; }

	/// <summary>The pools the peer is a member of.</summary>
	[JsonPropertyName("pool_ids")]
	public IReadOnlyList<string> PoolIds { get; init; } = [];

	/// <summary>The pools the peer currently draws on.</summary>
	[JsonPropertyName("active_pool_ids")]
	public IReadOnlyList<string> ActivePoolIds { get; init; } = [];

	/// <summary>The stacks the peer is a member of.</summary>
	[JsonPropertyName("stack_ids")]
	public IReadOnlyList<string> StackIds { get; init; } = [];

	/// <summary>The pool Splunk suggests for the peer, if any.</summary>
	[JsonPropertyName("pool_suggestion")]
	public string? PoolSuggestion { get; init; }

	/// <summary>The number of license warnings issued to the peer.</summary>
	[JsonPropertyName("warning_count")]
	public int WarningCount { get; init; }
}
