using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Adds and removes the <c>field::value</c> pairs a tag applies to (<c>POST search/tags/{tag_name}</c>).</summary>
/// <remarks>
/// Splunk applies the additions first, then the removals, and creates the tag if it does not exist (answering 201).
/// Each pair is written <c>{field}::{value}</c>, for example <c>host::web01</c> or <c>eventtype::failed_login</c>.
/// </remarks>
public sealed class TagUpdateRequest : SplunkFormRequest
{
	/// <summary>The <c>field::value</c> pairs to tag (<c>add</c>, repeated).</summary>
	[JsonPropertyName("add")]
	public IReadOnlyList<string>? Add { get; init; }

	/// <summary>The <c>field::value</c> pairs to untag (<c>delete</c>, repeated).</summary>
	[JsonPropertyName("delete")]
	public IReadOnlyList<string>? Delete { get; init; }
}
