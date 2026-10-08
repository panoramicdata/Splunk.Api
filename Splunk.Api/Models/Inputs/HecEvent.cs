using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>An event sent to the HTTP Event Collector in its JSON event format.</summary>
public sealed class HecEvent
{
	/// <summary>The event: a string, or any object, which is sent as JSON.</summary>
	[JsonPropertyName("event")]
	public required object Event { get; init; }

	/// <summary>The event's time, sent as epoch seconds with milliseconds; the time of receipt when unset.</summary>
	[JsonPropertyName("time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? Time { get; init; }

	/// <summary>The host field; the token's default when unset.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The source field; the token's default when unset.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype; the token's default when unset.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }

	/// <summary>The index; the token's default when unset. It must be one the token allows.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>Index-time fields that are not part of the event text.</summary>
	[JsonPropertyName("fields")]
	public IReadOnlyDictionary<string, object>? Fields { get; init; }
}
