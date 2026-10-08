using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>KV store status, backup, restore and maintenance (<c>kvstore</c>).</summary>
	public IKvStore KvStore => field ??= For<IKvStore>();

	/// <summary>KV store collection definitions (<c>storage/collections/config</c>); use in a <c>nobody</c> namespace.</summary>
	public IKvStoreCollections KvStoreCollections => field ??= For<IKvStoreCollections>();

	/// <summary>KV store documents (<c>storage/collections/data/{collection}</c>); use in a namespace.</summary>
	public IKvStoreData KvStoreData => field ??= For<IKvStoreData>();

	/// <summary>KV store collection statistics (<c>storage/collections/stats</c>); use in a namespace.</summary>
	public IKvStoreStatistics KvStoreStatistics => field ??= For<IKvStoreStatistics>();
}
