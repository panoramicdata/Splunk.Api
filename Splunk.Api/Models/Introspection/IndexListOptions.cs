using Refit;

namespace Splunk.Api.Models.Introspection;

/// <summary>Paging, filtering and the data type for <c>GET data/indexes</c> and <c>GET data/indexes-extended</c>.</summary>
public sealed class IndexListOptions : ListOptions
{
	/// <summary>Which indexes to list: event (Splunk's default), metric or all (<c>datatype</c>).</summary>
	[AliasAs("datatype")]
	public IndexDataType? DataType { get; init; }
}
