using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Search-time sourcetype renames, the <c>rename</c> attributes in <c>props.conf</c> (<c>data/props/sourcetype-rename</c>).</summary>
/// <remarks>An entry is named by the original sourcetype.</remarks>
public interface ISourcetypeRenames
{
	/// <summary>Lists renamed sourcetypes (<c>GET data/props/sourcetype-rename</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per renamed sourcetype.</returns>
	[Get("services/data/props/sourcetype-rename")]
	Task<SplunkFeed<SourcetypeRename>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Renames a sourcetype (<c>POST data/props/sourcetype-rename</c>).</summary>
	/// <param name="request">The original and new sourcetype names.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new rename.</returns>
	[Post("services/data/props/sourcetype-rename")]
	Task<SplunkFeed<SourcetypeRename>> CreateAsync([Body] SourcetypeRenameCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one renamed sourcetype (<c>GET data/props/sourcetype-rename/{name}</c>).</summary>
	/// <param name="name">The original sourcetype name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/props/sourcetype-rename/{name}")]
	Task<SplunkFeed<SourcetypeRename>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes the new name of a renamed sourcetype (<c>POST data/props/sourcetype-rename/{name}</c>).</summary>
	/// <param name="name">The original sourcetype name.</param>
	/// <param name="request">The new sourcetype name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated rename.</returns>
	[Post("services/data/props/sourcetype-rename/{name}")]
	Task<SplunkFeed<SourcetypeRename>> UpdateAsync(string name, [Body] SourcetypeRenameUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Removes a rename, restoring the original sourcetype name (<c>DELETE data/props/sourcetype-rename/{name}</c>).</summary>
	/// <param name="name">The original sourcetype name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the rename is removed.</returns>
	[Delete("services/data/props/sourcetype-rename/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
