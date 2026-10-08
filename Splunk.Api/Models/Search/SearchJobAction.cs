using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>A search job control action (<c>action</c> of <c>POST search/jobs/{search_id}/control</c>).</summary>
public enum SearchJobAction
{
	/// <summary>Pauses the job.</summary>
	[JsonStringEnumMemberName("pause")]
	Pause = 1,

	/// <summary>Resumes a paused job.</summary>
	[JsonStringEnumMemberName("unpause")]
	Unpause = 2,

	/// <summary>Stops the job, keeping the results so far.</summary>
	[JsonStringEnumMemberName("finalize")]
	Finalize = 3,

	/// <summary>Stops the job and deletes its results.</summary>
	[JsonStringEnumMemberName("cancel")]
	Cancel = 4,

	/// <summary>Resets the job's time to live, keeping it from expiring.</summary>
	[JsonStringEnumMemberName("touch")]
	Touch = 5,

	/// <summary>Sets the job's time to live; needs <see cref="SearchJobControlRequest.Ttl"/>.</summary>
	[JsonStringEnumMemberName("setttl")]
	SetTtl = 6,

	/// <summary>Sets the job's priority; needs <see cref="SearchJobControlRequest.Priority"/>.</summary>
	[JsonStringEnumMemberName("setpriority")]
	SetPriority = 7,

	/// <summary>Turns result previews on.</summary>
	[JsonStringEnumMemberName("enablepreview")]
	EnablePreview = 8,

	/// <summary>Turns result previews off.</summary>
	[JsonStringEnumMemberName("disablepreview")]
	DisablePreview = 9,

	/// <summary>Moves the job to another workload pool; needs <see cref="SearchJobControlRequest.WorkloadPool"/>.</summary>
	[JsonStringEnumMemberName("setworkloadpool")]
	SetWorkloadPool = 10
}
