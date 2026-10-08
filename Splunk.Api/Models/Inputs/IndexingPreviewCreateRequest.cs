using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// Starts a data preview job for a file on the Splunk server (<c>POST indexing/preview</c>). Put <c>props.conf</c>
/// settings to try in <see cref="SplunkFormRequest.AdditionalParameters"/> as <c>props.{setting}</c>, for example
/// <c>props.SHOULD_LINEMERGE</c>.
/// </summary>
public sealed class IndexingPreviewCreateRequest : SplunkFormRequest
{
	/// <summary>The absolute path of the file to preview (<c>input.path</c>).</summary>
	[JsonPropertyName("input.path")]
	public required string InputPath { get; init; }
}
