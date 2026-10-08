using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a lookup table file from a file in Splunk's lookup staging area (<c>POST data/lookup-table-files</c>).</summary>
/// <remarks>
/// Splunk does not take the file's content here: it moves a file that is already on the Splunk server, below the lookup
/// staging area <c>$SPLUNK_HOME/var/run/splunk/lookup_tmp</c>, into the app's <c>lookups</c> directory. Any other path is
/// refused with "Source file is outside of staging area".
/// </remarks>
public sealed class LookupTableFileCreateRequest : SplunkFormRequest
{
	/// <summary>The lookup table file name, for example <c>assets.csv</c>.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The path of the staged file on the Splunk server, below <c>$SPLUNK_HOME/var/run/splunk/lookup_tmp</c>
	/// (<c>eai:data</c>), for example <c>/opt/splunk/var/run/splunk/lookup_tmp/assets.csv</c>.
	/// </summary>
	[JsonPropertyName("eai:data")]
	public required string StagedPath { get; init; }
}
