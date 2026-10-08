using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>File and directory monitor inputs (<c>data/inputs/monitor</c>). The name of each is the monitored path.</summary>
public interface IMonitorInputs
{
	/// <summary>Lists the monitor inputs (<c>GET data/inputs/monitor</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per input.</returns>
	[Get("services/data/inputs/monitor")]
	Task<SplunkFeed<MonitorInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a monitor input (<c>POST data/inputs/monitor</c>). The path need not exist unless <c>check-path</c> is set.</summary>
	/// <param name="request">The input.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/monitor")]
	Task<SplunkFeed<MonitorInput>> CreateAsync([Body] MonitorInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a monitor input (<c>GET data/inputs/monitor/{name}</c>).</summary>
	/// <param name="name">The monitored path, exactly as listed (for example <c>$SPLUNK_HOME/var/log/splunk</c>).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/monitor/{name}")]
	Task<SplunkFeed<MonitorInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a monitor input (<c>POST data/inputs/monitor/{name}</c>).</summary>
	/// <param name="name">The monitored path.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Post("services/data/inputs/monitor/{name}")]
	Task<SplunkFeed<MonitorInput>> UpdateAsync(string name, [Body] MonitorInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a monitor input (<c>DELETE data/inputs/monitor/{name}</c>).</summary>
	/// <param name="name">The monitored path.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the input is deleted.</returns>
	[Delete("services/data/inputs/monitor/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Lists the files a monitor input is reading (<c>GET data/inputs/monitor/{name}/members</c>).</summary>
	/// <param name="name">The monitored path.</param>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per file, named by its full path; the entries have no other properties.</returns>
	[Get("services/data/inputs/monitor/{name}/members")]
	Task<SplunkFeed<SplunkDynamicContent>> ListMembersAsync(string name, [Query] ListOptions? options, CancellationToken cancellationToken);
}
