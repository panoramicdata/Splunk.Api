using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>This Splunk instance's service provider SAML metadata (<c>admin/SAML-sp-metadata</c>).</summary>
public sealed class SamlSpMetadata : SplunkContent
{
	/// <summary>The service provider metadata, as an XML <c>EntityDescriptor</c>.</summary>
	/// <remarks>The reference names this key <c>spMetadataPayload</c>; a live Splunk 10.6 returns <c>spMetadata</c>.</remarks>
	[JsonPropertyName("spMetadata")]
	public string? SpMetadata { get; init; }
}
