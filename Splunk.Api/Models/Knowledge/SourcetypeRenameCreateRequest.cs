using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Renames a sourcetype at search time (<c>POST data/props/sourcetype-rename</c>).</summary>
public sealed class SourcetypeRenameCreateRequest : SplunkFormRequest
{
	/// <summary>The original sourcetype name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The new sourcetype name.</summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }
}
