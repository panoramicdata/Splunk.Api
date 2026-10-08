using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.KvStore;

namespace Splunk.Api.Interfaces;

/// <summary>KV store status, backup, restore and maintenance (<c>kvstore</c>). Splunk Enterprise only.</summary>
public interface IKvStore
{
	/// <summary>Gets the KV store's status on a standalone instance or search head cluster member (<c>GET kvstore/status</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>status</c>.</returns>
	[Get("services/kvstore/status")]
	Task<SplunkFeed<KvStoreStatus>> GetStatusAsync(CancellationToken cancellationToken);

	/// <summary>Creates a backup archive of the KV store, an app or one collection (<c>POST kvstore/backup/create</c>).</summary>
	/// <param name="request">The archive name and what to back up.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when Splunk accepts the request; the backup itself runs in the background.</returns>
	[Post("services/kvstore/backup/create")]
	Task CreateBackupAsync([Body] KvStoreBackupRequest request, CancellationToken cancellationToken);

	/// <summary>Restores the KV store, an app or one collection from a backup archive (<c>POST kvstore/backup/restore</c>).</summary>
	/// <param name="request">The archive name and what to restore.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when Splunk accepts the request.</returns>
	/// <remarks>Restoring replaces the current contents of what is restored.</remarks>
	[Post("services/kvstore/backup/restore")]
	Task RestoreBackupAsync([Body] KvStoreRestoreRequest request, CancellationToken cancellationToken);

	/// <summary>Enters or leaves KV store maintenance mode (<c>POST kvstore/control/maintenance</c>).</summary>
	/// <param name="request">Whether to enter or leave maintenance mode.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the mode has changed.</returns>
	[Post("services/kvstore/control/maintenance")]
	Task SetMaintenanceModeAsync([Body] KvStoreMaintenanceRequest request, CancellationToken cancellationToken);
}
