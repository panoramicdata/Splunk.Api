using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>
/// Today's license usage (<c>licenser/usage</c>). The deprecated <c>slaves_usage_bytes</c> duplicate is in
/// <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class LicenseUsage : SplunkContent
{
	/// <summary>The daily quota of the active license group, in bytes.</summary>
	[JsonPropertyName("quota")]
	public long Quota { get; init; }

	/// <summary>The bytes indexed today by all peers.</summary>
	[JsonPropertyName("peers_usage_bytes")]
	public long PeersUsageBytes { get; init; }
}
