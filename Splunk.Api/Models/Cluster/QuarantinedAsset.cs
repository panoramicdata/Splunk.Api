using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A lookup quarantined by search head cluster configuration replication (<c>replication/configuration/quarantined-assets</c>).</summary>
public sealed class QuarantinedAsset : SplunkContent
{
	/// <summary>The asset identifier.</summary>
	[JsonPropertyName("assetId")]
	public string? AssetId { get; init; }

	/// <summary>The asset path, for example <c>/nobody/search/lookups/test.csv</c>.</summary>
	[JsonPropertyName("assetURI")]
	public string? AssetUri { get; init; }

	/// <summary>The owner.</summary>
	[JsonPropertyName("user")]
	public string? User { get; init; }

	/// <summary>The app.</summary>
	[JsonPropertyName("app")]
	public string? App { get; init; }

	/// <summary>The asset type, for example <c>lookups</c>.</summary>
	[JsonPropertyName("assetType")]
	public string? AssetType { get; init; }

	/// <summary>The lookup name.</summary>
	[JsonPropertyName("assetName")]
	public string? AssetName { get; init; }

	/// <summary>Where, when, how large and why, as Splunk formats it (<c>quarantined_at_host</c>, <c>quarantined_at</c>, <c>lookup_size</c>, <c>quarantine_reason</c>).</summary>
	[JsonPropertyName("quarantineInfo")]
	public string? QuarantineInfo { get; init; }
}
