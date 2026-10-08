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
}
