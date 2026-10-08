using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>Changes a license pool (<c>POST licenser/pools/{name}</c>).</summary>
public sealed class LicensePoolUpdateRequest : SplunkFormRequest
{
	/// <summary>The pool's description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The pool's quota: <c>MAX</c>, a number of bytes, or a size such as <c>50MB</c>.</summary>
	[JsonPropertyName("quota")]
	public string? Quota { get; init; }

	/// <summary>The peers in the pool: GUIDs separated by commas, or <c>*</c> for every peer.</summary>
	[JsonPropertyName("peers")]
	public string? Peers { get; init; }

	/// <summary>Whether <see cref="Peers"/> are added to the pool's peers rather than replacing them.</summary>
	[JsonPropertyName("append_peers")]
	public bool? AppendPeers { get; init; }
}
