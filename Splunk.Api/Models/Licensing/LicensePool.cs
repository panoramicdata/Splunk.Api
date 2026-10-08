using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>
/// A license pool (<c>licenser/pools</c>). The deprecated <c>slaves</c> and <c>slaves_usage_bytes</c> duplicates are in
/// <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class LicensePool : SplunkContent
{
	/// <summary>The pool's description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The license stack the pool draws on.</summary>
	[JsonPropertyName("stack_id")]
	public string? StackId { get; init; }

	/// <summary>The pool's quota as configured: <c>MAX</c> (the stack's whole quota) or a number of bytes.</summary>
	[JsonPropertyName("quota")]
	public string? Quota { get; init; }

	/// <summary>The pool's quota in bytes.</summary>
	[JsonPropertyName("effective_quota")]
	public long EffectiveQuota { get; init; }

	/// <summary>Whether the pool's quota is unlimited.</summary>
	[JsonPropertyName("is_unlimited")]
	public bool IsUnlimited { get; init; }

	/// <summary>Today's usage of the pool, in bytes.</summary>
	[JsonPropertyName("used_bytes")]
	public long UsedBytes { get; init; }

	/// <summary>The estimated effective usage, in bytes.</summary>
	[JsonPropertyName("estimated_effective_used_bytes")]
	public long EstimatedEffectiveUsedBytes { get; init; }

	/// <summary>The GUIDs of the peers in the pool; <c>*</c> for every peer.</summary>
	[JsonPropertyName("peers")]
	public IReadOnlyList<string> Peers { get; init; } = [];

	/// <summary>Each peer's usage of the pool, in bytes, keyed by peer GUID; <see langword="null"/> when none is reported.</summary>
	[JsonPropertyName("peers_usage_bytes")]
	public IReadOnlyDictionary<string, long>? PeersUsageBytes { get; init; }
}
