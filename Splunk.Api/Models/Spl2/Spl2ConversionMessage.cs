using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>A warning or error from converting SPL to SPL2.</summary>
public sealed class Spl2ConversionMessage
{
	/// <summary>The type: <c>error</c> or <c>warn</c>.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>A code, for example <c>SPL2_CONVERT_FAIL</c>.</summary>
	[JsonPropertyName("code")]
	public string? Code { get; init; }

	/// <summary>The message text.</summary>
	[JsonPropertyName("message")]
	public string? Text { get; init; }
}
