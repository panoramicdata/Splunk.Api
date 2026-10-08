using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Changes a calculated field's expression (<c>POST data/props/calcfields/{name}</c>).</summary>
public sealed class CalculatedFieldUpdateRequest : SplunkFormRequest
{
	/// <summary>The new eval expression.</summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }
}
