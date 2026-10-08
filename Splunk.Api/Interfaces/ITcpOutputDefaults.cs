using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Outputs;

namespace Splunk.Api.Interfaces;

/// <summary>The global forwarding settings, <c>[tcpout]</c> in <c>outputs.conf</c> (<c>data/outputs/tcp/default</c>).</summary>
public interface ITcpOutputDefaults
{
	/// <summary>Lists the global forwarding settings (<c>GET data/outputs/tcp/default</c>); there is one entry, <c>tcpout</c>.</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the settings.</returns>
	[Get("services/data/outputs/tcp/default")]
	Task<SplunkFeed<TcpOutputDefaults>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Configures the global forwarding settings (<c>POST data/outputs/tcp/default</c>, with <c>name=tcpout</c>).</summary>
	/// <param name="request">The settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the settings.</returns>
	[Post("services/data/outputs/tcp/default")]
	Task<SplunkFeed<TcpOutputDefaults>> CreateAsync([Body] TcpOutputDefaultsCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets the global forwarding settings (<c>GET data/outputs/tcp/default/{name}</c>).</summary>
	/// <param name="name">The stanza's name; the only valid value is <c>tcpout</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the settings.</returns>
	[Get("services/data/outputs/tcp/default/{name}")]
	Task<SplunkFeed<TcpOutputDefaults>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes the global forwarding settings (<c>POST data/outputs/tcp/default/{name}</c>).</summary>
	/// <param name="name">The stanza's name, <c>tcpout</c>.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the settings.</returns>
	[Post("services/data/outputs/tcp/default/{name}")]
	Task<SplunkFeed<TcpOutputDefaults>> UpdateAsync(string name, [Body] TcpOutputDefaultsUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Disables the global forwarding settings (<c>DELETE data/outputs/tcp/default/{name}</c>); the entry remains.</summary>
	/// <param name="name">The stanza's name, <c>tcpout</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the settings are disabled.</returns>
	[Delete("services/data/outputs/tcp/default/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
