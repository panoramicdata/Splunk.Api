using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// SAML metadata and certificate replication (<c>admin/SAML-idp-metadata</c>, <c>admin/SAML-sp-metadata</c> and
/// <c>admin/replicate-SAML-certs</c>). Requires the <c>change_authentication</c> capability.
/// </summary>
public interface ISamlMetadata
{
	/// <summary>Reads identity provider SAML metadata (<c>GET admin/SAML-idp-metadata</c>).</summary>
	/// <param name="options">The metadata file to read, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The parsed metadata; empty when no file is given.</returns>
	/// <remarks>A file Splunk cannot read or parse is answered with 400.</remarks>
	[Get("services/admin/SAML-idp-metadata")]
	Task<SplunkFeed<SamlIdpMetadata>> GetIdentityProviderMetadataAsync([Query] SamlIdpMetadataOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets this Splunk instance's service provider SAML metadata (<c>GET admin/SAML-sp-metadata</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>spMetadata</c>.</returns>
	[Get("services/admin/SAML-sp-metadata")]
	Task<SplunkFeed<SamlSpMetadata>> GetServiceProviderMetadataAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Replicates the identity provider certificates in <c>$SPLUNK_HOME/etc/auth/idpCerts</c> across a search head cluster
	/// (<c>POST admin/replicate-SAML-certs</c>).
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when replication has been requested.</returns>
	/// <remarks>Only available on search head clusters with the KV store enabled.</remarks>
	[Post("services/admin/replicate-SAML-certs")]
	Task ReplicateCertificatesAsync(CancellationToken cancellationToken);
}
