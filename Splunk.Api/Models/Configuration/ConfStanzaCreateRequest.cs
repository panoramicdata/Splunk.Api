using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Configuration;

/// <summary>
/// Creates a stanza in a <c>.conf</c> file (<c>POST configs/conf-{file}</c>). Put the stanza's keys and values in
/// <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public sealed class ConfStanzaCreateRequest : SplunkFormRequest
{
	/// <summary>The stanza name (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
