using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads enums case-insensitively by their wire name (see <see cref="WireNames"/>), mapping unrecognised names to the
/// enum's default value (by convention an <c>Unknown = 0</c> member). A JSON number reads as the member with that value, or
/// the default when no member has it; JSON <c>null</c> reads as the default. Writes the wire name.
/// </summary>
internal sealed class TolerantEnumConverterFactory : JsonConverterFactory
{
	/// <inheritdoc />
	public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

	/// <inheritdoc />
	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		=> (JsonConverter)Activator.CreateInstance(typeof(TolerantEnumConverter<>).MakeGenericType(typeToConvert))!;

	private sealed class TolerantEnumConverter<T> : JsonConverter<T> where T : struct, Enum
	{
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

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
			=> writer.WriteStringValue(WireNames.Of(value));
	}
}
