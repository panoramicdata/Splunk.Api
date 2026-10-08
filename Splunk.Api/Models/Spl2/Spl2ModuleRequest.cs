using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>An SPL2 module to create or replace (<c>PUT orchestrator/v1/spl2/modules/{resourceName}</c>), sent as JSON.</summary>
public sealed class Spl2ModuleRequest
{
	/// <summary>The module name (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The namespace (<c>namespace</c>), for example <c>apps.search</c>.</summary>
	[JsonPropertyName("namespace")]
	public required string Namespace { get; init; }

	/// <summary>The whole SPL2 source of the module (<c>definition</c>), for example <c>$s = from main | head 10; export {$s}</c>.</summary>
	[JsonPropertyName("definition")]
	public required string Definition { get; init; }

	/// <summary>The description (<c>description</c>).</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The display name (<c>displayName</c>).</summary>
	[JsonPropertyName("displayName")]
	public string? DisplayName { get; init; }

	/// <summary>The event sampling setting (<c>eventSampling</c>).</summary>
	[JsonPropertyName("eventSampling")]
	public string? EventSampling { get; init; }

	/// <summary>The default earliest time (<c>earliestTime</c>).</summary>
	[JsonPropertyName("earliestTime")]
	public string? EarliestTime { get; init; }

	/// <summary>The default latest time (<c>latestTime</c>).</summary>
	[JsonPropertyName("latestTime")]
	public string? LatestTime { get; init; }
}
