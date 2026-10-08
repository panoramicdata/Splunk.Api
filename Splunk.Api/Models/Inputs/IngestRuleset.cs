using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>An ingest actions ruleset (<c>data/ingest/rulesets</c>): rules applied to one sourcetype's events at ingest time.</summary>
public sealed class IngestRuleset : SplunkContent
{
	/// <summary>The ruleset's name.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>A description of the ruleset.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The sourcetype whose events the rules apply to.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }

	/// <summary>The rules, applied in order.</summary>
	[JsonPropertyName("rules")]
	public IReadOnlyList<IngestRule> Rules { get; init; } = [];
}
