using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// A role (<c>authorization/roles</c>); the entry name is the role name. The <c>imported_*</c> properties are what the
/// role inherits from <see cref="ImportedRoles"/>. The KV store deny lists and other keys are in
/// <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed partial class Role : SplunkContent
{
	/// <summary>
	/// The capabilities granted directly to the role. Splunk leaves out any the role already has through
	/// <see cref="ImportedRoles"/> (those are in <see cref="ImportedCapabilities"/>), even when they were set explicitly.
	/// </summary>
	[JsonPropertyName("capabilities")]
	public IReadOnlyList<string> Capabilities { get; init; } = [];

	/// <summary>The capabilities inherited from imported roles.</summary>
	[JsonPropertyName("imported_capabilities")]
	public IReadOnlyList<string> ImportedCapabilities { get; init; } = [];

	/// <summary>The roles this role imports.</summary>
	[JsonPropertyName("imported_roles")]
	public IReadOnlyList<string> ImportedRoles { get; init; } = [];

	/// <summary>The roles a member of this role may grant to users.</summary>
	[JsonPropertyName("grantable_roles")]
	public IReadOnlyList<string> GrantableRoles { get; init; } = [];

	/// <summary>The default app of the role's members; a user's own default app overrides it.</summary>
	[JsonPropertyName("defaultApp")]
	public string? DefaultApp { get; init; }

	/// <summary>The indexes the role may search.</summary>
	[JsonPropertyName("srchIndexesAllowed")]
	public IReadOnlyList<string> SearchIndexesAllowed { get; init; } = [];

	/// <summary>The indexes searched when a search names none.</summary>
	[JsonPropertyName("srchIndexesDefault")]
	public IReadOnlyList<string> SearchIndexesDefault { get; init; } = [];

	/// <summary>The indexes the role may not search or delete from.</summary>
	[JsonPropertyName("srchIndexesDisallowed")]
	public IReadOnlyList<string> SearchIndexesDisallowed { get; init; } = [];

	/// <summary>The indexes the role may delete events from.</summary>
	[JsonPropertyName("deleteIndexesAllowed")]
	public IReadOnlyList<string> DeleteIndexesAllowed { get; init; } = [];

	/// <summary>The search that restricts what the role's searches return.</summary>
	[JsonPropertyName("srchFilter")]
	public string? SearchFilter { get; init; }

	/// <summary>The maximum number of concurrent historical searches per member.</summary>
	[JsonPropertyName("srchJobsQuota")]
	public int? SearchJobsQuota { get; init; }

	/// <summary>The maximum number of concurrent real-time searches per member.</summary>
	[JsonPropertyName("rtSrchJobsQuota")]
	public int? RealTimeSearchJobsQuota { get; init; }

	/// <summary>The maximum number of concurrent historical searches across all members.</summary>
	[JsonPropertyName("cumulativeSrchJobsQuota")]
	public int? CumulativeSearchJobsQuota { get; init; }

	/// <summary>The maximum number of concurrent real-time searches across all members.</summary>
	[JsonPropertyName("cumulativeRTSrchJobsQuota")]
	public int? CumulativeRealTimeSearchJobsQuota { get; init; }

	/// <summary>The maximum number of queued searches per member.</summary>
	[JsonPropertyName("queuedSearchQuota")]
	public int? QueuedSearchQuota { get; init; }

	/// <summary>The disk space, in MB, a member's search jobs may use.</summary>
	[JsonPropertyName("srchDiskQuota")]
	public int? SearchDiskQuota { get; init; }

	/// <summary>The maximum time span of a search, in seconds; 0 or -1 for no limit.</summary>
	[JsonPropertyName("srchTimeWin")]
	public long? SearchTimeWindow { get; init; }

	/// <summary>How far back, in seconds, a search may reach; 0 or -1 for no limit.</summary>
	[JsonPropertyName("srchTimeEarliest")]
	public long? SearchTimeEarliest { get; init; }

	/// <summary>The minimum interval, in seconds, between runs of the role's scheduled searches.</summary>
	[JsonPropertyName("srchMinScheduleInterval")]
	public int? SearchMinScheduleInterval { get; init; }

	/// <summary>Whether the role may schedule searches with advanced cron expressions.</summary>
	[JsonPropertyName("srchAllowAdvancedCron")]
	public bool? SearchAllowAdvancedCron { get; init; }

	/// <summary>The transparent-mode federated providers the role may search.</summary>
	[JsonPropertyName("srchFederatedProvidersAllowed")]
	public IReadOnlyList<string> SearchFederatedProvidersAllowed { get; init; } = [];

	/// <summary>The transparent-mode federated providers searched by default.</summary>
	[JsonPropertyName("srchFederatedProvidersDefault")]
	public IReadOnlyList<string> SearchFederatedProvidersDefault { get; init; } = [];

	/// <summary>The field filters the role is exempt from.</summary>
	[JsonPropertyName("fieldFilterExemption")]
	public IReadOnlyList<string> FieldFilterExemption { get; init; } = [];
}
