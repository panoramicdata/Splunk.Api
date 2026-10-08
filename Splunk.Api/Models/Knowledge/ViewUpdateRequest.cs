using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Replaces a view's source (<c>POST data/ui/views/{name}</c>).</summary>
public sealed class ViewUpdateRequest : SplunkFormRequest
{
	/// <summary>The new view source (<c>eai:data</c>).</summary>
	[JsonPropertyName("eai:data")]
	public required string Data { get; init; }

	/// <summary>A message recorded with the revision and returned by the view's history (<c>eai:changelog</c>).</summary>
	[JsonPropertyName("eai:changelog")]
	public string? ChangeLog { get; init; }
}
