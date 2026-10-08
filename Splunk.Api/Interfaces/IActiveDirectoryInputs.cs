using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Active Directory monitoring inputs (<c>data/inputs/ad</c>). Only Splunk on Windows has them; elsewhere every call answers 404.</summary>
public interface IActiveDirectoryInputs
{
	/// <summary>Lists the Active Directory monitoring inputs (<c>GET data/inputs/ad</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per input.</returns>
	[Get("services/data/inputs/ad")]
	Task<SplunkFeed<ActiveDirectoryInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates an Active Directory monitoring input (<c>POST data/inputs/ad</c>).</summary>
	/// <param name="request">The input.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/ad")]
	Task<SplunkFeed<ActiveDirectoryInput>> CreateAsync([Body] ActiveDirectoryInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets an Active Directory monitoring input (<c>GET data/inputs/ad/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/ad/{name}")]
	Task<SplunkFeed<ActiveDirectoryInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes an Active Directory monitoring input (<c>POST data/inputs/ad/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/ad/{name}")]
	Task<SplunkFeed<ActiveDirectoryInput>> UpdateAsync(string name, [Body] ActiveDirectoryInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes an Active Directory monitoring input (<c>DELETE data/inputs/ad/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the input is deleted.</returns>
	[Delete("services/data/inputs/ad/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
