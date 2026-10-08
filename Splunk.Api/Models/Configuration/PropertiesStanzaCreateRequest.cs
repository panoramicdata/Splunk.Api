using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Configuration;

/// <summary>Creates an empty stanza in a <c>.conf</c> file (<c>POST properties/{file}</c>).</summary>
public sealed class PropertiesStanzaCreateRequest : SplunkFormRequest
{
	/// <summary>The stanza name (<c>__stanza</c>).</summary>
	[JsonPropertyName("__stanza")]
	public required string Stanza { get; init; }
}
