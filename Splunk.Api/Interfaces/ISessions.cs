using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>Logging in and the active sessions (<c>auth/login</c> and <c>authentication/httpauth-tokens</c>).</summary>
public interface ISessions
{
	/// <summary>Logs in and returns a session key (<c>POST auth/login</c>).</summary>
	/// <param name="request">The user name, password and, for RSA multifactor users, the passcode.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The session key: a secret to send as <c>Authorization: Splunk {key}</c>. Never log it.</returns>
	/// <remarks>
	/// <para>
	/// The client logs in by itself when <see cref="SplunkClientOptions"/> holds a user name and password, so call this
	/// only to obtain a session key for another purpose, such as another identity or another tool. The request is sent
	/// without this client's own credentials or session, and is allowed by a read-only client.
	/// </para>
	/// <para>Splunk answers only a <c>sessionKey</c> object, not a feed. Wrong credentials raise 401.</para>
	/// </remarks>
	[Post("services/auth/login")]
	Task<LoginResponse> LoginAsync([Body] LoginRequest request, CancellationToken cancellationToken);

	/// <summary>Lists the active sessions (<c>GET authentication/httpauth-tokens</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The sessions, named by their identifier; Splunk masks each session key.</returns>
	[Get("services/authentication/httpauth-tokens")]
	Task<SplunkFeed<SessionInfo>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one session (<c>GET authentication/httpauth-tokens/{name}</c>).</summary>
	/// <param name="name">The session's name, as listed by <see cref="ListAsync"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one session.</returns>
	/// <remarks>An unknown name is answered with 404 <c>Not a valid session: ...</c>.</remarks>
	[Get("services/authentication/httpauth-tokens/{name}")]
	Task<SplunkFeed<SessionInfo>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Ends a session (<c>DELETE authentication/httpauth-tokens/{name}</c>).</summary>
	/// <param name="name">The session's name, as listed by <see cref="ListAsync"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the session has ended.</returns>
	[Delete("services/authentication/httpauth-tokens/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
