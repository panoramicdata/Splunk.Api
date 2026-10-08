using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a boolean from any form Splunk uses: <c>true</c>/<c>false</c>, a number (0 is false), or a string such as
/// <c>"1"</c>, <c>"0"</c>, <c>"true"</c>, <c>"yes"</c>, <c>"on"</c>, <c>"t"</c> (case-insensitive). An empty string is
/// <see langword="false"/>, as is JSON <c>null</c>. Writes <c>true</c>/<c>false</c>.
/// </summary>
internal sealed class TolerantBooleanConverter : JsonConverter<bool>
{
	/// <inheritdoc />
	public override bool HandleNull => true;

	/// <inheritdoc />
	public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> TolerantReader.ReadBoolean(ref reader) ?? false;

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
		=> writer.WriteBooleanValue(value);
}
