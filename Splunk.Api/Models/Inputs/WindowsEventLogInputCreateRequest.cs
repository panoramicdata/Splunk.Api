using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a Windows event log collection (<c>POST data/inputs/win-event-log-collections</c>). Windows only.</summary>
public sealed class WindowsEventLogInputCreateRequest : WindowsEventLogInputUpdateRequest
{
	/// <summary>The collection's name; <c>localhost</c> reads the local event logs natively, any other name uses WMI.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
