using Refit;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>Search language parsing (<c>search/v2/parser</c>). The v1 GET form is disabled by default since Splunk 9.0.1.</summary>
public interface ISearchParser
{
	/// <summary>Parses a search without running it (<c>POST search/v2/parser</c>).</summary>
	/// <param name="request">The search and parsing options.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The search's phases and commands. A syntax error raises <see cref="SplunkApiException"/> (400).</returns>
	[Post("services/search/v2/parser")]
	Task<SearchParseResult> ParseAsync([Body] SearchParserRequest request, CancellationToken cancellationToken);
}
