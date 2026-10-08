using Refit;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>Search auto-complete (<c>search/typeahead</c>).</summary>
public interface ISearchTypeahead
{
	/// <summary>Gets auto-complete suggestions for a search term (<c>GET search/typeahead</c>).</summary>
	/// <param name="prefix">The term to complete (<c>prefix</c>), for example <c>index=_</c>.</param>
	/// <param name="count">The most suggestions to return (<c>count</c>).</param>
	/// <param name="maxServers">The most search peers to consult besides the search head (<c>max_servers</c>); 0 is all, <see langword="null"/> is Splunk's default (2).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The suggestions.</returns>
	[Get("services/search/typeahead")]
	Task<TypeaheadResponse> GetAsync(
		[AliasAs("prefix")] string prefix,
		[AliasAs("count")] int count,
		[AliasAs("max_servers")] int? maxServers,
		CancellationToken cancellationToken);
}
