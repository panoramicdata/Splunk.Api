using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a view, usually a dashboard (<c>POST data/ui/views</c>).</summary>
public sealed class ViewCreateRequest : SplunkFormRequest
{
	/// <summary>The view name, used in its URL.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The view source, for example <c>&lt;dashboard version="1.1"&gt;&lt;label&gt;Errors&lt;/label&gt;&lt;/dashboard&gt;</c>
	/// (<c>eai:data</c>).
	/// </summary>
	[JsonPropertyName("eai:data")]
	public required string Data { get; init; }
}
