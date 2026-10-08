using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>
/// Adds a receiver (<c>POST data/outputs/tcp/server</c>). When no default group is set, Splunk puts the receiver in a
/// new <c>default-autolb-group</c> and makes it the default, so this server starts forwarding to it.
/// </summary>
public sealed class TcpOutputServerCreateRequest : TcpOutputServerUpdateRequest
{
	/// <summary>The receiver as <c>host:port</c>.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
