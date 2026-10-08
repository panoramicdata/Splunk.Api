using Refit;

namespace Splunk.Api.Models.Access;

/// <summary>Which identity provider metadata to read (<c>GET admin/SAML-idp-metadata</c>).</summary>
public sealed class SamlIdpMetadataOptions
{
	/// <summary>The full path of the metadata file, local to the Splunk server (<c>idpMetadataFile</c>).</summary>
	[AliasAs("idpMetadataFile")]
	public string? IdpMetadataFile { get; init; }
}
