namespace Splunk.Api.Models.Knowledge;

/// <summary>Changes an event type (<c>POST saved/eventtypes/{name}</c>).</summary>
/// <remarks>Splunk requires the search on every update, even when only another setting changes; settings left out keep their values.</remarks>
public sealed class EventTypeUpdateRequest : EventTypeSettings;
