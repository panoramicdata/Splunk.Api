using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A Windows registry monitoring input (<c>data/inputs/registry</c>). Windows only.</summary>
public sealed class RegistryInput : InputContent
{
	/// <summary>Whether a baseline of the hive is taken when the input first starts.</summary>
	[JsonPropertyName("baseline")]
	public bool? Baseline { get; init; }

	/// <summary>A regular expression for the registry keys monitored.</summary>
	[JsonPropertyName("hive")]
	public string? Hive { get; init; }

	/// <summary>Whether every key below <see cref="Hive"/> is monitored (<c>monitorSubnodes</c>).</summary>
	[JsonPropertyName("monitorSubnodes")]
	public bool? MonitorSubnodes { get; init; }

	/// <summary>A regular expression for the processes whose registry access is monitored (<c>proc</c>).</summary>
	[JsonPropertyName("proc")]
	public string? Process { get; init; }

	/// <summary>The registry event types monitored, for example <c>set</c>, <c>create</c>, <c>delete</c>, <c>rename</c>.</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(TolerantStringListConverter))]
	public IReadOnlyList<string> Types { get; init; } = [];
}
