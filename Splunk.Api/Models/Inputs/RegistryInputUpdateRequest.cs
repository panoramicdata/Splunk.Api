using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a registry monitoring input (<c>POST data/inputs/registry/{name}</c>). Windows only.</summary>
public class RegistryInputUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether to establish a baseline of the registry keys.</summary>
	[JsonPropertyName("baseline")]
	public required bool Baseline { get; init; }

	/// <summary>The registry hive to monitor, for example <c>HKLM</c>.</summary>
	[JsonPropertyName("hive")]
	public required string Hive { get; init; }

	/// <summary>A regular expression for the processes whose registry access is monitored (<c>proc</c>).</summary>
	[JsonPropertyName("proc")]
	public required string Process { get; init; }

	/// <summary>The registry event types to monitor, separated by <c>|</c>, for example <c>set|create|delete|rename</c>.</summary>
	[JsonPropertyName("type")]
	public required string Type { get; init; }

	/// <summary>Whether the input is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>Whether every key below the hive is monitored (<c>monitorSubnodes</c>).</summary>
	[JsonPropertyName("monitorSubnodes")]
	public bool? MonitorSubnodes { get; init; }
}
