using Refit;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>Time argument parsing (<c>search/timeparser</c>): resolves relative and absolute times as Splunk would.</summary>
public interface ISearchTimeParser
{
	/// <summary>Resolves time arguments to absolute times (<c>GET search/timeparser</c>).</summary>
	/// <param name="times">The times to resolve (<c>time</c>, repeated), for example <c>-1d@d</c>, <c>now</c> or an absolute time.</param>
	/// <param name="options">The reference time and formats; <see langword="null"/> for ISO 8601 relative to now.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Each input time mapped to its resolved time, formatted by <see cref="TimeParserOptions.OutputTimeFormat"/>.</returns>
	/// <remarks>An unrecognisable time raises <see cref="SplunkApiException"/> (400, <c>Invalid time.</c>).</remarks>
	[Get("services/search/timeparser")]
	Task<IReadOnlyDictionary<string, string>> ParseAsync(
		[Query(CollectionFormat.Multi)][AliasAs("time")] IEnumerable<string> times,
		[Query] TimeParserOptions? options,
		CancellationToken cancellationToken);
}
