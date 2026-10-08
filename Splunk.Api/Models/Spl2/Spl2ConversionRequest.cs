using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>An SPL search to convert to SPL2 (<c>POST orchestrator/v1/spl2/convert</c> and <c>v2</c>), sent as JSON.</summary>
public sealed class Spl2ConversionRequest
{
	/// <summary>The SPL search (<c>spl1</c>).</summary>
	[JsonPropertyName("spl1")]
	public required string Spl { get; init; }

	/// <summary>The runtime the SPL2 is for (<c>runtime</c>); <c>splunkd</c> for Splunk Enterprise. Splunk requires it.</summary>
	[JsonPropertyName("runtime")]
	public string Runtime { get; init; } = "splunkd";
}
