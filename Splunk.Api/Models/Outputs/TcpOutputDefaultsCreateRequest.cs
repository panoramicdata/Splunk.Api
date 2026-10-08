using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>Configures the global forwarding settings (<c>POST data/outputs/tcp/default</c>).</summary>
public sealed class TcpOutputDefaultsCreateRequest : TcpOutputDefaultsUpdateRequest
{
	/// <summary>The stanza to configure; the only valid value is <c>tcpout</c>.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
