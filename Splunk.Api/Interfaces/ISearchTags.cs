using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Search-time tags from <c>tags.conf</c> (<c>search/tags</c>).</summary>
public interface ISearchTags
{
	/// <summary>Lists every tag (<c>GET search/tags</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per tag, without content.</returns>
	/// <remarks>Splunk returns no paging details and ignores paging parameters here.</remarks>
	[Get("services/search/tags")]
	Task<SplunkFeed<SplunkDynamicContent>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Lists the <c>field::value</c> pairs a tag applies to (<c>GET search/tags/{tag_name}</c>).</summary>
	/// <param name="tagName">The tag.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per pair, named <c>{field}::{value}</c>, without content.</returns>
	[Get("services/search/tags/{tagName}")]
	Task<SplunkFeed<SplunkDynamicContent>> GetAsync(string tagName, CancellationToken cancellationToken);

	/// <summary>Adds and removes the <c>field::value</c> pairs a tag applies to, creating the tag if needed (<c>POST search/tags/{tag_name}</c>).</summary>
	/// <param name="tagName">The tag.</param>
	/// <param name="request">The pairs to add and remove.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with no entries and an informational message.</returns>
	[Post("services/search/tags/{tagName}")]
	Task<SplunkFeed<SplunkDynamicContent>> UpdateAsync(string tagName, [Body] TagUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a tag from every <c>field::value</c> pair (<c>DELETE search/tags/{tag_name}</c>).</summary>
	/// <param name="tagName">The tag.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the tag is deleted.</returns>
	/// <remarks>Splunk marks the pairs <c>disabled</c> in <c>tags.conf</c> rather than removing the stanzas.</remarks>
	[Delete("services/search/tags/{tagName}")]
	Task DeleteAsync(string tagName, CancellationToken cancellationToken);
}
