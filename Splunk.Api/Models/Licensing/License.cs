using Splunk.Api.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>An installed license (<c>licenser/licenses</c>); the entry name is the license hash.</summary>
public sealed partial class License : SplunkContent
{
	/// <summary>A plain-text description of the license.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The license's unique identifier, used to address it.</summary>
	[JsonPropertyName("license_hash")]
	public string? LicenseHash { get; init; }

	/// <summary>The license's GUID.</summary>
	[JsonPropertyName("guid")]
	public string? LicenseGuid { get; init; }

	/// <summary>The license type, for example <c>enterprise</c>, <c>download-trial</c>, <c>forwarder</c> or <c>free</c>.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>The license group the license belongs to.</summary>
	[JsonPropertyName("group_id")]
	public string? GroupId { get; init; }

	/// <summary>The license subgroup, for example <c>Production</c>.</summary>
	[JsonPropertyName("subgroup_id")]
	public string? SubgroupId { get; init; }

	/// <summary>The license stack the license belongs to.</summary>
	[JsonPropertyName("stack_id")]
	public string? StackId { get; init; }

	/// <summary>The license status: <c>VALID</c> or <c>EXPIRED</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>The daily indexing quota, in bytes.</summary>
	[JsonPropertyName("quota")]
	public long Quota { get; init; }

	/// <summary>Whether the quota is unlimited.</summary>
	[JsonPropertyName("is_unlimited")]
	public bool IsUnlimited { get; init; }

	/// <summary>When the license was created.</summary>
	[JsonPropertyName("creation_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? CreationTime { get; init; }

	/// <summary>When the license expires.</summary>
	[JsonPropertyName("expiration_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? ExpirationTime { get; init; }

	/// <summary>The features the license enables.</summary>
	[JsonPropertyName("features")]
	public IReadOnlyList<string> Features { get; init; } = [];

	/// <summary>The features the license disables.</summary>
	[JsonPropertyName("disabled_features")]
	public IReadOnlyList<string> DisabledFeatures { get; init; } = [];

	/// <summary>The source types the license may index; empty for all.</summary>
	[JsonPropertyName("sourcetypes")]
	public IReadOnlyList<string> SourceTypes { get; init; } = [];

	/// <summary>The add-ons the license covers, keyed by add-on name, with their parameters; <see langword="null"/> if none.</summary>
	[JsonPropertyName("add_ons")]
	public IReadOnlyDictionary<string, JsonElement>? AddOns { get; init; }
}
