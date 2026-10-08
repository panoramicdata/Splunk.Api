using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>The global banner shown across Splunk Web (<c>data/ui/global-banner</c>, entry <c>BANNER_MESSAGE_SINGLETON</c>).</summary>
public sealed class GlobalBanner : SplunkContent
{
	/// <summary>The banner colour: <c>green</c>, <c>blue</c>, <c>yellow</c>, <c>orange</c> or <c>red</c> (<c>global_banner.background_color</c>).</summary>
	[JsonPropertyName("global_banner.background_color")]
	public string? BackgroundColor { get; init; }

	/// <summary>A link shown in the banner (<c>global_banner.hyperlink</c>).</summary>
	[JsonPropertyName("global_banner.hyperlink")]
	public string? Hyperlink { get; init; }

	/// <summary>The text of the link (<c>global_banner.hyperlink_text</c>).</summary>
	[JsonPropertyName("global_banner.hyperlink_text")]
	public string? HyperlinkText { get; init; }

	/// <summary>The banner text (<c>global_banner.message</c>).</summary>
	[JsonPropertyName("global_banner.message")]
	public string? Message { get; init; }

	/// <summary>Whether the banner is shown (<c>global_banner.visible</c>).</summary>
	[JsonPropertyName("global_banner.visible")]
	public bool? Visible { get; init; }
}
