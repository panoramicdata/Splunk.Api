using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Server;

namespace Splunk.Api.Interfaces;

/// <summary>System messages, as shown in Splunk Web's Messages menu (<c>messages</c>).</summary>
public interface IMessages
{
	/// <summary>Lists the system messages (<c>GET messages</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per message.</returns>
	[Get("services/messages")]
	Task<SplunkFeed<ServerMessage>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a persistent message (<c>POST messages</c>).</summary>
	/// <param name="request">The message.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new message.</returns>
	[Post("services/messages")]
	Task<SplunkFeed<ServerMessage>> CreateAsync([Body] MessageCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one message (<c>GET messages/{name}</c>).</summary>
	/// <param name="name">The message identifier.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the message.</returns>
	[Get("services/messages/{name}")]
	Task<SplunkFeed<ServerMessage>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Deletes a message (<c>DELETE messages/{name}</c>).</summary>
	/// <param name="name">The message identifier.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the message is deleted.</returns>
	/// <remarks>Splunk 10.6 answers <c>404</c> for a message that does not exist (the reference says <c>500</c>).</remarks>
	[Delete("services/messages/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
