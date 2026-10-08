using System.Globalization;
using System.Text.Json;

namespace Splunk.Api.Serialization;

/// <summary>The shared reading rules behind the tolerant converters.</summary>
internal static class TolerantReader
{
	private static readonly HashSet<string> TrueWords = new(StringComparer.OrdinalIgnoreCase) { "1", "true", "t", "yes", "y", "on" };
	private static readonly HashSet<string> FalseWords = new(StringComparer.OrdinalIgnoreCase) { "0", "false", "f", "no", "n", "off" };

	/// <summary>Reads a boolean, or <see langword="null"/> for JSON null or an empty string.</summary>
	public static bool? ReadBoolean(ref Utf8JsonReader reader)
		=> reader.TokenType switch
		{
			JsonTokenType.True => true,
			JsonTokenType.False => false,
			JsonTokenType.Null => null,
			JsonTokenType.Number => reader.GetDouble() != 0,
			JsonTokenType.String => ParseBoolean(reader.GetString()!),
			_ => throw new JsonException($"Cannot read a boolean from a JSON {reader.TokenType}.")
		};

	private static bool? ParseBoolean(string text)
	{
		var trimmed = text.Trim();
		if (trimmed.Length == 0)
		{
			return null;
		}

		if (TrueWords.Contains(trimmed))
		{
			return true;
		}

		if (FalseWords.Contains(trimmed))
		{
			return false;
		}

		return double.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out var number)
			? number != 0
			: throw new JsonException($"Cannot read a boolean from \"{text}\".");
	}

	/// <summary>
	/// Reads the text of a number: the raw token of a JSON number, or a trimmed string. Returns <see langword="null"/>
	/// for JSON null or an empty string.
	/// </summary>
	public static string? ReadNumberText(ref Utf8JsonReader reader)
		=> reader.TokenType switch
		{
			JsonTokenType.Null => null,
			JsonTokenType.Number => reader.HasValueSequence ? System.Text.EncodingExtensions.GetString(System.Text.Encoding.UTF8, reader.ValueSequence) : System.Text.Encoding.UTF8.GetString(reader.ValueSpan),
			JsonTokenType.String => reader.GetString()!.Trim() is { Length: > 0 } text ? text : null,
			JsonTokenType.True => "1",
			JsonTokenType.False => "0",
			_ => throw new JsonException($"Cannot read a number from a JSON {reader.TokenType}.")
		};

	/// <summary>Parses number text as an <see cref="long"/>, accepting a whole-valued decimal such as <c>5.0</c>.</summary>
	public static long ParseInt64(string text)
	{
		if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var whole))
		{
			return whole;
		}

		if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var number)
			&& number == Math.Floor(number)
			&& number >= long.MinValue
			&& number <= long.MaxValue)
		{
			return (long)number;
		}

		throw new JsonException($"Cannot read an integer from \"{text}\".");
	}

	/// <summary>Parses number text as an <see cref="int"/>.</summary>
	public static int ParseInt32(string text)
	{
		var value = ParseInt64(text);
		return value is >= int.MinValue and <= int.MaxValue
			? (int)value
			: throw new JsonException($"\"{text}\" is out of range for a 32-bit integer.");
	}

	/// <summary>Parses number text as a <see cref="double"/>, accepting <c>NaN</c> and infinities.</summary>
	public static double ParseDouble(string text)
		=> double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var number)
			? number
			: throw new JsonException($"Cannot read a number from \"{text}\".");
}
