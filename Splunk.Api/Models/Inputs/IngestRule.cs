using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>One rule of an ingest actions ruleset. Action-specific properties (such as <c>dest</c>) are in <see cref="AdditionalProperties"/>.</summary>
public sealed class IngestRule
{
	/// <summary>The rule's name.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>What the rule does, for example <c>filter</c>, <c>route</c> or <c>mask</c>.</summary>
	[JsonPropertyName("action")]
	public string? Action { get; init; }

	/// <summary>Which events the rule applies to.</summary>
	[JsonPropertyName("cond")]
	public IngestRuleCondition? Condition { get; init; }

	/// <summary>Any other properties of the rule, keyed by wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
