using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes an Active Directory monitoring input (<c>POST data/inputs/ad/{name}</c>). Windows only.</summary>
public class ActiveDirectoryInputUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether subtrees of the starting node are monitored (<c>monitorSubtree</c>).</summary>
	[JsonPropertyName("monitorSubtree")]
	public required bool MonitorSubtree { get; init; }

	/// <summary>Whether baseline objects (including deleted ones) are queried; Splunk's default is <see langword="true"/>.</summary>
	[JsonPropertyName("baseline")]
	public bool? Baseline { get; init; }

	/// <summary>The host field for events.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>Whether the schema is printed (<c>printSchema</c>); Splunk's default is <see langword="true"/>.</summary>
	[JsonPropertyName("printSchema")]
	public bool? PrintSchema { get; init; }

	/// <summary>The source field for events.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype for events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }

	/// <summary>Where in the directory tree monitoring starts (<c>startingNode</c>).</summary>
	[JsonPropertyName("startingNode")]
	public string? StartingNode { get; init; }

	/// <summary>The fully qualified name of the domain controller to monitor (<c>targetDc</c>).</summary>
	[JsonPropertyName("targetDc")]
	public string? TargetDomainController { get; init; }
}
