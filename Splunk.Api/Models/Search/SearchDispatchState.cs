using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>The state of a search job (<c>dispatchState</c>).</summary>
public enum SearchDispatchState
{
	/// <summary>A state this library does not know.</summary>
	Unknown = 0,

	/// <summary>Waiting to run.</summary>
	[JsonStringEnumMemberName("QUEUED")]
	Queued,

	/// <summary>Parsing the search.</summary>
	[JsonStringEnumMemberName("PARSING")]
	Parsing,

	/// <summary>Running.</summary>
	[JsonStringEnumMemberName("RUNNING")]
	Running,

	/// <summary>Finalizing (stopping early and keeping the results so far).</summary>
	[JsonStringEnumMemberName("FINALIZING")]
	Finalizing,

	/// <summary>Finished; results are complete.</summary>
	[JsonStringEnumMemberName("DONE")]
	Done,

	/// <summary>Paused.</summary>
	[JsonStringEnumMemberName("PAUSE")]
	Paused,

	/// <summary>Cancelled by Splunk.</summary>
	[JsonStringEnumMemberName("INTERNAL_CANCEL")]
	InternalCancel,

	/// <summary>Cancelled by a user.</summary>
	[JsonStringEnumMemberName("USER_CANCEL")]
	UserCancel,

	/// <summary>Cancelled because of bad input.</summary>
	[JsonStringEnumMemberName("BAD_INPUT_CANCEL")]
	BadInputCancel,

	/// <summary>The search process quit.</summary>
	[JsonStringEnumMemberName("QUIT")]
	Quit,

	/// <summary>Failed; see the job's messages.</summary>
	[JsonStringEnumMemberName("FAILED")]
	Failed
}
