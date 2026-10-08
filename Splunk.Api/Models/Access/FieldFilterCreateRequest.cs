using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates a field filter (<c>POST authorization/fieldfilters</c>).</summary>
public sealed class FieldFilterCreateRequest : FieldFilterSettings
{
	/// <summary>The filter name: letters, digits and underscores only.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
