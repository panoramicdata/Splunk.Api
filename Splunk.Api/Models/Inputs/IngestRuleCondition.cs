using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>The condition of an ingest actions rule (<c>cond</c>).</summary>
public sealed class IngestRuleCondition
{
	/// <summary>The kind of match, for example <c>regex</c> or <c>eval</c>.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>The event field matched, for example <c>_raw</c>.</summary>
	[JsonPropertyName("field")]
	public string? Field { get; init; }

	/// <summary>The expression to match.</summary>
	[JsonPropertyName("match")]
	public string? Match { get; init; }
}
