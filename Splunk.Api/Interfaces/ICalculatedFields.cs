using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Calculated fields, the <c>EVAL-</c> expressions in <c>props.conf</c> (<c>data/props/calcfields</c>).</summary>
/// <remarks>An entry is named <c>{stanza} : EVAL-{field}</c>; pass that name to <see cref="GetAsync"/>, <see cref="UpdateAsync"/> and <see cref="DeleteAsync"/>.</remarks>
public interface ICalculatedFields
{
	/// <summary>Lists calculated fields (<c>GET data/props/calcfields</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per calculated field.</returns>
	[Get("services/data/props/calcfields")]
	Task<SplunkFeed<CalculatedField>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a calculated field (<c>POST data/props/calcfields</c>).</summary>
	/// <param name="request">The field name, stanza and eval expression.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new calculated field.</returns>
	[Post("services/data/props/calcfields")]
	Task<SplunkFeed<CalculatedField>> CreateAsync([Body] CalculatedFieldCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one calculated field (<c>GET data/props/calcfields/{name}</c>).</summary>
	/// <param name="name">The entry name, <c>{stanza} : EVAL-{field}</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/props/calcfields/{name}")]
	Task<SplunkFeed<CalculatedField>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a calculated field's expression (<c>POST data/props/calcfields/{name}</c>).</summary>
	/// <param name="name">The entry name, <c>{stanza} : EVAL-{field}</c>.</param>
	/// <param name="request">The new expression.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated calculated field.</returns>
	[Post("services/data/props/calcfields/{name}")]
	Task<SplunkFeed<CalculatedField>> UpdateAsync(string name, [Body] CalculatedFieldUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a calculated field (<c>DELETE data/props/calcfields/{name}</c>).</summary>
	/// <param name="name">The entry name, <c>{stanza} : EVAL-{field}</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the field is deleted.</returns>
	[Delete("services/data/props/calcfields/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
