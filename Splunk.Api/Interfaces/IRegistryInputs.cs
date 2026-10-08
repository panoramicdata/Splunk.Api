using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Windows registry monitoring inputs (<c>data/inputs/registry</c>). Only Splunk on Windows has them; elsewhere every call answers 404.</summary>
public interface IRegistryInputs
{
	/// <summary>Lists the registry monitoring inputs (<c>GET data/inputs/registry</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per input.</returns>
	[Get("services/data/inputs/registry")]
	Task<SplunkFeed<RegistryInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a registry monitoring input (<c>POST data/inputs/registry</c>).</summary>
	/// <param name="request">The input.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/registry")]
	Task<SplunkFeed<RegistryInput>> CreateAsync([Body] RegistryInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a registry monitoring input (<c>GET data/inputs/registry/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/registry/{name}")]
	Task<SplunkFeed<RegistryInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a registry monitoring input (<c>POST data/inputs/registry/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/registry/{name}")]
	Task<SplunkFeed<RegistryInput>> UpdateAsync(string name, [Body] RegistryInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a registry monitoring input (<c>DELETE data/inputs/registry/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the input is deleted.</returns>
	[Delete("services/data/inputs/registry/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
