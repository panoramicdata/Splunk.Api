using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

public sealed partial class Role
{
	/// <summary>The indexes searchable through imported roles.</summary>
	[JsonPropertyName("imported_srchIndexesAllowed")]
	public IReadOnlyList<string> ImportedSearchIndexesAllowed { get; init; } = [];

	/// <summary>The default indexes inherited from imported roles.</summary>
	[JsonPropertyName("imported_srchIndexesDefault")]
	public IReadOnlyList<string> ImportedSearchIndexesDefault { get; init; } = [];

	/// <summary>The disallowed indexes inherited from imported roles.</summary>
	[JsonPropertyName("imported_srchIndexesDisallowed")]
	public IReadOnlyList<string> ImportedSearchIndexesDisallowed { get; init; } = [];

	/// <summary>The search filter inherited from imported roles.</summary>
	[JsonPropertyName("imported_srchFilter")]
	public string? ImportedSearchFilter { get; init; }

	/// <summary>The historical search quota inherited from imported roles.</summary>
	[JsonPropertyName("imported_srchJobsQuota")]
	public int? ImportedSearchJobsQuota { get; init; }

	/// <summary>The real-time search quota inherited from imported roles.</summary>
	[JsonPropertyName("imported_rtSrchJobsQuota")]
	public int? ImportedRealTimeSearchJobsQuota { get; init; }

	/// <summary>The disk quota, in MB, inherited from imported roles.</summary>
	[JsonPropertyName("imported_srchDiskQuota")]
	public int? ImportedSearchDiskQuota { get; init; }

	/// <summary>The search time window, in seconds, inherited from imported roles.</summary>
	[JsonPropertyName("imported_srchTimeWin")]
	public long? ImportedSearchTimeWindow { get; init; }

	/// <summary>The earliest search time, in seconds, inherited from imported roles.</summary>
	[JsonPropertyName("imported_srchTimeEarliest")]
	public long? ImportedSearchTimeEarliest { get; init; }
}
