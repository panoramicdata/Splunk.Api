namespace Splunk.Api.Models.Knowledge;

/// <summary>Replaces a lookup definition's settings (<c>POST data/transforms/lookups/{name}</c>).</summary>
/// <remarks>
/// Splunk rewrites the stanza from the fields sent: settings left out are removed. Always re-send the lookup's
/// <see cref="LookupDefinitionSettings.FileName"/>, <see cref="LookupDefinitionSettings.ExternalCommand"/> or
/// <see cref="LookupDefinitionSettings.Collection"/>; without it the definition becomes unusable and Splunk answers 404.
/// </remarks>
public sealed class LookupDefinitionUpdateRequest : LookupDefinitionSettings;
