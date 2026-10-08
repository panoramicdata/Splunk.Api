using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>
/// Starts the named search statements of an SPL2 module (<c>POST orchestrator/v1/spl2/modules/dispatch</c>), sent as JSON.
/// </summary>
public sealed class Spl2DispatchRequest
{
	/// <summary>The module source, exporting the statements to run (<c>module</c>), for example <c>$s = from _internal | head 3; export {$s}</c>.</summary>
	[JsonPropertyName("module")]
	public required string Module { get; init; }

	/// <summary>The namespace the module runs in (<c>namespace</c>), for example <c>apps.search</c>.</summary>
	[JsonPropertyName("namespace")]
	public string? Namespace { get; init; }

	/// <summary>The statements to run, keyed by statement name without the <c>$</c>, with their time range and options (<c>queryParameters</c>).</summary>
	[JsonPropertyName("queryParameters")]
	public required IReadOnlyDictionary<string, Spl2DispatchQuery> QueryParameters { get; init; }
}
