using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Adds and removes tags on one value of a field (<c>POST search/fields/{field_name}/tags</c>).</summary>
/// <remarks>Splunk applies the additions first, then the removals. Set at least one tag to add or remove.</remarks>
public sealed class FieldTagsUpdateRequest : SplunkFormRequest
{
	/// <summary>The field value the tags apply to.</summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }

	/// <summary>The tags to add to the field value (<c>add</c>, repeated).</summary>
	[JsonPropertyName("add")]
	public IReadOnlyList<string>? Add { get; init; }

	/// <summary>The tags to remove from the field value (<c>delete</c>, repeated).</summary>
	[JsonPropertyName("delete")]
	public IReadOnlyList<string>? Delete { get; init; }
}
