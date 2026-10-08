using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Applications;

/// <summary>An app's setup information (<c>apps/local/{name}/setup</c>).</summary>
public sealed class AppSetup : SplunkContent
{
	/// <summary>The app's setup definition, as <c>SetupInfo</c> XML (<c>eai:setup</c>).</summary>
	[JsonPropertyName("eai:setup")]
	public string? Setup { get; init; }
}
