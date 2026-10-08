using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Outputs;

namespace Splunk.Api.Interfaces;

/// <summary>The receivers this server forwards to (<c>data/outputs/tcp/server</c>). The name of each is <c>host:port</c>.</summary>
public interface ITcpOutputServers
{
	/// <summary>Lists the receivers and their connection status (<c>GET data/outputs/tcp/server</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per receiver.</returns>
	[Get("services/data/outputs/tcp/server")]
	Task<SplunkFeed<TcpOutputServer>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Adds a receiver (<c>POST data/outputs/tcp/server</c>). When no default group is set, Splunk puts it in a new
	/// <c>default-autolb-group</c>, makes that the default and starts forwarding.
	/// </summary>
	/// <param name="request">The receiver.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the receiver.</returns>
	[Post("services/data/outputs/tcp/server")]
	Task<SplunkFeed<TcpOutputServer>> CreateAsync([Body] TcpOutputServerCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a receiver (<c>GET data/outputs/tcp/server/{name}</c>).</summary>
	/// <param name="name">The receiver as <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the receiver.</returns>
	[Get("services/data/outputs/tcp/server/{name}")]
	Task<SplunkFeed<TcpOutputServer>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a receiver (<c>POST data/outputs/tcp/server/{name}</c>).</summary>
	/// <param name="name">The receiver as <c>host:port</c>.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the receiver.</returns>
	[Post("services/data/outputs/tcp/server/{name}")]
	Task<SplunkFeed<TcpOutputServer>> UpdateAsync(string name, [Body] TcpOutputServerUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Removes a receiver (<c>DELETE data/outputs/tcp/server/{name}</c>), and the automatic group it created.</summary>
	/// <param name="name">The receiver as <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the receiver is removed.</returns>
	[Delete("services/data/outputs/tcp/server/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Lists the current connections to a receiver (<c>GET data/outputs/tcp/server/{name}/allconnections</c>).</summary>
	/// <param name="name">The receiver as <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per connection.</returns>
	[Get("services/data/outputs/tcp/server/{name}/allconnections")]
	Task<SplunkFeed<TcpOutputServer>> ListConnectionsAsync(string name, CancellationToken cancellationToken);
}
