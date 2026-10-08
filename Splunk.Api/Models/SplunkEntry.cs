using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>One entry in a <see cref="SplunkFeed{T}"/>: a named object, its access control and its content.</summary>
/// <typeparam name="T">The type of <see cref="Content"/>.</typeparam>
public sealed class SplunkEntry<T>
{
	/// <summary>The object's name (for example a saved search, index or app name).</summary>
	[JsonPropertyName("name")]
	public string Name { get; init; } = string.Empty;

	/// <summary>The object's full URL, which identifies it.</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>When the object was last updated.</summary>
	[JsonPropertyName("updated")]
	public DateTimeOffset? Updated { get; init; }

	/// <summary>The user that owns the object, or <c>nobody</c>.</summary>
	[JsonPropertyName("author")]
	public string? Author { get; init; }

	/// <summary>Links to operations on this entry, keyed by relation (for example <c>edit</c>, <c>remove</c>, <c>list</c>).</summary>
	[JsonPropertyName("links")]
	public IReadOnlyDictionary<string, string> Links { get; init; } = new Dictionary<string, string>();

	/// <summary>The object's access control: owner, app, sharing and permissions.</summary>
	[JsonPropertyName("acl")]
	public SplunkAcl? Acl { get; init; }

	/// <summary>The object's properties.</summary>
	[JsonPropertyName("content")]
	public T? Content { get; init; }
}
