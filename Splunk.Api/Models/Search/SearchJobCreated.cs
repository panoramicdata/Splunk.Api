using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>The reply to creating or dispatching a search job: <c>{"sid":"..."}</c>, not the feed envelope.</summary>
public sealed class SearchJobCreated
{
	/// <summary>The new job's search ID.</summary>
	[JsonPropertyName("sid")]
	public string Sid { get; init; } = string.Empty;
}
