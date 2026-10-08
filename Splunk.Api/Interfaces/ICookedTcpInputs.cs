using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Cooked TCP inputs, the ports forwarders send to (<c>data/inputs/tcp/cooked</c>).</summary>
public interface ICookedTcpInputs
{
	/// <summary>Lists the cooked TCP inputs (<c>GET data/inputs/tcp/cooked</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per port.</returns>
	[Get("services/data/inputs/tcp/cooked")]
	Task<SplunkFeed<TcpInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Opens a receiving port for forwarders (<c>POST data/inputs/tcp/cooked</c>).</summary>
	/// <param name="request">The input.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/tcp/cooked")]
	Task<SplunkFeed<TcpInput>> CreateAsync([Body] CookedTcpInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a cooked TCP input (<c>GET data/inputs/tcp/cooked/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c> for an input restricted to one host.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/tcp/cooked/{name}")]
	Task<SplunkFeed<TcpInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a cooked TCP input (<c>POST data/inputs/tcp/cooked/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/tcp/cooked/{name}")]
	Task<SplunkFeed<TcpInput>> UpdateAsync(string name, [Body] CookedTcpInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Closes a receiving port (<c>DELETE data/inputs/tcp/cooked/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the input is deleted.</returns>
	[Delete("services/data/inputs/tcp/cooked/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Lists the forwarders connected to a port (<c>GET data/inputs/tcp/cooked/{name}/connections</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per connection.</returns>
	[Get("services/data/inputs/tcp/cooked/{name}/connections")]
	Task<SplunkFeed<InputConnection>> ListConnectionsAsync(string name, CancellationToken cancellationToken);
}
