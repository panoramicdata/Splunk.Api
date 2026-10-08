using Splunk.Api.Models.Search;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a <see cref="SearchResult"/> from a JSON object whose values are strings or, for multivalue fields, arrays of
/// strings. Numbers and booleans are read as their text, <c>null</c> as no values, and any other shape as its raw JSON.
/// Writes a single value as a string and several as an array.
/// </summary>
internal sealed class SearchResultConverter : JsonConverter<SearchResult>
{
	/// <inheritdoc />
	public override SearchResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException($"Expected a search result object, not a JSON {reader.TokenType}.");
		}

		using var document = JsonDocument.ParseValue(ref reader);
		var values = new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal);
		foreach (var property in document.RootElement.EnumerateObject())
		{
			values[property.Name] = ReadValues(property.Value);
		}

		return new SearchResult(values);
	}

	private static List<string> ReadValues(JsonElement value)
		=> value.ValueKind switch
		{
			JsonValueKind.Null => [],
			JsonValueKind.Array => [.. value.EnumerateArray().Where(item => item.ValueKind != JsonValueKind.Null).Select(ReadScalar)],
			_ => [ReadScalar(value)]
		};

	private static string ReadScalar(JsonElement value)
		=> value.ValueKind == JsonValueKind.String ? value.GetString()! : value.GetRawText();

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, SearchResult value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		foreach (var (field, values) in value.Values)
		{
			if (values.Count == 1)
			{
				writer.WriteString(field, values[0]);
				continue;
			}

			writer.WriteStartArray(field);
			foreach (var item in values)
			{
				writer.WriteStringValue(item);
			}

			writer.WriteEndArray();
		}

		writer.WriteEndObject();
	}
}
