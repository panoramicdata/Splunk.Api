using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api;

/// <summary>The outcome of <see cref="SplunkSearch.RunAsync(string, CancellationToken)"/>: the finished job and all of its results.</summary>
public sealed class SearchRunResult
{
	/// <summary>The job as it was when it finished.</summary>
	public required SearchJob Job { get; init; }

	/// <summary>The result fields, in column order.</summary>
	public IReadOnlyList<SearchField> Fields { get; init; } = [];

	/// <summary>Every result, in order.</summary>
	public IReadOnlyList<SearchResult> Results { get; init; } = [];

	/// <summary>Messages returned with the results, such as warnings.</summary>
	public IReadOnlyList<SplunkMessage> Messages { get; init; } = [];
}
