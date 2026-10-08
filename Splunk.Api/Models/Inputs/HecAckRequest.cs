using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>The acknowledgement IDs to query (<c>POST services/collector/ack</c>), sent as JSON.</summary>
public sealed class HecAckRequest
{
	/// <summary>The acknowledgement IDs returned when the events were sent.</summary>
	[JsonPropertyName("acks")]
	public required IReadOnlyList<long> Acks { get; init; }
}
