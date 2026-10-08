using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>The SSL settings shared by TCP inputs (<c>data/inputs/tcp/ssl</c>).</summary>
public interface ITcpSslSettings
{
	/// <summary>Gets the SSL settings (<c>GET data/inputs/tcp/ssl</c>); there is one entry, named <c>""</c>.</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the settings.</returns>
	[Get("services/data/inputs/tcp/ssl")]
	Task<SplunkFeed<TcpSslSettings>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets the SSL settings for a host (<c>GET data/inputs/tcp/ssl/{name}</c>).</summary>
	/// <param name="name">The host name; Splunk 10.6 answers any name with its single settings entry.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the settings.</returns>
	[Get("services/data/inputs/tcp/ssl/{name}")]
	Task<SplunkFeed<TcpSslSettings>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes the SSL settings (<c>POST data/inputs/tcp/ssl/{name}</c>).</summary>
	/// <param name="name">The host name, for example <c>ssl</c>.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed, empty in Splunk 10.6.</returns>
	[Post("services/data/inputs/tcp/ssl/{name}")]
	Task<SplunkFeed<TcpSslSettings>> UpdateAsync(string name, [Body] TcpSslSettingsUpdateRequest request, CancellationToken cancellationToken);
}
