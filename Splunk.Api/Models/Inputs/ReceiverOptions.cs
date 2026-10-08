using Refit;

namespace Splunk.Api.Models.Inputs;

/// <summary>The metadata for events sent through <c>receivers/simple</c> or <c>receivers/stream</c>, sent in the query string.</summary>
public sealed class ReceiverOptions
{
	/// <summary>The host field for the events.</summary>
	[AliasAs("host")]
	public string? Host { get; init; }

	/// <summary>A regular expression that extracts the host from each event (<c>host_regex</c>).</summary>
	[AliasAs("host_regex")]
	public string? HostRegex { get; init; }

	/// <summary>The index the events are stored in.</summary>
	[AliasAs("index")]
	public string? Index { get; init; }

	/// <summary>The source field for the events.</summary>
	[AliasAs("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype for the events.</summary>
	[AliasAs("sourcetype")]
	public string? Sourcetype { get; init; }
}
