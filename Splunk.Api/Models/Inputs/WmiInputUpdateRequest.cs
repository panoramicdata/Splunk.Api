using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a WMI collection (<c>POST data/inputs/win-wmi-collections/{name}</c>). Windows only.</summary>
public class WmiInputUpdateRequest : SplunkFormRequest
{
	/// <summary>A WMI class name.</summary>
	[JsonPropertyName("classes")]
	public required string Classes { get; init; }

	/// <summary>Seconds between queries.</summary>
	[JsonPropertyName("interval")]
	public required int Interval { get; init; }

	/// <summary>The first server to collect from (<c>lookup_host</c>).</summary>
	[JsonPropertyName("lookup_host")]
	public required string LookupHost { get; init; }

	/// <summary>Whether the collection is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The properties to collect, sent as repeated <c>fields</c> fields; all when unset.</summary>
	[JsonPropertyName("fields")]
	public IReadOnlyList<string>? Fields { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The class instances to collect, sent as repeated <c>instances</c> fields.</summary>
	[JsonPropertyName("instances")]
	public IReadOnlyList<string>? Instances { get; init; }

	/// <summary>Additional servers to collect from, comma-separated.</summary>
	[JsonPropertyName("server")]
	public string? Server { get; init; }
}
