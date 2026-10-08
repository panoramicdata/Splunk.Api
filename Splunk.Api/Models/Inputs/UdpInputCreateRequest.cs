using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a UDP input (<c>POST data/inputs/udp</c>).</summary>
public sealed class UdpInputCreateRequest : UdpInputUpdateRequest
{
	/// <summary>The UDP port to listen on.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
