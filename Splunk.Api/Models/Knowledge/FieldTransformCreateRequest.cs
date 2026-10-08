using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a field transformation (<c>POST data/transforms/extractions</c>).</summary>
public sealed class FieldTransformCreateRequest : FieldTransformSettings
{
	/// <summary>The <c>transforms.conf</c> stanza name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
