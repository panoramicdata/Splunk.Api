using Refit;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// The channel and default metadata of an HTTP Event Collector request, sent in the query string. Metadata set on an
/// individual <see cref="HecEvent"/> takes precedence.
/// </summary>
public sealed class HecRequestOptions : HecChannelOptions
{
	/// <summary>The default host field.</summary>
	[AliasAs("host")]
	public string? Host { get; init; }

	/// <summary>The default index.</summary>
	[AliasAs("index")]
	public string? Index { get; init; }

	/// <summary>The default source field.</summary>
	[AliasAs("source")]
	public string? Source { get; init; }

	/// <summary>The default sourcetype.</summary>
	[AliasAs("sourcetype")]
	public string? Sourcetype { get; init; }

	/// <summary>The default time, in epoch seconds. Setting it turns <see cref="AutoExtractTimestamp"/> off.</summary>
	[AliasAs("time")]
	public long? Time { get; init; }

	/// <summary>
	/// On the event endpoint, whether events without a time take it from their text (<c>auto_extract_timestamp</c>).
	/// </summary>
	[AliasAs("auto_extract_timestamp")]
	public bool? AutoExtractTimestamp { get; init; }
}
