using Refit;

namespace Splunk.Api.Models.Inputs;

/// <summary>The channel of an HTTP Event Collector request, sent in the query string.</summary>
public class HecChannelOptions
{
	/// <summary>The channel (a GUID); overrides <see cref="SplunkHecClientOptions.Channel"/> for this request.</summary>
	[AliasAs("channel")]
	public string? Channel { get; init; }
}
