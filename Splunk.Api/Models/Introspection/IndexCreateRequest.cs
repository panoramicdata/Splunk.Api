using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>Creates an index (<c>POST data/indexes</c>).</summary>
/// <remarks>
/// The reference marks <see cref="HomePath"/>, <see cref="ColdPath"/> and <see cref="ThawedPath"/> as required; Splunk 10.6
/// defaults them to <c>$SPLUNK_DB/&lt;name&gt;/db</c>, <c>colddb</c> and <c>thaweddb</c> when they are left out.
/// </remarks>
public sealed class IndexCreateRequest : IndexSettings
{
	/// <summary>The index name: lowercase letters, digits, <c>_</c> and <c>-</c>, not starting with <c>_</c> or <c>-</c> (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The kind of data the index holds (<c>datatype</c>); Splunk's default is events.</summary>
	[JsonPropertyName("datatype")]
	public IndexDataType? DataType { get; init; }

	/// <summary>The hot and warm bucket path (<c>homePath</c>).</summary>
	[JsonPropertyName("homePath")]
	public string? HomePath { get; init; }

	/// <summary>The cold bucket path (<c>coldPath</c>).</summary>
	[JsonPropertyName("coldPath")]
	public string? ColdPath { get; init; }

	/// <summary>The thawed bucket path (<c>thawedPath</c>).</summary>
	[JsonPropertyName("thawedPath")]
	public string? ThawedPath { get; init; }
}
