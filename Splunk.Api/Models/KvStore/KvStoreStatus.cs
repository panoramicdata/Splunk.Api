using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>The KV store's status on a standalone instance or search head cluster member (<c>kvstore/status</c>).</summary>
public sealed class KvStoreStatus : SplunkContent
{
	/// <summary>This server's KV store member.</summary>
	[JsonPropertyName("current")]
	public KvStoreCurrentStatus? Current { get; init; }

	/// <summary>The co-hosted KV store service, where Splunk runs one.</summary>
	[JsonPropertyName("cohosted")]
	public KvStoreCohostedStatus? Cohosted { get; init; }

	/// <summary>The search head cluster's KV store members, keyed by member; <see langword="null"/> on a standalone instance.</summary>
	[JsonPropertyName("members")]
	public IReadOnlyDictionary<string, JsonElement>? Members { get; init; }
}
