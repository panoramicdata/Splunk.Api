using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a cooked (forwarder) TCP input (<c>POST data/inputs/tcp/cooked/{name}</c>). Unset properties are left unchanged.</summary>
public class CookedTcpInputUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether the input uses SSL (<c>SSL</c>); SSL must already be configured.</summary>
	[JsonPropertyName("SSL")]
	public bool? Ssl { get; init; }

	/// <summary>How the host field is set from the sender (<c>connection_host</c>).</summary>
	[JsonPropertyName("connection_host")]
	public ConnectionHost? ConnectionHost { get; init; }

	/// <summary>Whether the input is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The host field for events that lack one.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The only host the input accepts connections from (<c>restrictToHost</c>).</summary>
	[JsonPropertyName("restrictToHost")]
	public string? RestrictToHost { get; init; }
}
