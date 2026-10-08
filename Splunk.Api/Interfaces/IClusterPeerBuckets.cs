using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The buckets of this peer node (<c>cluster/peer/buckets</c>).</summary>
/// <remarks>
/// Call these on an indexer cluster peer; other nodes answer HTTP 503 "Cluster peer is not enabled on this node, check
/// clustering stanza in server.conf".
/// </remarks>
public interface IClusterPeerBuckets
{
	/// <summary>Lists the peer's buckets (<c>GET cluster/peer/buckets</c>).</summary>
	/// <param name="options">A generation, paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per bucket.</returns>
	[Get("services/cluster/peer/buckets")]
	Task<SplunkFeed<ClusterPeerBucket>> ListAsync([Query] ClusterPeerBucketListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one of the peer's buckets (<c>GET cluster/peer/buckets/{name}</c>).</summary>
	/// <param name="name">The bucket ID.</param>
	/// <param name="generationId">The generation to describe the bucket in (<c>generation_id</c>), or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the bucket.</returns>
	[Get("services/cluster/peer/buckets/{name}")]
	Task<SplunkFeed<ClusterPeerBucket>> GetAsync(string name, [AliasAs("generation_id")] string? generationId, CancellationToken cancellationToken);

	/// <summary>Removes a bucket from the peer (<c>DELETE cluster/peer/buckets/{name}</c>).</summary>
	/// <remarks>The bucket ID is sent both in the path and, as the reference requires, as a <c>bucket_id</c> form field.</remarks>
	/// <param name="name">The bucket ID.</param>
	/// <param name="request">The bucket ID again.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the bucket is removed.</returns>
	[Delete("services/cluster/peer/buckets/{name}")]
	Task DeleteAsync(string name, [Body] ClusterPeerBucketRemoveRequest request, CancellationToken cancellationToken);
}
