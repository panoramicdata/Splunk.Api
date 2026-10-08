using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>
/// The reply to an SPL2 dispatch: each statement's search job. Read results with
/// <see cref="Interfaces.ISearchJobResults.GetResultsAsync"/> and the statement's <see cref="Spl2DispatchedQuery.Sid"/>.
/// </summary>
public sealed class Spl2DispatchResult
{
	/// <summary>The module source that was run.</summary>
	[JsonPropertyName("module")]
	public string? Module { get; init; }

	/// <summary>The namespace it ran in.</summary>
	[JsonPropertyName("namespace")]
	public string? Namespace { get; init; }

	/// <summary>Each statement's job, keyed by statement name.</summary>
	[JsonPropertyName("queryParameters")]
	public IReadOnlyDictionary<string, Spl2DispatchedQuery> QueryParameters { get; init; } = new Dictionary<string, Spl2DispatchedQuery>();

	/// <summary>Work-in-progress modules the dispatch used, if any.</summary>
	[JsonPropertyName("wipModules")]
	public JsonElement? WipModules { get; init; }
}
