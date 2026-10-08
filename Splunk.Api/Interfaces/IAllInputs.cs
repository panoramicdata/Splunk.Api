using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Every input of every kind, including modular inputs (<c>data/inputs/all</c>). Read-only.</summary>
public interface IAllInputs
{
	/// <summary>Lists every input (<c>GET data/inputs/all</c>).</summary>
	/// <param name="options">Paging, filtering and <c>common</c>, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per input; <see cref="DataInput.Kind"/> tells the kinds apart.</returns>
	[Get("services/data/inputs/all")]
	Task<SplunkFeed<DataInput>> ListAsync([Query] DataInputListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets the inputs with a given name, of any kind (<c>GET data/inputs/all/{name}</c>).</summary>
	/// <param name="name">The input's name, for example a port or a monitored path.</param>
	/// <param name="options">The <c>common</c> option, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the matching inputs.</returns>
	[Get("services/data/inputs/all/{name}")]
	Task<SplunkFeed<DataInput>> GetAsync(string name, [Query] DataInputOptions? options, CancellationToken cancellationToken);
}
