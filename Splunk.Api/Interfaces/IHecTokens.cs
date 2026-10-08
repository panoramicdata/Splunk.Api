using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>
/// HTTP Event Collector tokens and global settings (<c>data/inputs/http</c>). Send events with the tokens through
/// <see cref="SplunkHecClient"/>.
/// </summary>
public interface IHecTokens
{
	/// <summary>Lists the tokens (<c>GET data/inputs/http</c>). The global settings entry is not listed.</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per token, named <c>http://{name}</c>.</returns>
	[Get("services/data/inputs/http")]
	Task<SplunkFeed<HecToken>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a token (<c>POST data/inputs/http</c>); Splunk generates its value.</summary>
	/// <param name="request">The token.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token, including its value.</returns>
	[Post("services/data/inputs/http")]
	Task<SplunkFeed<HecToken>> CreateAsync([Body] HecTokenCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a token (<c>GET data/inputs/http/{name}</c>).</summary>
	/// <param name="name">The token's name, with or without the <c>http://</c> prefix.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token.</returns>
	[Get("services/data/inputs/http/{name}")]
	Task<SplunkFeed<HecToken>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a token (<c>POST data/inputs/http/{name}</c>).</summary>
	/// <param name="name">The token's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token.</returns>
	[Post("services/data/inputs/http/{name}")]
	Task<SplunkFeed<HecToken>> UpdateAsync(string name, [Body] HecTokenUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Gets the collector's global settings (<c>GET data/inputs/http/http</c>): pass
	/// <see cref="HecSettingsUpdateRequest.SettingsName"/>.
	/// </summary>
	/// <param name="name">The settings entry's name, <c>http</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the settings.</returns>
	[Get("services/data/inputs/http/{name}")]
	Task<SplunkFeed<HecSettings>> GetSettingsAsync(string name, CancellationToken cancellationToken);

	/// <summary>
	/// Changes the collector's global settings (<c>POST data/inputs/http/http</c>): pass
	/// <see cref="HecSettingsUpdateRequest.SettingsName"/>. The reference documents these settings on
	/// <c>POST data/inputs/http</c>, but Splunk 10.6 requires the <c>http</c> entry's own path.
	/// </summary>
	/// <param name="name">The settings entry's name, <c>http</c>.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the settings.</returns>
	[Post("services/data/inputs/http/{name}")]
	Task<SplunkFeed<HecSettings>> UpdateSettingsAsync(string name, [Body] HecSettingsUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a token (<c>DELETE data/inputs/http/{name}</c>).</summary>
	/// <param name="name">The token's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the token is deleted.</returns>
	[Delete("services/data/inputs/http/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Disables a token (<c>POST data/inputs/http/{name}/disable</c>).</summary>
	/// <param name="name">The token's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token.</returns>
	[Post("services/data/inputs/http/{name}/disable")]
	Task<SplunkFeed<HecToken>> DisableAsync(string name, CancellationToken cancellationToken);

	/// <summary>Enables a token (<c>POST data/inputs/http/{name}/enable</c>). This reloads the collector.</summary>
	/// <param name="name">The token's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token.</returns>
	[Post("services/data/inputs/http/{name}/enable")]
	Task<SplunkFeed<HecToken>> EnableAsync(string name, CancellationToken cancellationToken);

	/// <summary>Gives a token a new value (<c>POST data/inputs/http/{name}/rotate</c>); the old value stops working.</summary>
	/// <param name="name">The token's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the token and its new value.</returns>
	[Post("services/data/inputs/http/{name}/rotate")]
	Task<SplunkFeed<HecToken>> RotateAsync(string name, CancellationToken cancellationToken);
}
