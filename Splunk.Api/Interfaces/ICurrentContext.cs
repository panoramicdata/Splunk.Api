using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>The authenticated user (<c>authentication/current-context</c>).</summary>
public interface ICurrentContext
{
	/// <summary>Gets the authenticated user's name, roles and capabilities (<c>GET authentication/current-context</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>context</c>.</returns>
	[Get("services/authentication/current-context")]
	Task<SplunkFeed<CurrentContext>> GetAsync(CancellationToken cancellationToken);
}
