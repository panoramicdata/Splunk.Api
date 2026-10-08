using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>The indexing status of acknowledgement IDs.</summary>
public sealed class HecAckResponse
{
	/// <summary>
	/// For each ID, <see langword="true"/> once its events are indexed; <see langword="false"/> means not yet known
	/// (query again later).
	/// </summary>
	[JsonPropertyName("acks")]
	public IReadOnlyDictionary<long, bool> Acks { get; init; } = new Dictionary<long, bool>();
}
