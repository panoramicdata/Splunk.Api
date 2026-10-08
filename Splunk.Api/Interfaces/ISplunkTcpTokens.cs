using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Tokens forwarders must present to receiving ports (<c>data/inputs/tcp/splunktcptoken</c>). Requires the
/// <c>edit_splunktcp_token</c> capability.
/// </summary>
public interface ISplunkTcpTokens
{
	/// <summary>Lists the receiver tokens (<c>GET data/inputs/tcp/splunktcptoken</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per token, named <c>splunktcptoken://{name}</c>.</returns>
	[Get("services/data/inputs/tcp/splunktcptoken")]
	Task<SplunkFeed<SplunkTcpToken>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a receiver token (<c>POST data/inputs/tcp/splunktcptoken</c>).</summary>
	/// <param name="request">The token.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token.</returns>
	[Post("services/data/inputs/tcp/splunktcptoken")]
	Task<SplunkFeed<SplunkTcpToken>> CreateAsync([Body] SplunkTcpTokenCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a receiver token (<c>GET data/inputs/tcp/splunktcptoken/{name}</c>).</summary>
	/// <param name="name">The token's name, with or without the <c>splunktcptoken://</c> prefix.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token.</returns>
	[Get("services/data/inputs/tcp/splunktcptoken/{name}")]
	Task<SplunkFeed<SplunkTcpToken>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a receiver token's value (<c>POST data/inputs/tcp/splunktcptoken/{name}</c>).</summary>
	/// <param name="name">The token's name.</param>
	/// <param name="request">The new value.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token.</returns>
	[Post("services/data/inputs/tcp/splunktcptoken/{name}")]
	Task<SplunkFeed<SplunkTcpToken>> UpdateAsync(string name, [Body] SplunkTcpTokenUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a receiver token (<c>DELETE data/inputs/tcp/splunktcptoken/{name}</c>).</summary>
	/// <param name="name">The token's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the token is deleted.</returns>
	[Delete("services/data/inputs/tcp/splunktcptoken/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
