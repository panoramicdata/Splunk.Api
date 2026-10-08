using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>
/// The paging parameters of a job's results and events (GET). Search result endpoints do not use
/// <see cref="ListOptions"/>; leave a property <see langword="null"/> for Splunk's default.
/// </summary>
public abstract class SearchPageOptions
{
	/// <summary>The most rows to return (<c>count</c>); Splunk's default is 100, and 0 returns every available row.</summary>
	[AliasAs("count")]
	public int? Count { get; init; }

	/// <summary>The index of the first row (<c>offset</c>); a negative offset counts back from the end.</summary>
	[AliasAs("offset")]
	public int? Offset { get; init; }

	/// <summary>The fields to return (<c>f</c>, repeated); every field by default.</summary>
	[AliasAs("f")]
	[Query(CollectionFormat.Multi)]
	public IEnumerable<string>? Fields { get; init; }
}
