using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>A page of SPL2 modules (<c>GET orchestrator/v1/spl2/modules</c>).</summary>
public sealed class Spl2ModuleList
{
	/// <summary>The modules.</summary>
	[JsonPropertyName("results")]
	public IReadOnlyList<Spl2Module> Results { get; init; } = [];

	/// <summary>The total number of matching modules, when Splunk reports it.</summary>
	[JsonPropertyName("totalCount")]
	public int? TotalCount { get; init; }
}
