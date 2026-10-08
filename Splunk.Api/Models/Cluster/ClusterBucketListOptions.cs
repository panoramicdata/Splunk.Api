using Refit;

namespace Splunk.Api.Models.Cluster;

/// <summary>The parameters of <c>GET cluster/manager/buckets</c>.</summary>
public sealed class ClusterBucketListOptions : ListOptions
{
	/// <summary>Bucket filters, each <c>name=value</c> (or <c>name&gt;value</c> etc. for numbers), sent as repeated <c>filter</c> parameters. Filter names: <c>index</c>, <c>status</c>, <c>search_state</c>, <c>replication_count</c>, <c>search_count</c>, <c>bucket_size</c>, <c>frozen</c>, <c>has_primary</c>, <c>meets_multisite_replication_count</c>, <c>meets_multisite_search_count</c>, <c>multisite_bucket</c>, <c>origin_site</c>, <c>standalone</c>.</summary>
	[AliasAs("filter")]
	[Query(CollectionFormat.Multi)]
	public IEnumerable<string>? Filters { get; init; }

	/// <summary>Whether to include summaries.</summary>
	[AliasAs("summaries")]
	public bool? Summaries { get; init; }
}
