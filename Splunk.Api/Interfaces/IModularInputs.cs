using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>The modular input kinds apps define (<c>data/modular-inputs</c>). Read-only.</summary>
public interface IModularInputs
{
	/// <summary>Lists the modular input kinds (<c>GET data/modular-inputs</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per kind.</returns>
	[Get("services/data/modular-inputs")]
	Task<SplunkFeed<ModularInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a modular input kind and its parameters (<c>GET data/modular-inputs/{name}</c>).</summary>
	/// <param name="name">The kind's name, for example <c>journald</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the kind.</returns>
	[Get("services/data/modular-inputs/{name}")]
	Task<SplunkFeed<ModularInput>> GetAsync(string name, CancellationToken cancellationToken);
}
