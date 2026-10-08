using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>The converter <see cref="TolerantEnumConverterFactory"/> creates for one enum type.</summary>
/// <typeparam name="T">The enum type.</typeparam>
internal sealed class TolerantEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
	/// <inheritdoc />
	public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> reader.TokenType switch
		{
			JsonTokenType.String => WireNames.Parse<T>(reader.GetString()!),
			JsonTokenType.Number => reader.TryGetInt64(out var number) ? FromNumber(number) : default,
			JsonTokenType.Null => default,
			_ => throw new JsonException($"Cannot read {typeof(T).Name} from a JSON {reader.TokenType}.")
		};

	// A checked conversion to the underlying type first, so a byte- or long-backed enum works and an out-of-range number
	// is not truncated into a defined member.
	private static T FromNumber(long number)
	{
		try
		{
			var underlying = Convert.ChangeType(number, Enum.GetUnderlyingType(typeof(T)), CultureInfo.InvariantCulture);
			var value = (T)Enum.ToObject(typeof(T), underlying);
			return Enum.IsDefined(value) ? value : default;
		}
		catch (OverflowException)
		{
			return default;
		}
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		=> writer.WriteStringValue(WireNames.Of(value));
}
