using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Windows event log collections (<c>data/inputs/win-event-log-collections</c>). Only Splunk on Windows has them; elsewhere every call answers 404.</summary>
public interface IWindowsEventLogInputs
{
	/// <summary>Lists the event log collections (<c>GET data/inputs/win-event-log-collections</c>).</summary>
	/// <param name="options">Paging, filtering and <c>lookup_host</c>, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per collection.</returns>
	[Get("services/data/inputs/win-event-log-collections")]
	Task<SplunkFeed<WindowsEventLogInput>> ListAsync([Query] WindowsEventLogInputListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates an event log collection (<c>POST data/inputs/win-event-log-collections</c>).</summary>
	/// <param name="request">The collection.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the collection.</returns>
	[Post("services/data/inputs/win-event-log-collections")]
	Task<SplunkFeed<WindowsEventLogInput>> CreateAsync([Body] WindowsEventLogInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets an event log collection (<c>GET data/inputs/win-event-log-collections/{name}</c>).</summary>
	/// <param name="name">The collection's name.</param>
	/// <param name="options">The <c>lookup_host</c> option, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the collection.</returns>
	[Get("services/data/inputs/win-event-log-collections/{name}")]
	Task<SplunkFeed<WindowsEventLogInput>> GetAsync(string name, [Query] WindowsEventLogInputOptions? options, CancellationToken cancellationToken);

	/// <summary>Changes an event log collection (<c>POST data/inputs/win-event-log-collections/{name}</c>).</summary>
	/// <param name="name">The collection's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the collection.</returns>
	[Post("services/data/inputs/win-event-log-collections/{name}")]
	Task<SplunkFeed<WindowsEventLogInput>> UpdateAsync(string name, [Body] WindowsEventLogInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes an event log collection (<c>DELETE data/inputs/win-event-log-collections/{name}</c>).</summary>
	/// <param name="name">The collection's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the collection is deleted.</returns>
	[Delete("services/data/inputs/win-event-log-collections/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
