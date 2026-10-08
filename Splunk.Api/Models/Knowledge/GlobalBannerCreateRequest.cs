using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Writes the global banner (<c>POST data/ui/global-banner</c>).</summary>
/// <remarks>
/// The reference names the fields <c>message</c>, <c>visible</c>, <c>background_color</c>, <c>hyperlink</c> and
/// <c>hyperlink_text</c>; Splunk 10.6 takes them with a <c>global_banner.</c> prefix and also requires <c>name</c>, as its
/// own example and the endpoint's <c>_new</c> template show. Splunk Web uses the name <c>BANNER_MESSAGE_SINGLETON</c>.
/// </remarks>
public sealed class GlobalBannerCreateRequest : SplunkFormRequest
{
	/// <summary>The name Splunk Web reads the banner from.</summary>
	public const string SingletonName = "BANNER_MESSAGE_SINGLETON";

	/// <summary>The banner name; Splunk Web shows the banner named <see cref="SingletonName"/>.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The banner text (<c>global_banner.message</c>).</summary>
	[JsonPropertyName("global_banner.message")]
	public string? Message { get; init; }

	/// <summary>Whether the banner is shown (<c>global_banner.visible</c>).</summary>
	[JsonPropertyName("global_banner.visible")]
	public bool? Visible { get; init; }

	/// <summary>The banner colour: <c>green</c>, <c>blue</c>, <c>yellow</c>, <c>orange</c> or <c>red</c> (<c>global_banner.background_color</c>).</summary>
	[JsonPropertyName("global_banner.background_color")]
	public string? BackgroundColor { get; init; }

	/// <summary>A link to show in the banner, starting <c>http://</c> or <c>https://</c> (<c>global_banner.hyperlink</c>).</summary>
	[JsonPropertyName("global_banner.hyperlink")]
	public string? Hyperlink { get; init; }

	/// <summary>The text of the link (<c>global_banner.hyperlink_text</c>).</summary>
	[JsonPropertyName("global_banner.hyperlink_text")]
	public string? HyperlinkText { get; init; }
}
