using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>A node of the health report tree: a feature category, a feature or an indicator.</summary>
public sealed class HealthFeature
{
	/// <summary>The node's status.</summary>
	[JsonPropertyName("health")]
	public HealthColor Health { get; init; }

	/// <summary>Whether the feature's health reporting is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The child nodes, keyed by display name; empty for a leaf.</summary>
	[JsonPropertyName("features")]
	public IReadOnlyDictionary<string, HealthFeature> Features { get; init; } = new Dictionary<string, HealthFeature>();

	/// <summary>Why the node is not green, keyed by color then by reason number; <see langword="null"/> when green.</summary>
	[JsonPropertyName("reasons")]
	public JsonElement? Reasons { get; init; }

	/// <summary>Messages attached to the node, if any.</summary>
	[JsonPropertyName("messages")]
	public JsonElement? Messages { get; init; }
}
