using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>An Active Directory monitoring input (<c>data/inputs/ad</c>). Windows only.</summary>
public sealed class ActiveDirectoryInput : InputContent
{
	/// <summary>Whether subtrees of <see cref="StartingNode"/> are monitored (<c>monitorSubtree</c>).</summary>
	[JsonPropertyName("monitorSubtree")]
	public bool? MonitorSubtree { get; init; }

	/// <summary>Where in the directory tree monitoring starts (<c>startingNode</c>); the root when unset.</summary>
	[JsonPropertyName("startingNode")]
	public string? StartingNode { get; init; }

	/// <summary>The domain controller monitored (<c>targetDc</c>); the local one when unset.</summary>
	[JsonPropertyName("targetDc")]
	public string? TargetDomainController { get; init; }

	/// <summary>Whether baseline objects are queried.</summary>
	[JsonPropertyName("baseline")]
	public bool? Baseline { get; init; }

	/// <summary>Whether the schema is printed (<c>printSchema</c>).</summary>
	[JsonPropertyName("printSchema")]
	public bool? PrintSchema { get; init; }
}
