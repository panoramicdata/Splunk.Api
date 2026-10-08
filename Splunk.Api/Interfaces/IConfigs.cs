using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Configuration;

namespace Splunk.Api.Interfaces;

/// <summary>
/// The stanzas of <c>.conf</c> configuration files (<c>configs/conf-{file}</c>).
/// </summary>
/// <remarks>
/// The namespace (<see cref="SplunkClient.InNamespace(string, string)"/>) selects which layer of the file is read and
/// which app's <c>local</c> directory is written: use <c>nobody</c> and an app to write app-level configuration. Writing
/// needs the <c>admin_all_objects</c> capability. Many settings take effect only after a reload or restart.
/// </remarks>
public interface IConfigs
{
	/// <summary>Lists the stanzas of a configuration file (<c>GET configs/conf-{file}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>, for example <c>props</c>.</param>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per stanza.</returns>
	[Get("services/configs/conf-{file}")]
	Task<SplunkFeed<ConfStanza>> ListStanzasAsync(string file, [Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a stanza, with any keys in <see cref="SplunkFormRequest.AdditionalParameters"/> (<c>POST configs/conf-{file}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>. A file that does not exist yet is created.</param>
	/// <param name="request">The stanza name and its keys.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new stanza.</returns>
	[Post("services/configs/conf-{file}")]
	Task<SplunkFeed<ConfStanza>> CreateStanzaAsync(string file, [Body] ConfStanzaCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one stanza (<c>GET configs/conf-{file}/{stanza}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name. Characters such as <c>/</c> and <c>:</c> are escaped.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the stanza.</returns>
	[Get("services/configs/conf-{file}/{stanza}")]
	Task<SplunkFeed<ConfStanza>> GetStanzaAsync(string file, string stanza, CancellationToken cancellationToken);

	/// <summary>Adds or changes keys of a stanza (<c>POST configs/conf-{file}/{stanza}</c>). Keys not sent are unchanged.</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name.</param>
	/// <param name="values">The keys and values to write.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated stanza.</returns>
	[Post("services/configs/conf-{file}/{stanza}")]
	Task<SplunkFeed<ConfStanza>> UpdateStanzaAsync(string file, string stanza, [Body] IDictionary<string, string?> values, CancellationToken cancellationToken);

	/// <summary>Deletes a stanza from the namespace's <c>local</c> layer (<c>DELETE configs/conf-{file}/{stanza}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the stanza is deleted.</returns>
	[Delete("services/configs/conf-{file}/{stanza}")]
	Task DeleteStanzaAsync(string file, string stanza, CancellationToken cancellationToken);
}
