using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Search field configurations from <c>fields.conf</c>, and the tags on field values (<c>search/fields</c>).</summary>
public interface ISearchFields
{
	/// <summary>Lists the fields with a <c>fields.conf</c> configuration (<c>GET search/fields</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per field, without content.</returns>
	/// <remarks>Splunk returns no paging details and ignores paging parameters here.</remarks>
	[Get("services/search/fields")]
	Task<SplunkFeed<string>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Gets one field's configuration (<c>GET search/fields/{field_name}</c>).</summary>
	/// <param name="fieldName">The field name, for example <c>sourcetype</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>
	/// A feed with one entry whose content is Splunk's text rendering of the settings, for example
	/// <c>PropertiesMap: {INDEXED -&gt; 'True' INDEXED_VALUE -&gt; 'False' TOKENIZER -&gt; ''}</c>.
	/// </returns>
	[Get("services/search/fields/{fieldName}")]
	Task<SplunkFeed<string>> GetAsync(string fieldName, CancellationToken cancellationToken);

	/// <summary>Lists the tags on values of a field (<c>GET search/fields/{field_name}/tags</c>).</summary>
	/// <param name="fieldName">The field name, for example <c>host</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per tagged value, named <c>{value}::{tag}</c>, without content.</returns>
	[Get("services/search/fields/{fieldName}/tags")]
	Task<SplunkFeed<SplunkDynamicContent>> ListTagsAsync(string fieldName, CancellationToken cancellationToken);

	/// <summary>Adds and removes tags on one value of a field (<c>POST search/fields/{field_name}/tags</c>).</summary>
	/// <param name="fieldName">The field name.</param>
	/// <param name="request">The field value and the tags to add and remove.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with no entries and an informational message.</returns>
	[Post("services/search/fields/{fieldName}/tags")]
	Task<SplunkFeed<SplunkDynamicContent>> UpdateTagsAsync(string fieldName, [Body] FieldTagsUpdateRequest request, CancellationToken cancellationToken);
}
