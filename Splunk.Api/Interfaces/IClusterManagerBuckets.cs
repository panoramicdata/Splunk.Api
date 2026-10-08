using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>Clustered buckets, on the cluster manager (<c>cluster/manager/buckets</c>).</summary>
/// <remarks>
/// Call these on the cluster manager; other nodes answer HTTP 503 "Cluster manager is not enabled on this node". Bucket
/// IDs have the form <c>index~number~originGuid</c>.
/// </remarks>
public interface IClusterManagerBuckets
{
	/// <summary>Lists the clustered buckets (<c>GET cluster/manager/buckets</c>).</summary>
	/// <param name="options">Filters, paging and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per bucket.</returns>
	[Get("services/cluster/manager/buckets")]
	Task<SplunkFeed<ClusterBucket>> ListAsync([Query] ClusterBucketListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a clustered bucket (<c>GET cluster/manager/buckets/{name}</c>).</summary>
	/// <param name="name">The bucket ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the bucket.</returns>
	[Get("services/cluster/manager/buckets/{name}")]
	Task<SplunkFeed<ClusterBucket>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Adds a bucket to the fix list (<c>POST cluster/manager/buckets/{bucket_id}/fix</c>).</summary>
	/// <remarks>Needs the admin role or <c>indexes_edit</c>.</remarks>
	/// <param name="bucketId">The bucket ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/buckets/{bucketId}/fix")]
	Task<SplunkFeed<SplunkDynamicContent>> FixAsync(string bucketId, CancellationToken cancellationToken);

	/// <summary>
	/// Starts a corruption fixup of a clustered bucket that is not SmartStore-enabled
	/// (<c>POST cluster/manager/buckets/{bucket_id}/fix_corrupt_bucket</c>).
	/// </summary>
	/// <remarks>Needs the admin role or <c>edit_indexer_cluster</c>.</remarks>
	/// <param name="bucketId">The bucket ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/buckets/{bucketId}/fix_corrupt_bucket")]
	Task<SplunkFeed<SplunkDynamicContent>> FixCorruptAsync(string bucketId, CancellationToken cancellationToken);

	/// <summary>Marks a bucket frozen on the manager (<c>POST cluster/manager/buckets/{bucket_id}/freeze</c>).</summary>
	/// <remarks>
	/// The peers' copies are not frozen, and the state may not survive a manager restart. Needs the admin role or
	/// <c>indexes_edit</c>.
	/// </remarks>
	/// <param name="bucketId">The bucket ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/buckets/{bucketId}/freeze")]
	Task<SplunkFeed<SplunkDynamicContent>> FreezeAsync(string bucketId, CancellationToken cancellationToken);

	/// <summary>Deletes every copy of a bucket (<c>POST cluster/manager/buckets/{bucket_id}/remove_all</c>).</summary>
	/// <remarks>
	/// Irreversible data loss. Hot buckets cannot be removed ("cannot remove hot bucket from cluster"). Needs the admin role
	/// or <c>indexes_edit</c>.
	/// </remarks>
	/// <param name="bucketId">The bucket ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/buckets/{bucketId}/remove_all")]
	Task<SplunkFeed<SplunkDynamicContent>> RemoveAllAsync(string bucketId, CancellationToken cancellationToken);

	/// <summary>Deletes a bucket's copy from one peer (<c>POST cluster/manager/buckets/{bucket_id}/remove_from_peer</c>).</summary>
	/// <remarks>
	/// Irreversible data loss; if the cluster loses its complete state it starts fixup, which may copy the bucket back.
	/// </remarks>
	/// <param name="bucketId">The bucket ID.</param>
	/// <param name="request">The peer.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/buckets/{bucketId}/remove_from_peer")]
	Task<SplunkFeed<SplunkDynamicContent>> RemoveFromPeerAsync(string bucketId, [Body] ClusterBucketRemoveFromPeerRequest request, CancellationToken cancellationToken);
}
