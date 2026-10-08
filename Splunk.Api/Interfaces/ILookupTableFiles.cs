using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Lookup table files (<c>data/lookup-table-files</c>). Splunk Enterprise only.</summary>
/// <remarks>
/// Splunk does not take a file's content through this endpoint: creating or replacing a file moves a file that is already
/// on the Splunk server, below the lookup staging area <c>$SPLUNK_HOME/var/run/splunk/lookup_tmp</c>, into the app's
/// <c>lookups</c> directory. Use a namespace (<see cref="SplunkClient.InNamespace(string, string)"/>) to choose the app.
/// </remarks>
public interface ILookupTableFiles
{
	/// <summary>Lists lookup table files (<c>GET data/lookup-table-files</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per file, named by file name.</returns>
	[Get("services/data/lookup-table-files")]
	Task<SplunkFeed<LookupTableFile>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a lookup table file from a staged file (<c>POST data/lookup-table-files</c>).</summary>
	/// <param name="request">The file name and the staged file's path on the server.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new file. Splunk answers 400 when the path is outside the staging area.</returns>
	[Post("services/data/lookup-table-files")]
	Task<SplunkFeed<LookupTableFile>> CreateAsync([Body] LookupTableFileCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one lookup table file (<c>GET data/lookup-table-files/{name}</c>).</summary>
	/// <param name="name">The file name, for example <c>assets.csv</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/lookup-table-files/{name}")]
	Task<SplunkFeed<LookupTableFile>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Replaces a lookup table file with a staged file (<c>POST data/lookup-table-files/{name}</c>).</summary>
	/// <param name="name">The file name.</param>
	/// <param name="request">The staged replacement file's path on the server.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated file.</returns>
	[Post("services/data/lookup-table-files/{name}")]
	Task<SplunkFeed<LookupTableFile>> UpdateAsync(string name, [Body] LookupTableFileUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a lookup table file (<c>DELETE data/lookup-table-files/{name}</c>).</summary>
	/// <param name="name">The file name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the file is deleted.</returns>
	[Delete("services/data/lookup-table-files/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
