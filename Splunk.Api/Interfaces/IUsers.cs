using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>Users (<c>authentication/users</c>). Changing users requires the <c>edit_user</c> capability.</summary>
public interface IUsers
{
	/// <summary>Lists the users (<c>GET authentication/users</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The users.</returns>
	[Get("services/authentication/users")]
	Task<SplunkFeed<User>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a user (<c>POST authentication/users</c>).</summary>
	/// <param name="request">The user.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created user.</returns>
	[Post("services/authentication/users")]
	Task<SplunkFeed<User>> CreateAsync([Body] UserCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one user (<c>GET authentication/users/{name}</c>).</summary>
	/// <param name="name">The user name. Splunk matches it case-insensitively.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one user.</returns>
	[Get("services/authentication/users/{name}")]
	Task<SplunkFeed<User>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Updates a user (<c>POST authentication/users/{name}</c>).</summary>
	/// <param name="name">The user name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated user.</returns>
	[Post("services/authentication/users/{name}")]
	Task<SplunkFeed<User>> UpdateAsync(string name, [Body] UserUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a user (<c>DELETE authentication/users/{name}</c>).</summary>
	/// <param name="name">The user name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the user is deleted.</returns>
	[Delete("services/authentication/users/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
