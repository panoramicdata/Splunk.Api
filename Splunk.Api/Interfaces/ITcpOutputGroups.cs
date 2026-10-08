using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Outputs;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Forwarding target groups (<c>data/outputs/tcp/group</c>). Reading requires the <c>list_forwarders</c> capability,
/// changing them <c>edit_forwarders</c>.
/// </summary>
public interface ITcpOutputGroups
{
	/// <summary>Lists the target groups (<c>GET data/outputs/tcp/group</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per group.</returns>
	[Get("services/data/outputs/tcp/group")]
	Task<SplunkFeed<TcpOutputGroup>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Creates a target group (<c>POST data/outputs/tcp/group</c>). When no default group is set, Splunk makes this one
	/// the default and starts forwarding to it.
	/// </summary>
	/// <param name="request">The group.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the group.</returns>
	[Post("services/data/outputs/tcp/group")]
	Task<SplunkFeed<TcpOutputGroup>> CreateAsync([Body] TcpOutputGroupCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a target group (<c>GET data/outputs/tcp/group/{name}</c>).</summary>
	/// <param name="name">The group's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the group.</returns>
	[Get("services/data/outputs/tcp/group/{name}")]
	Task<SplunkFeed<TcpOutputGroup>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a target group (<c>POST data/outputs/tcp/group/{name}</c>).</summary>
	/// <param name="name">The group's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the group.</returns>
	[Post("services/data/outputs/tcp/group/{name}")]
	Task<SplunkFeed<TcpOutputGroup>> UpdateAsync(string name, [Body] TcpOutputGroupUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a target group (<c>DELETE data/outputs/tcp/group/{name}</c>); Splunk clears it as the default group.</summary>
	/// <param name="name">The group's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the group is deleted.</returns>
	[Delete("services/data/outputs/tcp/group/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
