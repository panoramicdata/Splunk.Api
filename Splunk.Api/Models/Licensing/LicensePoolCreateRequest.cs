using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>Creates a license pool (<c>POST licenser/pools</c>).</summary>
public sealed class LicensePoolCreateRequest : SplunkFormRequest
{
	/// <summary>The pool name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The pool's quota: <c>MAX</c> (the stack's whole quota; one such pool per stack), a number of bytes, or a size such
	/// as <c>50MB</c> or <c>1GB</c>.
	/// </summary>
	[JsonPropertyName("quota")]
	public required string Quota { get; init; }

	/// <summary>The license stack the pool draws on, for example <c>enterprise</c> or <c>download-trial</c>.</summary>
	[JsonPropertyName("stack_id")]
	public required string StackId { get; init; }

	/// <summary>The pool's description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The peers in the pool: GUIDs separated by commas, or <c>*</c> for every peer.</summary>
	[JsonPropertyName("peers")]
	public string? Peers { get; init; }
}
