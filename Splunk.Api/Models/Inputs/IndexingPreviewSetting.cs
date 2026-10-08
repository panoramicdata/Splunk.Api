using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>One <c>props.conf</c> setting of a data preview job.</summary>
public sealed class IndexingPreviewSetting
{
	/// <summary>The setting's value.</summary>
	[JsonPropertyName("value")]
	public string? Value { get; init; }

	/// <summary>The <c>props.conf</c> stanza it comes from; empty for an explicit setting.</summary>
	[JsonPropertyName("stanza")]
	public string? Stanza { get; init; }
}
