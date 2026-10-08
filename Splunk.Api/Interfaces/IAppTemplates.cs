using Refit;
using Splunk.Api.Models;

namespace Splunk.Api.Interfaces;

/// <summary>
/// App templates (<c>apps/apptemplates</c>), the starting points for <see cref="IApps.CreateAsync"/>. Splunk installs
/// <c>barebones</c> and <c>sample_app</c>.
/// </summary>
public interface IAppTemplates
{
	/// <summary>Lists the app templates (<c>GET apps/apptemplates</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The templates, named by template; the reference documents no properties.</returns>
	[Get("services/apps/apptemplates")]
	Task<SplunkFeed<SplunkDynamicContent>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one app template (<c>GET apps/apptemplates/{name}</c>).</summary>
	/// <param name="name">The template name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one template.</returns>
	[Get("services/apps/apptemplates/{name}")]
	Task<SplunkFeed<SplunkDynamicContent>> GetAsync(string name, CancellationToken cancellationToken);
}
