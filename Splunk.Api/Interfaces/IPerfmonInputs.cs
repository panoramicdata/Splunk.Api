using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Windows Performance Monitor inputs (<c>data/inputs/win-perfmon</c>). Only Splunk on Windows has them; elsewhere every call answers 404.</summary>
public interface IPerfmonInputs
{
	/// <summary>Lists the Performance Monitor inputs (<c>GET data/inputs/win-perfmon</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per input.</returns>
	[Get("services/data/inputs/win-perfmon")]
	Task<SplunkFeed<PerfmonInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a Performance Monitor input (<c>POST data/inputs/win-perfmon</c>).</summary>
	/// <param name="request">The input.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/win-perfmon")]
	Task<SplunkFeed<PerfmonInput>> CreateAsync([Body] PerfmonInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a Performance Monitor input (<c>GET data/inputs/win-perfmon/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/win-perfmon/{name}")]
	Task<SplunkFeed<PerfmonInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a Performance Monitor input (<c>POST data/inputs/win-perfmon/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/win-perfmon/{name}")]
	Task<SplunkFeed<PerfmonInput>> UpdateAsync(string name, [Body] PerfmonInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a Performance Monitor input (<c>DELETE data/inputs/win-perfmon/{name}</c>).</summary>
	/// <param name="name">The input's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the input is deleted.</returns>
	[Delete("services/data/inputs/win-perfmon/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
