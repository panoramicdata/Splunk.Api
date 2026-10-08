using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Configuration;

/// <summary>Sets one key of a <c>.conf</c> stanza (<c>POST properties/{file}/{stanza}/{key}</c>).</summary>
public sealed class PropertyValueRequest : SplunkFormRequest
{
	/// <summary>The value to write (<c>value</c>).</summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }
}
