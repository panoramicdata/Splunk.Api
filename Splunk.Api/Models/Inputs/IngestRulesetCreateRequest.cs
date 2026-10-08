using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates an ingest actions ruleset (<c>POST data/ingest/rulesets</c>).</summary>
public sealed class IngestRulesetCreateRequest : IngestRulesetUpdateRequest
{
	/// <summary>The ruleset's name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
