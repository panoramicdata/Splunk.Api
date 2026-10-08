using Refit;

namespace Splunk.Api.Models.Inputs;

/// <summary>What an HTTP Event Collector health check covers, sent in the query string.</summary>
public sealed class HecHealthOptions
{
	/// <summary>Whether to check the acknowledgement service too (<c>ack</c>).</summary>
	[AliasAs("ack")]
	public bool? Ack { get; init; }

	/// <summary>A token whose queue to check (<c>token</c>); all tokens when unset.</summary>
	[AliasAs("token")]
	public string? Token { get; init; }
}
