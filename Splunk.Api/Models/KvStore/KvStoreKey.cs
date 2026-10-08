using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>The key of a KV store document that was inserted or updated (<c>{"_key": "..."}</c>).</summary>
public sealed class KvStoreKey
{
	/// <summary>The document's key (<c>_key</c>).</summary>
	[JsonPropertyName("_key")]
	public string Key { get; init; } = string.Empty;
}
