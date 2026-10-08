using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Field aliases, the <c>FIELDALIAS-</c> attributes in <c>props.conf</c> (<c>data/props/fieldaliases</c>).</summary>
/// <remarks>An entry is named <c>{stanza} : FIELDALIAS-{name}</c>; pass that name to the single-entry operations.</remarks>
public interface IFieldAliases
{
	/// <summary>Lists field aliases (<c>GET data/props/fieldaliases</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per alias group.</returns>
	[Get("services/data/props/fieldaliases")]
	Task<SplunkFeed<FieldAlias>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a field alias group (<c>POST data/props/fieldaliases</c>).</summary>
	/// <param name="request">The name, stanza and aliases.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new alias group.</returns>
	[Post("services/data/props/fieldaliases")]
	Task<SplunkFeed<FieldAlias>> CreateAsync([Body] FieldAliasCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one field alias group (<c>GET data/props/fieldaliases/{name}</c>).</summary>
	/// <param name="name">The entry name, for example <c>access_combined : FIELDALIAS-src</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/props/fieldaliases/{name}")]
	Task<SplunkFeed<FieldAlias>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Replaces the aliases of a field alias group (<c>POST data/props/fieldaliases/{name}</c>).</summary>
	/// <param name="name">The entry name.</param>
	/// <param name="request">The complete new set of aliases: Splunk removes those left out.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated alias group.</returns>
	[Post("services/data/props/fieldaliases/{name}")]
	Task<SplunkFeed<FieldAlias>> UpdateAsync(string name, [Body] FieldAliasUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a field alias group (<c>DELETE data/props/fieldaliases/{name}</c>).</summary>
	/// <param name="name">The entry name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the alias group is deleted.</returns>
	[Delete("services/data/props/fieldaliases/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
