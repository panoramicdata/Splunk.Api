using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>Creates a KV store backup archive (<c>POST kvstore/backup/create</c>).</summary>
public class KvStoreBackupRequest : SplunkFormRequest
{
	/// <summary>The archive file name, created under <c>$SPLUNK_HOME/var/lib/splunk/kvstorebackup</c> (<c>archiveName</c>).</summary>
	[JsonPropertyName("archiveName")]
	public required string ArchiveName { get; init; }

	/// <summary>Limits the operation to one app (<c>appName</c>). Not allowed with <see cref="PointInTime"/>.</summary>
	[JsonPropertyName("appName")]
	public string? AppName { get; init; }

	/// <summary>Limits the operation to one collection of <see cref="AppName"/> (<c>collectionName</c>). Not allowed with <see cref="PointInTime"/>.</summary>
	[JsonPropertyName("collectionName")]
	public string? CollectionName { get; init; }

	/// <summary>Takes a consistent point-in-time copy (<c>pointInTime</c>); single instances only.</summary>
	[JsonPropertyName("pointInTime")]
	public bool? PointInTime { get; init; }

	/// <summary>Cancels a point-in-time operation in progress (<c>cancel</c>).</summary>
	[JsonPropertyName("cancel")]
	public bool? Cancel { get; init; }

	/// <summary>How many collections a point-in-time operation processes at once (<c>parallelCollections</c>); default 1.</summary>
	[JsonPropertyName("parallelCollections")]
	public int? ParallelCollections { get; init; }
}
