using Splunk.Api.Serialization;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// One search result or event: a map of field names to values. A field has one value, or several for a multivalue
/// field (Splunk sends those as a JSON array). Read a single value with the indexer or <see cref="GetString"/>, and every
/// value with <see cref="GetValues"/>.
/// </summary>
[JsonConverter(typeof(SearchResultConverter))]
public sealed class SearchResult
{
	/// <summary>Creates a result from its field values.</summary>
	/// <param name="values">Each field's values, in field order.</param>
	public SearchResult(IReadOnlyDictionary<string, IReadOnlyList<string>> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		Values = values;
	}

	/// <summary>Every field and its values, in the order Splunk returned them.</summary>
	public IReadOnlyDictionary<string, IReadOnlyList<string>> Values { get; }

	/// <summary>The field names, in the order Splunk returned them.</summary>
	public IEnumerable<string> FieldNames => Values.Keys;

	/// <summary>A field's value (the first value of a multivalue field), or <see langword="null"/> when the field is absent.</summary>
	/// <param name="field">The field name, for example <c>host</c>.</param>
	public string? this[string field] => GetString(field);

	/// <summary>The event text (<c>_raw</c>), or <see langword="null"/> for a result without it.</summary>
	public string? Raw => GetString("_raw");

	/// <summary>
	/// The event time (<c>_time</c>), or <see langword="null"/> when absent or not a recognisable time. Splunk sends an
	/// ISO 8601 time in results (<c>2026-10-08T13:47:15.000+00:00</c>) and a <c>GMT</c>-suffixed one in exports.
	/// </summary>
	public DateTimeOffset? Time => ParseTime(GetString("_time"));

	/// <summary>Whether the result has the field.</summary>
	/// <param name="field">The field name.</param>
	/// <returns><see langword="true"/> when the field is present.</returns>
	public bool Contains(string field) => Values.ContainsKey(field);

	/// <summary>A field's value: the only value, or the first value of a multivalue field.</summary>
	/// <param name="field">The field name.</param>
	/// <returns>The value, or <see langword="null"/> when the field is absent or has no values.</returns>
	public string? GetString(string field) => Values.TryGetValue(field, out var values) && values.Count > 0 ? values[0] : null;

	/// <summary>Every value of a field.</summary>
	/// <param name="field">The field name.</param>
	/// <returns>The values in order; empty when the field is absent.</returns>
	public IReadOnlyList<string> GetValues(string field) => Values.TryGetValue(field, out var values) ? values : [];

	/// <summary>Whether a field has more than one value.</summary>
	/// <param name="field">The field name.</param>
	/// <returns><see langword="true"/> for a multivalue field.</returns>
	public bool IsMultivalue(string field) => GetValues(field).Count > 1;

	private static DateTimeOffset? ParseTime(string? text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}

		var normalized = text.EndsWith(" GMT", StringComparison.Ordinal) ? text[..^4] + "Z" : text;
		return DateTimeOffset.TryParse(normalized, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var time) ? time : null;
	}
}
