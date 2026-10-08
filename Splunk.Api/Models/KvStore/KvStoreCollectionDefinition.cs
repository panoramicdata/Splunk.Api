using Splunk.Api.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>A KV store collection definition (<c>storage/collections/config</c>).</summary>
public sealed class KvStoreCollectionDefinition : SplunkContent
{
	private const string FieldPrefix = "field.";
	private const string AcceleratedFieldPrefix = "accelerated_fields.";

	/// <summary>Whether the collection is replicated to indexers (<c>replicate</c>).</summary>
	[JsonPropertyName("replicate")]
	public bool? Replicate { get; init; }

	/// <summary>How replicated data is dumped, for example <c>auto</c> (<c>replication_dump_strategy</c>).</summary>
	[JsonPropertyName("replication_dump_strategy")]
	public string? ReplicationDumpStrategy { get; init; }

	/// <summary>The maximum replication dump file size in kilobytes (<c>replication_dump_maximum_file_size</c>).</summary>
	[JsonPropertyName("replication_dump_maximum_file_size")]
	public long? ReplicationDumpMaximumFileSize { get; init; }

	/// <summary>Whether field types are enforced on insert and update (<c>enforceTypes</c>).</summary>
	[JsonPropertyName("enforceTypes")]
	public bool? EnforceTypes { get; init; }

	/// <summary>Whether slow operations are profiled (<c>profilingEnabled</c>).</summary>
	[JsonPropertyName("profilingEnabled")]
	public bool? ProfilingEnabled { get; init; }

	/// <summary>The profiling threshold in milliseconds (<c>profilingThresholdMs</c>).</summary>
	[JsonPropertyName("profilingThresholdMs")]
	public int? ProfilingThresholdMs { get; init; }

	/// <summary>The collection type, usually <c>undefined</c> (<c>type</c>).</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>The declared field types, keyed by field name (from the <c>field.&lt;name&gt;</c> keys).</summary>
	[JsonIgnore]
	public IReadOnlyDictionary<string, KvStoreFieldType> Fields
		=> Prefixed(FieldPrefix).ToDictionary(p => p.Key, p => WireNames.Parse<KvStoreFieldType>(p.Value), StringComparer.Ordinal);

	/// <summary>
	/// The accelerations (indexes), keyed by name, each a JSON index specification such as <c>{"name": 1}</c>
	/// (from the <c>accelerated_fields.&lt;name&gt;</c> keys).
	/// </summary>
	[JsonIgnore]
	public IReadOnlyDictionary<string, string> AcceleratedFields
		=> Prefixed(AcceleratedFieldPrefix).ToDictionary(p => p.Key, p => p.Value, StringComparer.Ordinal);

	private IEnumerable<KeyValuePair<string, string>> Prefixed(string prefix)
		=> AdditionalProperties
			.Where(p => p.Key.StartsWith(prefix, StringComparison.Ordinal) && p.Value.ValueKind == JsonValueKind.String)
			.Select(p => new KeyValuePair<string, string>(p.Key[prefix.Length..], p.Value.GetString()!));
}
