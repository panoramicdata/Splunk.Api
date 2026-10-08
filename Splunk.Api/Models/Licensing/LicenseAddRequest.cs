using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>Adds a license (<c>POST licenser/licenses</c>).</summary>
public sealed class LicenseAddRequest : SplunkFormRequest
{
	/// <summary>The path of the license file on the Splunk server. Ignored when <see cref="Payload"/> is set.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The license itself, as XML.</summary>
	[JsonPropertyName("payload")]
	public string? Payload { get; init; }
}
