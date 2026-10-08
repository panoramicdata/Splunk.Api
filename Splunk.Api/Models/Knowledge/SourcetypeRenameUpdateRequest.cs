using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Changes the new name of a renamed sourcetype (<c>POST data/props/sourcetype-rename/{name}</c>).</summary>
public sealed class SourcetypeRenameUpdateRequest : SplunkFormRequest
{
	/// <summary>The new sourcetype name.</summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }
}
