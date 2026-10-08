using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a raw TCP input (<c>POST data/inputs/tcp/raw/{name}</c>). Unset properties are left unchanged.</summary>
/// <remarks>Splunk resets <c>connection_host</c> to <c>dns</c> on an update that does not set it.</remarks>
public class RawTcpInputUpdateRequest : SplunkFormRequest
{
	/// <summary>How the host field is set from the sender (<c>connection_host</c>).</summary>
	[JsonPropertyName("connection_host")]
	public ConnectionHost? ConnectionHost { get; init; }

	/// <summary>Whether the input is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The host field for events.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>Where events are queued: <c>parsingQueue</c> (the default) or <c>indexQueue</c>.</summary>
	[JsonPropertyName("queue")]
	public string? Queue { get; init; }

	/// <summary>Seconds of idleness after which the last event is considered complete (<c>rawTcpDoneTimeout</c>).</summary>
	[JsonPropertyName("rawTcpDoneTimeout")]
	public int? RawTcpDoneTimeout { get; init; }

	/// <summary>The only host the input accepts connections from (<c>restrictToHost</c>).</summary>
	[JsonPropertyName("restrictToHost")]
	public string? RestrictToHost { get; init; }

	/// <summary>Whether the input uses SSL (<c>SSL</c>); SSL must already be configured.</summary>
	[JsonPropertyName("SSL")]
	public bool? Ssl { get; init; }

	/// <summary>The source field for events.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype for events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }
}
