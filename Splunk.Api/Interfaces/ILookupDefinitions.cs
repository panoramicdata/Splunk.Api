using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Lookup definitions, the lookup stanzas in <c>transforms.conf</c> (<c>data/transforms/lookups</c>).</summary>
/// <remarks>
/// Splunk lists only definitions it can use: a file lookup whose file does not exist is hidden from every operation
/// here (404) although its stanza exists.
/// </remarks>
public interface ILookupDefinitions
{
	/// <summary>Lists lookup definitions (<c>GET data/transforms/lookups</c>).</summary>
	/// <param name="options">Paging and filtering, and <c>getsize</c>; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per lookup definition.</returns>
	[Get("services/data/transforms/lookups")]
	Task<SplunkFeed<LookupDefinition>> ListAsync([Query] LookupDefinitionListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a lookup definition (<c>POST data/transforms/lookups</c>).</summary>
	/// <param name="request">The name and settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new lookup definition.</returns>
	[Post("services/data/transforms/lookups")]
	Task<SplunkFeed<LookupDefinition>> CreateAsync([Body] LookupDefinitionCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one lookup definition (<c>GET data/transforms/lookups/{name}</c>).</summary>
	/// <param name="name">The lookup definition name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	/// <remarks>The reference documents a <c>replicate_delta</c> parameter here; Splunk 10.6 refuses it, so it is not offered.</remarks>
	[Get("services/data/transforms/lookups/{name}")]
	Task<SplunkFeed<LookupDefinition>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Replaces a lookup definition's settings (<c>POST data/transforms/lookups/{name}</c>).</summary>
	/// <param name="name">The lookup definition name.</param>
	/// <param name="request">The complete new settings, including the file, command or collection: settings left out are removed.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated lookup definition.</returns>
	[Post("services/data/transforms/lookups/{name}")]
	Task<SplunkFeed<LookupDefinition>> UpdateAsync(string name, [Body] LookupDefinitionUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a lookup definition (<c>DELETE data/transforms/lookups/{name}</c>).</summary>
	/// <param name="name">The lookup definition name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the lookup definition is deleted.</returns>
	[Delete("services/data/transforms/lookups/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
