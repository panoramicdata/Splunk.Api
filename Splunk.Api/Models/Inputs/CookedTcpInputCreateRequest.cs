using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a cooked (forwarder) TCP input, a receiving port (<c>POST data/inputs/tcp/cooked</c>).</summary>
public sealed class CookedTcpInputCreateRequest : CookedTcpInputUpdateRequest
{
	/// <summary>The port to listen on.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
