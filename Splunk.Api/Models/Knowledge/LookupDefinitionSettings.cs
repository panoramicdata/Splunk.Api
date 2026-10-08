using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>The settings of a lookup definition, shared by <see cref="LookupDefinitionCreateRequest"/> and <see cref="LookupDefinitionUpdateRequest"/>.</summary>
/// <remarks>Set <see cref="FileName"/> for a file lookup, <see cref="ExternalCommand"/> and <see cref="FieldsList"/> for an external lookup, or <see cref="Collection"/> with <see cref="ExternalType"/> <c>kvstore</c> for a KV store lookup.</remarks>
public abstract class LookupDefinitionSettings : SplunkFormRequest
{
	/// <summary>The lookup table file name, for a file lookup.</summary>
	[JsonPropertyName("filename")]
	public string? FileName { get; init; }

	/// <summary>The KV store collection, for a KV store lookup (with <see cref="ExternalType"/> <c>kvstore</c>).</summary>
	[JsonPropertyName("collection")]
	public string? Collection { get; init; }

	/// <summary>The command and arguments of an external (scripted) lookup, for example <c>external_lookup.py clienthost clientip</c>.</summary>
	[JsonPropertyName("external_cmd")]
	public string? ExternalCommand { get; init; }

	/// <summary>The kind of external lookup: <c>python</c> (the default), <c>executable</c>, <c>kvstore</c> or <c>geo</c>.</summary>
	[JsonPropertyName("external_type")]
	public string? ExternalType { get; init; }

	/// <summary>The fields the lookup supports, comma-separated: required for external and KV store lookups.</summary>
	[JsonPropertyName("fields_list")]
	public string? FieldsList { get; init; }

	/// <summary>The value returned when fewer than <see cref="MinMatches"/> entries match.</summary>
	[JsonPropertyName("default_match")]
	public string? DefaultMatch { get; init; }

	/// <summary>The maximum number of matches returned for each input value.</summary>
	[JsonPropertyName("max_matches")]
	public int? MaxMatches { get; init; }

	/// <summary>The minimum number of matches returned for each input value.</summary>
	[JsonPropertyName("min_matches")]
	public int? MinMatches { get; init; }

	/// <summary>For temporal lookups, the lookup table field holding the timestamp.</summary>
	[JsonPropertyName("time_field")]
	public string? TimeField { get; init; }

	/// <summary>For temporal lookups, the <c>strptime</c> format of <see cref="TimeField"/>.</summary>
	[JsonPropertyName("time_format")]
	public string? TimeFormat { get; init; }

	/// <summary>For temporal lookups, the maximum number of seconds an event may be later than the lookup entry.</summary>
	[JsonPropertyName("max_offset_secs")]
	public long? MaxOffsetSeconds { get; init; }

	/// <summary>For temporal lookups, the minimum number of seconds an event must be later than the lookup entry.</summary>
	[JsonPropertyName("min_offset_secs")]
	public long? MinOffsetSeconds { get; init; }

	/// <summary>Whether matching is case-sensitive.</summary>
	[JsonPropertyName("case_sensitive_match")]
	public bool? CaseSensitiveMatch { get; init; }

	/// <summary>Non-exact match rules, for example <c>WILDCARD(url)</c> or <c>CIDR(ip)</c>.</summary>
	[JsonPropertyName("match_type")]
	public string? MatchType { get; init; }

	/// <summary>Whether only changes to a CSV lookup are replicated to search peers.</summary>
	[JsonPropertyName("replicate_delta")]
	public bool? ReplicateDelta { get; init; }

	/// <summary>Whether the lookup definition is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }
}
