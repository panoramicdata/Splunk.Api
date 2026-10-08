using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Automatic lookups, the <c>LOOKUP-</c> attributes in <c>props.conf</c> (<c>data/props/lookups</c>).</summary>
/// <remarks>An entry is named <c>{stanza} : LOOKUP-{name}</c>; pass that name to the single-entry operations.</remarks>
public interface IAutomaticLookups
{
	/// <summary>Lists automatic lookups (<c>GET data/props/lookups</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per automatic lookup.</returns>
	[Get("services/data/props/lookups")]
	Task<SplunkFeed<AutomaticLookup>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates an automatic lookup (<c>POST data/props/lookups</c>).</summary>
	/// <param name="request">The name, stanza, lookup definition and fields.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new automatic lookup.</returns>
	[Post("services/data/props/lookups")]
	Task<SplunkFeed<AutomaticLookup>> CreateAsync([Body] AutomaticLookupCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one automatic lookup (<c>GET data/props/lookups/{name}</c>).</summary>
	/// <param name="name">The entry name, for example <c>access_combined : LOOKUP-assets</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/props/lookups/{name}")]
	Task<SplunkFeed<AutomaticLookup>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Replaces an automatic lookup's settings (<c>POST data/props/lookups/{name}</c>).</summary>
	/// <param name="name">The entry name.</param>
	/// <param name="request">The complete new settings: Splunk removes input and output fields left out.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated automatic lookup.</returns>
	[Post("services/data/props/lookups/{name}")]
	Task<SplunkFeed<AutomaticLookup>> UpdateAsync(string name, [Body] AutomaticLookupUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes an automatic lookup (<c>DELETE data/props/lookups/{name}</c>).</summary>
	/// <param name="name">The entry name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the automatic lookup is deleted.</returns>
	[Delete("services/data/props/lookups/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
