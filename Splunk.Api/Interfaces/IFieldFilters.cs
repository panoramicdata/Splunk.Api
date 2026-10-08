using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Field filters (<c>authorization/fieldfilters</c>), which remove or hash sensitive field values in search results
/// for every role not exempt from them. Reading requires the <c>admin</c>, <c>sc_admin</c> or <c>power</c> role;
/// changing requires <c>admin</c> or <c>sc_admin</c>.
/// </summary>
public interface IFieldFilters
{
	/// <summary>Lists the field filters (<c>GET authorization/fieldfilters</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The field filters.</returns>
	[Get("services/authorization/fieldfilters")]
	Task<SplunkFeed<FieldFilter>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a field filter (<c>POST authorization/fieldfilters</c>).</summary>
	/// <param name="request">The field filter.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created field filter.</returns>
	/// <remarks>The reference documents no parameters for this POST; the request carries those of the update.</remarks>
	[Post("services/authorization/fieldfilters")]
	Task<SplunkFeed<FieldFilter>> CreateAsync([Body] FieldFilterCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one field filter (<c>GET authorization/fieldfilters/{name}</c>).</summary>
	/// <param name="name">The field filter name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one field filter.</returns>
	[Get("services/authorization/fieldfilters/{name}")]
	Task<SplunkFeed<FieldFilter>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Updates a field filter (<c>POST authorization/fieldfilters/{name}</c>).</summary>
	/// <param name="name">The field filter name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated field filter.</returns>
	[Post("services/authorization/fieldfilters/{name}")]
	Task<SplunkFeed<FieldFilter>> UpdateAsync(string name, [Body] FieldFilterUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a field filter (<c>DELETE authorization/fieldfilters/{name}</c>).</summary>
	/// <param name="name">The field filter name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the field filter is deleted.</returns>
	[Delete("services/authorization/fieldfilters/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
