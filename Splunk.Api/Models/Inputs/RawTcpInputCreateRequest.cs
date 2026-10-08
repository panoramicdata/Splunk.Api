using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a raw TCP input (<c>POST data/inputs/tcp/raw</c>).</summary>
public sealed class RawTcpInputCreateRequest : RawTcpInputUpdateRequest
{
	/// <summary>The port to listen on.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
