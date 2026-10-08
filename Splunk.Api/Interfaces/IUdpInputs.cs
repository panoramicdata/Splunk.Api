using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>UDP inputs (<c>data/inputs/udp</c>).</summary>
public interface IUdpInputs
{
	/// <summary>Lists the UDP inputs (<c>GET data/inputs/udp</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per port.</returns>
	[Get("services/data/inputs/udp")]
	Task<SplunkFeed<UdpInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Opens a UDP port (<c>POST data/inputs/udp</c>).</summary>
	/// <param name="request">The input.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/udp")]
	Task<SplunkFeed<UdpInput>> CreateAsync([Body] UdpInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a UDP input (<c>GET data/inputs/udp/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c> for an input restricted to one host.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/udp/{name}")]
	Task<SplunkFeed<UdpInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a UDP input (<c>POST data/inputs/udp/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/udp/{name}")]
	Task<SplunkFeed<UdpInput>> UpdateAsync(string name, [Body] UdpInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Closes a UDP port (<c>DELETE data/inputs/udp/{name}</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the input is deleted.</returns>
	[Delete("services/data/inputs/udp/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Lists the connections to a UDP port (<c>GET data/inputs/udp/{name}/connections</c>).</summary>
	/// <param name="name">The port, or <c>host:port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per connection.</returns>
	[Get("services/data/inputs/udp/{name}/connections")]
	Task<SplunkFeed<InputConnection>> ListConnectionsAsync(string name, CancellationToken cancellationToken);
}
