namespace Splunk.Api.Models.Knowledge;

/// <summary>
/// A search-time field extraction in <c>props.conf</c> (<c>data/props/extractions</c>): an inline <c>EXTRACT-</c> regular
/// expression, or a <c>REPORT-</c> list of <c>transforms.conf</c> stanzas.
/// </summary>
/// <remarks>
/// <see cref="PropsEntry.Type"/> reads <c>Inline</c> for <c>EXTRACT-</c> and <c>Uses transform</c> for <c>REPORT-</c>
/// extractions; <see cref="PropsEntry.Value"/> is the regular expression or the transform names.
/// </remarks>
public sealed class FieldExtraction : PropsEntry;
