using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>A license stack (<c>licenser/stacks</c>); the entry name is the stack ID.</summary>
public sealed class LicenseStackInfo : SplunkContent
{
	/// <summary>The stack's name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The stack's license type, for example <c>enterprise</c> or <c>free</c>.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>The stack's daily quota in bytes: the sum of its licenses' quotas.</summary>
	[JsonPropertyName("quota")]
	public long Quota { get; init; }

	/// <summary>Whether the quota is unlimited.</summary>
	[JsonPropertyName("is_unlimited")]
	public bool IsUnlimited { get; init; }

	/// <summary>The number of violations allowed within <see cref="WindowPeriod"/>.</summary>
	[JsonPropertyName("max_violations")]
	public int MaxViolations { get; init; }

	/// <summary>The rolling period, in days, over which violations are counted.</summary>
	[JsonPropertyName("window_period")]
	public int WindowPeriod { get; init; }

	/// <summary>The maximum retention size; 0 for no limit.</summary>
	[JsonPropertyName("max_retention_size")]
	public long MaxRetentionSize { get; init; }

	/// <summary>Whether conditional license enforcement is active (<c>cle_active</c>, 0 or 1).</summary>
	[JsonPropertyName("cle_active")]
	public bool ConditionalEnforcementActive { get; init; }
}
