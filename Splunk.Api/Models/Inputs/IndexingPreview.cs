using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// A data preview job (<c>indexing/preview</c>). The entry name is the job ID, which is also a search job ID: read the
/// previewed events from <c>search/v2/jobs/{id}/results_preview</c>. A list shows each job without its settings.
/// </summary>
public sealed class IndexingPreview : SplunkContent
{
	/// <summary>The <c>props.conf</c> settings given for this preview, keyed by setting name.</summary>
	[JsonPropertyName("explicit")]
	public IReadOnlyDictionary<string, IndexingPreviewSetting> Explicit { get; init; } = new Dictionary<string, IndexingPreviewSetting>();

	/// <summary>The <c>props.conf</c> settings inherited from existing stanzas, keyed by setting name.</summary>
	[JsonPropertyName("inherited")]
	public IReadOnlyDictionary<string, IndexingPreviewSetting> Inherited { get; init; } = new Dictionary<string, IndexingPreviewSetting>();
}
