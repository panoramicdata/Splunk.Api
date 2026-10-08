using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads enums case-insensitively by their wire name (see <see cref="WireNames"/>), mapping unrecognised names to the
/// enum's default value (by convention an <c>Unknown = 0</c> member). Writes the wire name.
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
				JsonTokenType.Number when reader.TryGetInt32(out var number) && Enum.IsDefined(typeof(T), number) => (T)Enum.ToObject(typeof(T), number),
				JsonTokenType.Number => default,
				_ => throw new JsonException("Expected an enum name string.")
			};

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
			=> writer.WriteStringValue(WireNames.Of(value));
	}
}
