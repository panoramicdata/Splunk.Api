using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Event types (<c>saved/eventtypes</c>).</summary>
public interface IEventTypes
{
	/// <summary>Lists event types (<c>GET saved/eventtypes</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per event type.</returns>
	[Get("services/saved/eventtypes")]
	Task<SplunkFeed<EventType>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates an event type (<c>POST saved/eventtypes</c>).</summary>
	/// <param name="request">The name, search and settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new event type.</returns>
	[Post("services/saved/eventtypes")]
	Task<SplunkFeed<EventType>> CreateAsync([Body] EventTypeCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one event type (<c>GET saved/eventtypes/{name}</c>).</summary>
	/// <param name="name">The event type name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/saved/eventtypes/{name}")]
	Task<SplunkFeed<EventType>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes an event type (<c>POST saved/eventtypes/{name}</c>).</summary>
	/// <param name="name">The event type name.</param>
	/// <param name="request">The search, which Splunk requires on every update, and the settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated event type.</returns>
	[Post("services/saved/eventtypes/{name}")]
	Task<SplunkFeed<EventType>> UpdateAsync(string name, [Body] EventTypeUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes an event type (<c>DELETE saved/eventtypes/{name}</c>).</summary>
	/// <param name="name">The event type name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the event type is deleted.</returns>
	[Delete("services/saved/eventtypes/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
