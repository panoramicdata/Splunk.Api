using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a list of Unix epoch seconds (numbers or numeric strings) as <see cref="DateTimeOffset"/> values; JSON
/// <c>null</c> reads as an empty list. Writes epoch seconds. Apply with
/// <c>[JsonConverter(typeof(EpochSecondsListConverter))]</c>.
/// </summary>
internal sealed class EpochSecondsListConverter : JsonConverter<IReadOnlyList<DateTimeOffset>>
{
	/// <inheritdoc />
	public override bool HandleNull => true;

	/// <inheritdoc />
	public override IReadOnlyList<DateTimeOffset> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return [];
		}

		if (reader.TokenType != JsonTokenType.StartArray)
		{
			throw new JsonException($"Expected an array of epoch seconds, not a JSON {reader.TokenType}.");
		}

		var times = new List<DateTimeOffset>();
		while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
		{
			if (TolerantReader.ReadNumberText(ref reader) is { } text)
			{
				times.Add(DateTimeOffset.FromUnixTimeMilliseconds((long)Math.Round(TolerantReader.ParseDouble(text) * 1000)));
			}
		}

		return times;
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, IReadOnlyList<DateTimeOffset> value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();
		foreach (var time in value)
		{
			writer.WriteNumberValue(time.ToUnixTimeMilliseconds() / 1000.0);
		}

		writer.WriteEndArray();
	}
}
