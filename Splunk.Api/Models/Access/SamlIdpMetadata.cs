using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Identity provider SAML metadata read from a file (<c>admin/SAML-idp-metadata</c>).</summary>
public sealed class SamlIdpMetadata : SplunkContent
{
	/// <summary>The identity provider metadata, as XML.</summary>
	[JsonPropertyName("idpMetadataPayload")]
	public string? IdpMetadataPayload { get; init; }
}
