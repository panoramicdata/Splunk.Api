using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>
/// The settings shared by creating and updating a KV store collection. <see cref="Fields"/> and
/// <see cref="AcceleratedFields"/> are sent as <c>field.&lt;name&gt;</c> and <c>accelerated_fields.&lt;name&gt;</c> form
/// fields through <see cref="SplunkFormRequest.AdditionalParameters"/>, so in an object initializer set them after (not
/// before) any <see cref="SplunkFormRequest.AdditionalParameters"/> assignment.
/// </summary>
public abstract class KvStoreCollectionSettings : SplunkFormRequest
{
	private const string FieldPrefix = "field.";
	private const string AcceleratedFieldPrefix = "accelerated_fields.";

	/// <summary>Whether field types are enforced on insert and update (<c>enforceTypes</c>).</summary>
	[JsonPropertyName("enforceTypes")]
	public bool? EnforceTypes { get; init; }

	/// <summary>Whether the collection is replicated to indexers, for lookups in distributed searches (<c>replicate</c>).</summary>
	[JsonPropertyName("replicate")]
	public bool? Replicate { get; init; }

	/// <summary>Whether slow operations are profiled (<c>profilingEnabled</c>).</summary>
	[JsonPropertyName("profilingEnabled")]
	public bool? ProfilingEnabled { get; init; }

	/// <summary>The profiling threshold in milliseconds (<c>profilingThresholdMs</c>); Splunk's default is 1000.</summary>
	[JsonPropertyName("profilingThresholdMs")]
	public int? ProfilingThresholdMs { get; init; }

	/// <summary>Field types to declare, keyed by field name (<c>field.&lt;name&gt;</c>).</summary>
	[JsonIgnore]
	public IReadOnlyDictionary<string, KvStoreFieldType> Fields
	{
		get => Read(FieldPrefix).ToDictionary(p => p.Key, p => WireNames.Parse<KvStoreFieldType>(p.Value), StringComparer.Ordinal);
		init => Write(FieldPrefix, value.Select(p => new KeyValuePair<string, string>(p.Key, WireNames.Of(p.Value))));
	}

	/// <summary>
	/// Accelerations (indexes) to declare, keyed by name, each a JSON index specification such as <c>{"name": 1}</c> or
	/// <c>{"a": 1, "b": -1}</c> (<c>accelerated_fields.&lt;name&gt;</c>).
	/// </summary>
	[JsonIgnore]
	public IReadOnlyDictionary<string, string> AcceleratedFields
	{
		get => Read(AcceleratedFieldPrefix).ToDictionary(p => p.Key, p => p.Value, StringComparer.Ordinal);
		init => Write(AcceleratedFieldPrefix, value);
	}

	private IEnumerable<KeyValuePair<string, string>> Read(string prefix)
		=> AdditionalParameters
			.Where(p => p.Key.StartsWith(prefix, StringComparison.Ordinal))
			.Select(p => new KeyValuePair<string, string>(p.Key[prefix.Length..], p.Value ?? string.Empty));

	private void Write(string prefix, IEnumerable<KeyValuePair<string, string>> values)
	{
		foreach (var (name, value) in values)
		{
			AdditionalParameters[prefix + name] = value;
		}
	}
}
