using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The health of splunkd or of a distributed deployment (<c>server/health/...</c>).</summary>
public sealed class HealthReport : SplunkContent
{
	/// <summary>The overall status.</summary>
	[JsonPropertyName("health")]
	public HealthColor Health { get; init; }

	/// <summary>The feature tree, keyed by display name; only the <c>details</c> endpoints fill it.</summary>
	[JsonPropertyName("features")]
	public IReadOnlyDictionary<string, HealthFeature> Features { get; init; } = new Dictionary<string, HealthFeature>();
}
