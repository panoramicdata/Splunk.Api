using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a string from a JSON string, or the text of a number or boolean (Splunk is inconsistent about quoting). An
/// object or array is read as its raw JSON text, so an unexpected shape does not fail the whole response.
/// </summary>
internal sealed class TolerantStringConverter : JsonConverter<string>
{
	/// <inheritdoc />
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		switch (reader.TokenType)
		{
			case JsonTokenType.String:
				return reader.GetString();
			case JsonTokenType.Number:
				return TolerantReader.ReadNumberText(ref reader);
			case JsonTokenType.True:
				return "true";
			case JsonTokenType.False:
				return "false";
			case JsonTokenType.Null:
				return null;
			default:
				using (var document = JsonDocument.ParseValue(ref reader))
				{
					return document.RootElement.GetRawText();
				}
		}
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value);
}
