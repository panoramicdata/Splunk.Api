using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Licensing;

namespace Splunk.Api.Interfaces;

/// <summary>Licenser messages, alerts and persisted warnings for this node (<c>licenser/messages</c>).</summary>
public interface ILicenseMessages
{
	/// <summary>Lists the licenser messages (<c>GET licenser/messages</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The messages, named by message ID.</returns>
	[Get("services/licenser/messages")]
	Task<SplunkFeed<LicenseMessage>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one licenser message (<c>GET licenser/messages/{name}</c>).</summary>
	/// <param name="name">The message ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one message.</returns>
	[Get("services/licenser/messages/{name}")]
	Task<SplunkFeed<LicenseMessage>> GetAsync(string name, CancellationToken cancellationToken);
}
