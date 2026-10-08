using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>
/// The directory service (<c>directory</c>): every user-configurable object an app provides (views, navigation,
/// saved searches, event types, tags, field extractions, lookups, workflow actions, field aliases and more).
/// </summary>
/// <remarks>Use a namespace (<see cref="SplunkClient.InNamespace(string, string)"/>) to see what one app provides.</remarks>
public interface IDirectoryEntries
{
	/// <summary>Lists app-scoped objects (<c>GET directory</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per object; each entry's links point at the object's own endpoint.</returns>
	[Get("services/directory")]
	Task<SplunkFeed<DirectoryEntry>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one directory entry (<c>GET directory/{name}</c>).</summary>
	/// <param name="name">The object name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/directory/{name}")]
	Task<SplunkFeed<DirectoryEntry>> GetAsync(string name, CancellationToken cancellationToken);
}
