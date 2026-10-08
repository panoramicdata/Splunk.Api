using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a lookup definition (<c>POST data/transforms/lookups</c>).</summary>
/// <remarks>
/// Splunk accepts a file lookup whose file does not exist, but then hides it: it is not listed and cannot be read,
/// changed or deleted through <c>data/transforms/lookups</c> until the file exists.
/// </remarks>
public sealed class LookupDefinitionCreateRequest : LookupDefinitionSettings
{
	/// <summary>The lookup definition (<c>transforms.conf</c> stanza) name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
