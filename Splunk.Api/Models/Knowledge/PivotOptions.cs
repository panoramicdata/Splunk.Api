using Refit;

namespace Splunk.Api.Models.Knowledge;

/// <summary>The pivot to translate into searches (<c>GET datamodel/pivot/{name}</c>). Set exactly one property.</summary>
public sealed class PivotOptions
{
	/// <summary>A <c>| pivot</c> search over the data model, for example <c>| pivot MyModel MyObject count(MyObject) AS c</c> (<c>pivot_search</c>).</summary>
	[AliasAs("pivot_search")]
	public string? PivotSearch { get; init; }

	/// <summary>The pivot as JSON (<c>pivot_json</c>).</summary>
	[AliasAs("pivot_json")]
	public string? PivotJson { get; init; }
}
