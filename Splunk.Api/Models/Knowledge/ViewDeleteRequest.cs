using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Deletes a view with a change message (<c>DELETE data/ui/views/{name}</c>).</summary>
public sealed class ViewDeleteRequest : SplunkFormRequest
{
	/// <summary>A message recorded with the deletion (<c>eai:changelog</c>).</summary>
	[JsonPropertyName("eai:changelog")]
	public required string ChangeLog { get; init; }
}
