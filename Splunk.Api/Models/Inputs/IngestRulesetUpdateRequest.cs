using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes an ingest actions ruleset (<c>POST data/ingest/rulesets/{name}</c>).</summary>
/// <remarks>Splunk finds the ruleset by name and sourcetype, so <see cref="Sourcetype"/> must be the ruleset's current one.</remarks>
public class IngestRulesetUpdateRequest : SplunkFormRequest
{
	/// <summary>The sourcetype whose events the rules apply to.</summary>
	[JsonPropertyName("sourcetype")]
	public required string Sourcetype { get; init; }

	/// <summary>A description of the ruleset.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The rules, applied in order. Sent as one JSON array (<see cref="RulesJson"/>).</summary>
	[JsonIgnore]
	public IReadOnlyList<IngestRule>? Rules { get; init; }

	/// <summary>The rules as the JSON array Splunk receives in the <c>rules</c> field.</summary>
	[JsonPropertyName("rules")]
	public string? RulesJson => Rules is null ? null : JsonSerializer.Serialize(Rules, SplunkJson.Options);
}
