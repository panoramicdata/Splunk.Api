using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a prebuilt dashboard panel (<c>POST data/ui/panels</c>).</summary>
public sealed class PanelCreateRequest : SplunkFormRequest
{
	/// <summary>The panel name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The panel's Simple XML source, for example <c>&lt;panel&gt;&lt;label&gt;Errors&lt;/label&gt;&lt;/panel&gt;</c> (<c>eai:data</c>).</summary>
	[JsonPropertyName("eai:data")]
	public required string Data { get; init; }
}
