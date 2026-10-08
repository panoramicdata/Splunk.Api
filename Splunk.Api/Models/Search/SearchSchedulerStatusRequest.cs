using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Enables or disables the search scheduler (<c>POST search/scheduler/status</c>).</summary>
public sealed class SearchSchedulerStatusRequest : SplunkFormRequest
{
	/// <summary><see langword="true"/> to stop running scheduled searches, <see langword="false"/> to resume (<c>disabled</c>).</summary>
	[JsonPropertyName("disabled")]
	public required bool Disabled { get; init; }
}
