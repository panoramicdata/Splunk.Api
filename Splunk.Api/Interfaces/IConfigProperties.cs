using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Configuration;

namespace Splunk.Api.Interfaces;

/// <summary>
/// <c>.conf</c> configuration files, stanzas and individual keys (<c>properties</c>).
/// </summary>
/// <remarks>
/// The namespace (<see cref="SplunkClient.InNamespace(string, string)"/>) selects the layer that is read and the app whose
/// <c>local</c> directory is written. Deleting a stanza or key needs an explicit namespace (not <c>services/</c> and not
/// a <c>-</c> wildcard) and removes only the <c>local</c> copy. Writing needs the <c>admin_all_objects</c> capability.
/// </remarks>
public interface IConfigProperties
{
	/// <summary>Lists the configuration files (<c>GET properties</c>). Entries have a name and no content.</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per file.</returns>
	[Get("services/properties")]
	Task<SplunkFeed<SplunkDynamicContent>> ListFilesAsync(CancellationToken cancellationToken);

	/// <summary>Creates an empty configuration file in the namespace's <c>local</c> directory (<c>POST properties</c>).</summary>
	/// <param name="request">The file name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the file exists. Splunk answers <c>201</c> with no body.</returns>
	/// <remarks>The REST API cannot delete a file once created.</remarks>
	[Post("services/properties")]
	Task CreateFileAsync([Body] PropertiesFileCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Lists the stanzas of a file (<c>GET properties/{file}</c>). Entries have a name and no content.</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per stanza.</returns>
	[Get("services/properties/{file}")]
	Task<SplunkFeed<SplunkDynamicContent>> ListStanzasAsync(string file, CancellationToken cancellationToken);

	/// <summary>Creates an empty stanza (<c>POST properties/{file}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="request">The stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the stanza exists. Splunk answers <c>201</c> with no body.</returns>
	[Post("services/properties/{file}")]
	Task CreateStanzaAsync(string file, [Body] PropertiesStanzaCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets the keys of a stanza (<c>GET properties/{file}/{stanza}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per key: the entry name is the key and its content the value.</returns>
	[Get("services/properties/{file}/{stanza}")]
	Task<SplunkFeed<string>> GetStanzaAsync(string file, string stanza, CancellationToken cancellationToken);

	/// <summary>Adds or changes keys of a stanza (<c>POST properties/{file}/{stanza}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name.</param>
	/// <param name="values">The keys and values to write; at least one.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed carrying only Splunk's confirmation message, for example <c>Successfully modified 2 key(s)</c>.</returns>
	[Post("services/properties/{file}/{stanza}")]
	Task<SplunkFeed<SplunkDynamicContent>> UpdateStanzaAsync(string file, string stanza, [Body] IDictionary<string, string?> values, CancellationToken cancellationToken);

	/// <summary>Deletes the <c>local</c> copy of a stanza (<c>DELETE properties/{file}/{stanza}?local_only=true</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the stanza is deleted.</returns>
	/// <remarks><c>local_only=true</c> is required: without it Splunk only disables the stanza.</remarks>
	[Delete("services/properties/{file}/{stanza}?local_only=true")]
	Task DeleteStanzaAsync(string file, string stanza, CancellationToken cancellationToken);

	/// <summary>Gets one key's value as plain text (<c>GET properties/{file}/{stanza}/{key}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name.</param>
	/// <param name="key">The key.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The value exactly as Splunk returns it (<c>text/plain</c>).</returns>
	[Get("services/properties/{file}/{stanza}/{key}")]
	Task<string> GetValueAsync(string file, string stanza, string key, CancellationToken cancellationToken);

	/// <summary>Sets one key (<c>POST properties/{file}/{stanza}/{key}</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name.</param>
	/// <param name="key">The key.</param>
	/// <param name="request">The value.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed carrying only Splunk's confirmation message.</returns>
	[Post("services/properties/{file}/{stanza}/{key}")]
	Task<SplunkFeed<SplunkDynamicContent>> SetValueAsync(string file, string stanza, string key, [Body] PropertyValueRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes the <c>local</c> copy of one key (<c>DELETE properties/{file}/{stanza}/{key}?local_only=true</c>).</summary>
	/// <param name="file">The file name without <c>.conf</c>.</param>
	/// <param name="stanza">The stanza name.</param>
	/// <param name="key">The key.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the key is deleted.</returns>
	/// <remarks><c>local_only=true</c> is required: without it Splunk answers <c>400 Key deletion is not supported</c>.</remarks>
	[Delete("services/properties/{file}/{stanza}/{key}?local_only=true")]
	Task DeleteValueAsync(string file, string stanza, string key, CancellationToken cancellationToken);
}
