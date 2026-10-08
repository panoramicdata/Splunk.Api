using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>Restores the KV store from a backup archive (<c>POST kvstore/backup/restore</c>).</summary>
public sealed class KvStoreRestoreRequest : KvStoreBackupRequest
{
	/// <summary>Insertion workers per collection for a point-in-time restore (<c>insertionsWorkersPerCollection</c>); default 1.</summary>
	[JsonPropertyName("insertionsWorkersPerCollection")]
	public int? InsertionsWorkersPerCollection { get; init; }
}
