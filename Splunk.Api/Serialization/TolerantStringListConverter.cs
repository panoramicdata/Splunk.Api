using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a list of strings from a JSON array, or from a single string (Splunk returns a multi-valued setting as an array
/// when it holds several values and as a plain string when it holds one). An empty string and JSON <c>null</c> read as an
/// empty list; array items that are numbers or booleans are read as their text. Writes a JSON array. Apply with
/// <c>[JsonConverter(typeof(TolerantStringListConverter))]</c>.
/// </summary>
internal sealed class TolerantStringListConverter : JsonConverter<IReadOnlyList<string>>
{
	private static readonly TolerantStringConverter Item = new();

	/// <inheritdoc />
	public override bool HandleNull => true;

	/// <inheritdoc />
	public override IReadOnlyList<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.StartArray)
		{
			var single = Item.Read(ref reader, typeof(string), options);
			return string.IsNullOrEmpty(single) ? [] : [single];
		}

		var items = new List<string>();
		for (reader.Read(); reader.TokenType != JsonTokenType.EndArray; reader.Read())
		{
			if (Item.Read(ref reader, typeof(string), options) is { } item)
			{
				items.Add(item);
			}
		}

		return items;
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, IReadOnlyList<string>? value, JsonSerializerOptions options)
	{
		if (value is null)
		{
			writer.WriteNullValue();
			return;
		}

		writer.WriteStartArray();
		foreach (var item in value)
		{
			writer.WriteStringValue(item);
		}

		writer.WriteEndArray();
	}
}
