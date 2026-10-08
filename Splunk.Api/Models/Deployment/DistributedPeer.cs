using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A search peer of this search head (<c>search/distributed/peers</c>).</summary>
/// <remarks><c>status_details</c> and any other unmodelled keys are in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class DistributedPeer : SplunkContent
{
	/// <summary>The Splunk server name of the peer.</summary>
	[JsonPropertyName("peerName")]
	public string? PeerName { get; init; }

	/// <summary>Whether the peer is <c>configured</c> or <c>discovered</c>.</summary>
	[JsonPropertyName("peerType")]
	public string? PeerType { get; init; }

	/// <summary>The peer status, for example <c>Up</c>, <c>Down</c> or <c>Authentication Failed</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>The bundle replication status: <c>Initial</c>, <c>In progress</c>, <c>Failed</c>, <c>Successful</c> or <c>Mounted</c>.</summary>
	[JsonPropertyName("replicationStatus")]
	public string? ReplicationStatus { get; init; }

	/// <summary>Whether the peer's management port uses TLS.</summary>
	[JsonPropertyName("is_https")]
	public bool? IsHttps { get; init; }

	/// <summary>The peer's GUID.</summary>
	[JsonPropertyName("guid")]
	public string? PeerGuid { get; init; }

	/// <summary>The peer's Splunk build.</summary>
	[JsonPropertyName("build")]
	public string? Build { get; init; }

	/// <summary>The peer's Splunk version.</summary>
	[JsonPropertyName("version")]
	public string? Version { get; init; }

	/// <summary>The peer's license signature.</summary>
	[JsonPropertyName("licenseSignature")]
	public string? LicenseSignature { get; init; }

	/// <summary>The bundles of this search head the peer has, newest first.</summary>
	[JsonPropertyName("bundle_versions")]
	public IReadOnlyList<string> BundleVersions { get; init; } = [];
}
