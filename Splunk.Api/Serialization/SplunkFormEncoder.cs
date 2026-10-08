using Splunk.Api.Models;
using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Turns a request object into the ordered form fields Splunk expects. See <see cref="SplunkFormRequest"/> for the rules.
/// </summary>
internal static class SplunkFormEncoder
{
	private static readonly ConcurrentDictionary<Type, FormProperty[]> PropertyCache = new();

	/// <summary>
	/// Encodes <paramref name="body"/>: a <see cref="SplunkFormRequest"/> (or any object) property by property, a
	/// sequence of string key/value pairs as given, or any other dictionary entry by entry (keys and values formatted as
	/// scalars, sequences repeated). A <see langword="null"/> pair or entry value is sent as an empty string.
	/// </summary>
	public static List<KeyValuePair<string, string>> Encode(object body)
	{
		ArgumentNullException.ThrowIfNull(body);
		var fields = new List<KeyValuePair<string, string>>();
		// Nullable annotations are erased: this also matches IEnumerable<KeyValuePair<string, string>>.
		if (body is IEnumerable<KeyValuePair<string, string?>> pairs)
		{
			fields.AddRange(pairs.Select(p => new KeyValuePair<string, string>(p.Key, p.Value ?? string.Empty)));
			return fields;
		}

		// Any other dictionary (such as Dictionary<string, object>) would otherwise be encoded by its own properties
		// (comparer, count, keys, values).
		if (body is IDictionary dictionary)
		{
			foreach (DictionaryEntry entry in dictionary)
			{
				AddValue(fields, FormatScalar(entry.Key), entry.Value ?? string.Empty);
			}

			return fields;
		}

		foreach (var property in PropertyCache.GetOrAdd(body.GetType(), DiscoverProperties))
		{
			AddValue(fields, property.Name, property.Getter(body));
		}

		if (body is SplunkFormRequest request)
		{
			AddAdditional(fields, request.AdditionalParameters);
		}

		return fields;
	}

	private static void AddAdditional(List<KeyValuePair<string, string>> fields, IDictionary<string, string?> additional)
	{
		var typed = fields.Select(f => f.Key).ToHashSet(StringComparer.Ordinal);
		foreach (var (key, value) in additional)
		{
			if (typed.Contains(key))
			{
				throw new ArgumentException($"The additional parameter '{key}' is also set by a typed property; set it in one place only.", nameof(additional));
			}

			fields.Add(new(key, value ?? string.Empty));
		}
	}

	private static void AddValue(List<KeyValuePair<string, string>> fields, string name, object? value)
	{
		switch (value)
		{
			case null:
				return;
			case string text:
				fields.Add(new(name, text));
				return;
			case IEnumerable sequence:
				foreach (var item in sequence)
				{
					if (item is not null)
					{
						fields.Add(new(name, FormatScalar(item)));
					}
				}

				return;
			default:
				fields.Add(new(name, FormatScalar(value)));
				return;
		}
	}

	/// <summary>Formats a single value the way Splunk reads it in a form field or query string.</summary>
	public static string FormatScalar(object value)
		=> value switch
		{
			string text => text,
			bool flag => flag ? "true" : "false",
			Enum member => WireNames.Of(member),
			DateTimeOffset moment => moment.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture),
			DateTime moment => moment.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture),
			TimeSpan span => ((long)span.TotalSeconds).ToString(CultureInfo.InvariantCulture),
			IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
			_ => value.ToString() ?? string.Empty
		};

	private static FormProperty[] DiscoverProperties(Type type)
		=> [.. type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(p => p.GetGetMethod() is not null
				&& p.GetIndexParameters().Length == 0
				&& p.GetCustomAttribute<JsonIgnoreAttribute>() is null
				&& !(p.DeclaringType == typeof(SplunkFormRequest) && p.Name == nameof(SplunkFormRequest.AdditionalParameters)))
			.OrderBy(p => p.MetadataToken)
			.Select(p => new FormProperty(
				p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? JsonNamingPolicy.SnakeCaseLower.ConvertName(p.Name),
				p.GetValue))];

	private sealed record FormProperty(string Name, Func<object?, object?> Getter);
}
