using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates an HTTP Event Collector token (<c>POST data/inputs/http</c>). Splunk generates the token value.</summary>
public sealed class HecTokenCreateRequest : HecTokenUpdateRequest
{
	/// <summary>The token's name; Splunk lists it as <c>http://{name}</c>.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
