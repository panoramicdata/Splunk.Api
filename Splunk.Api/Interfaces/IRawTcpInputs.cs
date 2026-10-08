using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Raw TCP inputs (<c>data/inputs/tcp/raw</c>). Requires the <c>edit_tcp</c> capability.</summary>
public interface IRawTcpInputs
{
	/// <summary>Lists the raw TCP inputs (<c>GET data/inputs/tcp/raw</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per port.</returns>
	[Get("services/data/inputs/tcp/raw")]
	Task<SplunkFeed<TcpInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Opens a port that accepts raw TCP data (<c>POST data/inputs/tcp/raw</c>).</summary>
	/// <param name="request">The input.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/tcp/raw")]
	Task<SplunkFeed<TcpInput>> CreateAsync([Body] RawTcpInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a raw TCP input (<c>GET data/inputs/tcp/raw/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c> for an input restricted to one host.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/tcp/raw/{name}")]
	Task<SplunkFeed<TcpInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a raw TCP input (<c>POST data/inputs/tcp/raw/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/tcp/raw/{name}")]
	Task<SplunkFeed<TcpInput>> UpdateAsync(string name, [Body] RawTcpInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Closes a raw TCP port (<c>DELETE data/inputs/tcp/raw/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the input is deleted.</returns>
	[Delete("services/data/inputs/tcp/raw/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Lists the senders connected to a port (<c>GET data/inputs/tcp/raw/{name}/connections</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per connection.</returns>
	[Get("services/data/inputs/tcp/raw/{name}/connections")]
	Task<SplunkFeed<InputConnection>> ListConnectionsAsync(string name, CancellationToken cancellationToken);
}
