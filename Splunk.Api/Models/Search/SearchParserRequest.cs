using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Parses a search without running it (<c>POST search/v2/parser</c>).</summary>
public sealed class SearchParserRequest : SplunkFormRequest
{
	/// <summary>The search to parse (<c>q</c>), for example <c>search index=_internal | stats count by host</c>.</summary>
	[JsonPropertyName("q")]
	public required string Query { get; init; }

	/// <summary>Whether to check syntax only, without expanding macros or optimizing (<c>parse_only</c>).</summary>
	[JsonPropertyName("parse_only")]
	public bool? ParseOnly { get; init; }

	/// <summary>Whether lookups are taken into account (<c>enable_lookups</c>).</summary>
	[JsonPropertyName("enable_lookups")]
	public bool? EnableLookups { get; init; }

	/// <summary>Whether macro definitions are reloaded from macros.conf (<c>reload_macros</c>).</summary>
	[JsonPropertyName("reload_macros")]
	public bool? ReloadMacros { get; init; }
}
