using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>Moves a knowledge object to another app or user context (<c>POST {endpoint}/{name}/move</c>).</summary>
public sealed class MoveRequest : SplunkFormRequest
{
	/// <summary>The app to move the object to.</summary>
	[JsonPropertyName("app")]
	public required string App { get; init; }

	/// <summary>The user to move the object to, or <c>nobody</c>.</summary>
	[JsonPropertyName("user")]
	public required string User { get; init; }
}
