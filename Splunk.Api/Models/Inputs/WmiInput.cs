using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A WMI collection (<c>data/inputs/win-wmi-collections</c>). Windows only.</summary>
public sealed class WmiInput : InputContent
{
	/// <summary>The WMI class collected (<c>classes</c>; reads also report <c>class</c>, kept in additional properties).</summary>
	[JsonPropertyName("classes")]
	public string? Classes { get; init; }

	/// <summary>The properties collected from the class.</summary>
	[JsonPropertyName("fields")]
	[JsonConverter(typeof(TolerantStringListConverter))]
	public IReadOnlyList<string> Fields { get; init; } = [];

	/// <summary>The class instances collected.</summary>
	[JsonPropertyName("instances")]
	[JsonConverter(typeof(TolerantStringListConverter))]
	public IReadOnlyList<string> Instances { get; init; } = [];

	/// <summary>Seconds between queries.</summary>
	[JsonPropertyName("interval")]
	public int? Interval { get; init; }

	/// <summary>The first server collected from (<c>lookup_host</c>).</summary>
	[JsonPropertyName("lookup_host")]
	public string? LookupHost { get; init; }

	/// <summary>The servers collected from, comma-separated.</summary>
	[JsonPropertyName("server")]
	public string? Server { get; init; }

	/// <summary>The WQL query Splunk runs.</summary>
	[JsonPropertyName("wql")]
	public string? Wql { get; init; }
}
