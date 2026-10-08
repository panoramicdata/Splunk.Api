using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// How creating a job waits (<c>exec_mode</c>). A oneshot search, which returns its results instead of a job, is
/// <see cref="Interfaces.ISearchJobs.RunOneshotAsync"/>.
/// </summary>
public enum SearchExecutionMode
{
	/// <summary>Returns the search ID at once; the job runs in the background.</summary>
	[JsonStringEnumMemberName("normal")]
	Normal = 1,

	/// <summary>Returns the search ID once the job has finished.</summary>
	[JsonStringEnumMemberName("blocking")]
	Blocking = 2
}
