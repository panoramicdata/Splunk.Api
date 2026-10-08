using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Stored credentials (<c>storage/passwords</c>), the encrypted secrets apps keep in <c>passwords.conf</c>. Reading
/// requires <c>list_storage_passwords</c>; changing requires <c>edit_storage_passwords</c>. Credentials belong to an
/// app, so use <see cref="SplunkClient.InNamespace(string, string)"/> (typically <c>nobody</c> and the app) to choose
/// where they are written.
/// </summary>
/// <remarks>
/// Reads return each password in <b>clear text</b> (<see cref="StoredPassword.ClearPassword"/>). Never log or persist
/// the responses.
/// </remarks>
public interface IStoragePasswords
{
	/// <summary>Lists the stored credentials (<c>GET storage/passwords</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The credentials, <b>including each password in clear text</b>.</returns>
	[Get("services/storage/passwords")]
	Task<SplunkFeed<StoredPassword>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Stores a credential (<c>POST storage/passwords</c>).</summary>
	/// <param name="request">The user name, password and realm.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The stored credential, named <c>{realm}:{username}:</c> (without the clear-text password).</returns>
	[Post("services/storage/passwords")]
	Task<SplunkFeed<StoredPassword>> CreateAsync([Body] StoredPasswordCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one stored credential (<c>GET storage/passwords/{name}</c>).</summary>
	/// <param name="name">
	/// The credential's name, <c>{realm}:{username}:</c> (<c>:{username}:</c> without a realm). Colons in the realm or user
	/// name are escaped with a backslash.
	/// </param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one credential, <b>including the password in clear text</b>.</returns>
	[Get("services/storage/passwords/{name}")]
	Task<SplunkFeed<StoredPassword>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a stored credential's password (<c>POST storage/passwords/{name}</c>).</summary>
	/// <param name="name">The credential's name, <c>{realm}:{username}:</c>.</param>
	/// <param name="request">The new password.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated credential (without the clear-text password).</returns>
	[Post("services/storage/passwords/{name}")]
	Task<SplunkFeed<StoredPassword>> UpdateAsync(string name, [Body] StoredPasswordUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a stored credential (<c>DELETE storage/passwords/{name}</c>).</summary>
	/// <param name="name">The credential's name, <c>{realm}:{username}:</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the credential is deleted.</returns>
	[Delete("services/storage/passwords/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
