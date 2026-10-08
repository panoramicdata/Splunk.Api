using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// The settings of a role, shared by <see cref="RoleCreateRequest"/> and <see cref="RoleUpdateRequest"/>. Each list is
/// sent as one field per value and replaces the role's whole list. Set others (such as the KV store deny lists) in
/// <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public abstract class RoleSettings : SplunkFormRequest
{
	/// <summary>The capabilities to grant directly.</summary>
	[JsonPropertyName("capabilities")]
	public IReadOnlyList<string>? Capabilities { get; init; }

	/// <summary>The roles to import: their capabilities, indexes and other settings are inherited.</summary>
	[JsonPropertyName("imported_roles")]
	public IReadOnlyList<string>? ImportedRoles { get; init; }

	/// <summary>The roles a member of this role may grant to users.</summary>
	[JsonPropertyName("grantable_roles")]
	public IReadOnlyList<string>? GrantableRoles { get; init; }

	/// <summary>The default app (its folder name) of the role's members.</summary>
	[JsonPropertyName("defaultApp")]
	public string? DefaultApp { get; init; }

	/// <summary>The indexes the role may search; wildcards are allowed.</summary>
	[JsonPropertyName("srchIndexesAllowed")]
	public IReadOnlyList<string>? SearchIndexesAllowed { get; init; }

	/// <summary>The indexes searched when a search names none.</summary>
	[JsonPropertyName("srchIndexesDefault")]
	public IReadOnlyList<string>? SearchIndexesDefault { get; init; }

	/// <summary>The indexes the role may not search or delete from.</summary>
	[JsonPropertyName("srchIndexesDisallowed")]
	public IReadOnlyList<string>? SearchIndexesDisallowed { get; init; }

	/// <summary>The indexes the role may delete events from.</summary>
	[JsonPropertyName("deleteIndexesAllowed")]
	public IReadOnlyList<string>? DeleteIndexesAllowed { get; init; }

	/// <summary>A search that restricts what the role's searches return.</summary>
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

	/// <summary>The maximum time span of a search, in seconds; 0 for no limit.</summary>
	[JsonPropertyName("srchTimeWin")]
	public long? SearchTimeWindow { get; init; }

	/// <summary>How far back, in seconds, a search may reach.</summary>
	[JsonPropertyName("srchTimeEarliest")]
	public long? SearchTimeEarliest { get; init; }

	/// <summary>The transparent-mode federated providers the role may search, separated by semicolons.</summary>
	[JsonPropertyName("srchFederatedProvidersAllowed")]
	public string? SearchFederatedProvidersAllowed { get; init; }

	/// <summary>The transparent-mode federated providers searched by default, separated by semicolons.</summary>
	[JsonPropertyName("srchFederatedProvidersDefault")]
	public string? SearchFederatedProvidersDefault { get; init; }

	/// <summary>The field filters the role is exempt from.</summary>
	[JsonPropertyName("fieldFilterExemption")]
	public IReadOnlyList<string>? FieldFilterExemption { get; init; }
}
