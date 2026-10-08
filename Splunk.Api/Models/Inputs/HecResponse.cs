using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>The HTTP Event Collector's reply, for example <c>{"text":"Success","code":0,"ackId":3}</c>.</summary>
public sealed class HecResponse
{
	/// <summary>The status as text, for example <c>Success</c> or <c>HEC is healthy</c>.</summary>
	[JsonPropertyName("text")]
	public string? Text { get; init; }

	/// <summary>The status as a number: 0 for success, 17 for a healthy collector.</summary>
	[JsonPropertyName("code")]
	public int? Code { get; init; }

	/// <summary>The acknowledgement ID of the request, when the token uses indexer acknowledgement.</summary>
	[JsonPropertyName("ackId")]
	public long? AckId { get; init; }

	/// <summary>The zero-based index of the first invalid event, on a rejected batch (<c>invalid-event-number</c>).</summary>
	[JsonPropertyName("invalid-event-number")]
	public int? InvalidEventNumber { get; init; }
}
