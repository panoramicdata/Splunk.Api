namespace Splunk.Api.Models.Knowledge;

/// <summary>Replaces an automatic lookup's settings (<c>POST data/props/lookups/{name}</c>).</summary>
/// <remarks>Splunk rewrites the whole lookup from the fields sent: input and output fields left out are removed.</remarks>
public sealed class AutomaticLookupUpdateRequest : AutomaticLookupSettings;
