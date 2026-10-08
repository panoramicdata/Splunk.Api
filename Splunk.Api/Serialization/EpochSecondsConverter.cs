using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a nullable <see cref="DateTimeOffset"/> from Unix epoch seconds (a number or numeric string, fractions kept to
/// the millisecond), or from an ISO 8601 string. JSON <c>null</c>, an empty string and 0 read as <see langword="null"/>;
/// a value outside the range of <see cref="DateTimeOffset"/> (or NaN) raises a <see cref="JsonException"/>.
/// Writes epoch seconds. Apply with <c>[JsonConverter(typeof(EpochSecondsConverter))]</c>.
/// </summary>
internal sealed class EpochSecondsConverter : JsonConverter<DateTimeOffset?>
{
	// The Unix milliseconds of DateTimeOffset.MinValue and MaxValue.
	private const double MinMilliseconds = -62_135_596_800_000;
	private const double MaxMilliseconds = 253_402_300_799_999;

	/// <inheritdoc />
	public override bool HandleNull => true;

	/// <inheritdoc />
	public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.String && TryParseIso(reader.GetString()!, out var parsed))
		{
			return parsed;
		}

		if (TolerantReader.ReadNumberText(ref reader) is not { } number)
		{
			return null;
		}

		var seconds = TolerantReader.ParseDouble(number);
		if (seconds == 0)
		{
			return null;
		}

		// Checked here: a NaN would cast to 0 (1970), and an infinity or a huge value would make FromUnixTimeMilliseconds
		// throw ArgumentOutOfRangeException rather than a JsonException naming the property.
		var milliseconds = Math.Round(seconds * 1000);
		return milliseconds is >= MinMilliseconds and <= MaxMilliseconds
			? DateTimeOffset.FromUnixTimeMilliseconds((long)milliseconds)
			: throw new JsonException($"\"{number}\" is not a valid epoch time in seconds.");
	}

	/// <summary>Parses date text, but not numeric text, which DateTimeOffset would also accept (<c>"1.5"</c> as 5 January).</summary>
	private static bool TryParseIso(string text, out DateTimeOffset parsed)
	{
		parsed = default;
		return !double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out _)
			&& DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out parsed);
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
	{
		if (value is { } moment)
		{
			writer.WriteNumberValue(moment.ToUnixTimeMilliseconds() / 1000.0);
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
