using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>WMI collections (<c>data/inputs/win-wmi-collections</c>). Only Splunk on Windows has them; elsewhere every call answers 404.</summary>
public interface IWmiInputs
{
	/// <summary>Lists the WMI collections (<c>GET data/inputs/win-wmi-collections</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per collection.</returns>
	[Get("services/data/inputs/win-wmi-collections")]
	Task<SplunkFeed<WmiInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a WMI collection (<c>POST data/inputs/win-wmi-collections</c>).</summary>
	/// <param name="request">The collection.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the collection.</returns>
	[Post("services/data/inputs/win-wmi-collections")]
	Task<SplunkFeed<WmiInput>> CreateAsync([Body] WmiInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a WMI collection (<c>GET data/inputs/win-wmi-collections/{name}</c>).</summary>
	/// <param name="name">The collection's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the collection.</returns>
	[Get("services/data/inputs/win-wmi-collections/{name}")]
	Task<SplunkFeed<WmiInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a WMI collection (<c>POST data/inputs/win-wmi-collections/{name}</c>).</summary>
	/// <param name="name">The collection's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the collection.</returns>
	[Post("services/data/inputs/win-wmi-collections/{name}")]
	Task<SplunkFeed<WmiInput>> UpdateAsync(string name, [Body] WmiInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a WMI collection (<c>DELETE data/inputs/win-wmi-collections/{name}</c>); 400 when it does not exist.</summary>
	/// <param name="name">The collection's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the collection is deleted.</returns>
	[Delete("services/data/inputs/win-wmi-collections/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
