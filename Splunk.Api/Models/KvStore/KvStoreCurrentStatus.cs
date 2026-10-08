using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>The status of this server's KV store member (<c>kvstore/status</c>, <c>current</c>).</summary>
public sealed class KvStoreCurrentStatus
{
	/// <summary>The status, for example <c>ready</c>, <c>starting</c> or <c>failed</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>Whether the KV store is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>Whether this is a standalone (not clustered) KV store.</summary>
	[JsonPropertyName("standalone")]
	public bool? Standalone { get; init; }

	/// <summary>The member's identifier.</summary>
	[JsonPropertyName("guid")]
	public string? MemberGuid { get; init; }

	/// <summary>The KV store port.</summary>
	[JsonPropertyName("port")]
	public int? Port { get; init; }

	/// <summary>The storage engine, for example <c>wiredTiger</c>.</summary>
	[JsonPropertyName("storageEngine")]
	public string? StorageEngine { get; init; }

	/// <summary>The backup and restore status, for example <c>Ready</c>.</summary>
	[JsonPropertyName("backupRestoreStatus")]
	public string? BackupRestoreStatus { get; init; }

	/// <summary>The storage-engine migration status, for example <c>NotStarted</c>.</summary>
	[JsonPropertyName("migrationStatus")]
	public string? MigrationStatus { get; init; }

	/// <summary>The replication status in a search head cluster, for example <c>KV store captain</c>.</summary>
	[JsonPropertyName("replicationStatus")]
	public string? ReplicationStatus { get; init; }

	/// <summary>The replica set name in a search head cluster.</summary>
	[JsonPropertyName("replicaSet")]
	public string? ReplicaSet { get; init; }

	/// <summary>Whether a version upgrade is in progress.</summary>
	[JsonPropertyName("versionUpgradeInProgress")]
	public bool? VersionUpgradeInProgress { get; init; }

	/// <summary>Every property Splunk returned that is not modelled, such as oplog timestamps.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
