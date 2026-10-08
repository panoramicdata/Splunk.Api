namespace Splunk.Api.Models.KvStore;

/// <summary>
/// Changes a KV store collection (<c>POST storage/collections/config/{collection}</c>): adds or changes field types and
/// accelerations, or changes its settings.
/// </summary>
public sealed class KvStoreCollectionUpdateRequest : KvStoreCollectionSettings;
