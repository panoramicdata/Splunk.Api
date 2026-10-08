using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>
/// A user-configurable object listed by the directory service (<c>directory</c>): a view, saved search, event type,
/// field extraction, lookup, tag and so on.
/// </summary>
/// <remarks>The entry's <c>id</c> and links point at the object's own endpoint.</remarks>
public sealed class DirectoryEntry : SplunkContent
{
	/// <summary>The object type, for example <c>views</c>, <c>savedsearch</c> or <c>transforms-extract</c> (<c>eai:type</c>).</summary>
	[JsonPropertyName("eai:type")]
	public string? Type { get; init; }

	/// <summary>The endpoint the object belongs to, for example <c>/data/ui/views</c> (<c>eai:location</c>).</summary>
	[JsonPropertyName("eai:location")]
	public string? Location { get; init; }

	/// <summary>Whether the object's owner no longer exists (<c>eai:orphaned</c>).</summary>
	[JsonPropertyName("eai:orphaned")]
	public bool? Orphaned { get; init; }
}
