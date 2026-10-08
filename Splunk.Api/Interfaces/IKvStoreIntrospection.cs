using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>KV store introspection (<c>server/introspection/kvstore</c>).</summary>
public interface IKvStoreIntrospection
{
	/// <summary>Lists the KV store introspection resources (<c>GET server/introspection/kvstore</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the entries <c>collectionstats</c>, <c>replicasetstats</c> and <c>serverstatus</c>.</returns>
	[Get("services/server/introspection/kvstore")]
	Task<SplunkFeed<SplunkDynamicContent>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Gets storage statistics for every collection (<c>GET server/introspection/kvstore/collectionstats</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>collectionStats</c>.</returns>
	[Get("services/server/introspection/kvstore/collectionstats")]
	Task<SplunkFeed<KvStoreCollectionStats>> GetCollectionStatsAsync(CancellationToken cancellationToken);

	/// <summary>Gets the replica set status as this server sees it (<c>GET server/introspection/kvstore/replicasetstats</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	/// <remarks>
	/// Splunk 10.6 with the co-hosted (non-MongoDB) KV store answers <c>503</c> "KV Store backed by Mongo is not available.
	/// Unable to list replica set."
	/// </remarks>
	[Get("services/server/introspection/kvstore/replicasetstats")]
	Task<SplunkFeed<KvStoreIntrospectionData>> GetReplicaSetStatsAsync(CancellationToken cancellationToken);

	/// <summary>Gets the state of the KV store database process (<c>GET server/introspection/kvstore/serverstatus</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>serverStatus</c>.</returns>
	[Get("services/server/introspection/kvstore/serverstatus")]
	Task<SplunkFeed<KvStoreIntrospectionData>> GetServerStatusAsync(CancellationToken cancellationToken);
}
