using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Replaces a lookup table file with a file from Splunk's lookup staging area (<c>POST data/lookup-table-files/{name}</c>).</summary>
/// <remarks>See <see cref="LookupTableFileCreateRequest"/>: the file must already be on the Splunk server, below <c>$SPLUNK_HOME/var/run/splunk/lookup_tmp</c>.</remarks>
public sealed class LookupTableFileUpdateRequest : SplunkFormRequest
{
	/// <summary>The path of the staged replacement file on the Splunk server (<c>eai:data</c>).</summary>
	[JsonPropertyName("eai:data")]
	public required string StagedPath { get; init; }
}
