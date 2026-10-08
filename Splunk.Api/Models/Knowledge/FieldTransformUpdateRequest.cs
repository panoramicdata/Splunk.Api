namespace Splunk.Api.Models.Knowledge;

/// <summary>Replaces a field transformation's settings (<c>POST data/transforms/extractions/{name}</c>).</summary>
/// <remarks>
/// Splunk rewrites the stanza from the fields sent: a setting left out returns to its default (for example
/// <c>MV_ADD</c> to false), so send every setting the transformation should keep.
/// </remarks>
public sealed class FieldTransformUpdateRequest : FieldTransformSettings;
