using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A prebuilt dashboard panel (<c>data/ui/panels</c>).</summary>
public sealed class Panel : SplunkContent
{
	/// <summary>The panel's Simple XML source (<c>eai:data</c>).</summary>
	[JsonPropertyName("eai:data")]
	public string? Data { get; init; }

	/// <summary>A hash of the current definition (<c>eai:digest</c>).</summary>
	[JsonPropertyName("eai:digest")]
	public string? Digest { get; init; }

	/// <summary>The panel label.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The panel title (<c>panel.title</c>).</summary>
	[JsonPropertyName("panel.title")]
	public string? Title { get; init; }

	/// <summary>The root XML element, normally <c>panel</c>.</summary>
	[JsonPropertyName("rootNode")]
	public string? RootNode { get; init; }
}
