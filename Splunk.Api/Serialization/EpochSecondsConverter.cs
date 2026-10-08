using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a nullable <see cref="DateTimeOffset"/> from Unix epoch seconds (a number or numeric string, fractions kept to
/// the millisecond), or from an ISO 8601 string. JSON <c>null</c>, an empty string and 0 read as <see langword="null"/>.
/// Writes epoch seconds. Apply with <c>[JsonConverter(typeof(EpochSecondsConverter))]</c>.
/// </summary>
internal sealed class EpochSecondsConverter : JsonConverter<DateTimeOffset?>
{
	/// <inheritdoc />
	public override bool HandleNull => true;

	/// <inheritdoc />
	public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.String
			&& reader.GetString() is { } text
			&& DateTimeOffset.TryParse(text, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed)
			&& !double.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out _))
		{
			return parsed;
		}

		if (TolerantReader.ReadNumberText(ref reader) is not { } number)
		{
			return null;
		}

		var seconds = TolerantReader.ParseDouble(number);
		return seconds == 0 ? null : DateTimeOffset.FromUnixTimeMilliseconds((long)Math.Round(seconds * 1000));
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
