using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// RSA Authentication Manager multifactor authentication (<c>admin/Rsa-MFA</c> and
/// <c>admin/Rsa-MFA-config-verify</c>). Requires the <c>change_authentication</c> capability. Splunk Enterprise only.
/// </summary>
public interface IRsaMfa
{
	/// <summary>Lists the RSA configuration settings (<c>GET admin/Rsa-MFA</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The configurations. Splunk hides the access key.</returns>
	[Get("services/admin/Rsa-MFA")]
	Task<SplunkFeed<RsaMfaConfiguration>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates or edits the RSA configuration (<c>POST admin/Rsa-MFA</c>).</summary>
	/// <param name="request">The configuration.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The configuration.</returns>
	[Post("services/admin/Rsa-MFA")]
	Task<SplunkFeed<RsaMfaConfiguration>> SaveAsync([Body] RsaMfaRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes the RSA configuration (<c>DELETE admin/Rsa-MFA</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is deleted.</returns>
	/// <remarks>The reference documents this DELETE on the collection path, without a configuration name.</remarks>
	[Delete("services/admin/Rsa-MFA")]
	Task DeleteAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Verifies an RSA configuration by authenticating a user (<c>POST admin/Rsa-MFA-config-verify/{rsa-stanza-name}</c>).
	/// </summary>
	/// <param name="stanzaName">The RSA configuration stanza name.</param>
	/// <param name="request">The RSA user name and passcode.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Whether the configuration is valid, as Splunk reports it.</returns>
	/// <remarks>
	/// Without an RSA configuration, a live Splunk 10.6 answers 404 <c>No Rsa MFA configuration was found.</c>
	/// </remarks>
	[Post("services/admin/Rsa-MFA-config-verify/{stanzaName}")]
	Task<SplunkFeed<SplunkDynamicContent>> VerifyAsync(string stanzaName, [Body] RsaMfaVerifyRequest request, CancellationToken cancellationToken);
}
