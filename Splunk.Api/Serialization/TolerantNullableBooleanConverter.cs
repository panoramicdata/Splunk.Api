using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a nullable boolean as <see cref="TolerantBooleanConverter"/> does, except that JSON <c>null</c> and an empty
/// string are <see langword="null"/>.
/// </summary>
internal sealed class TolerantNullableBooleanConverter : JsonConverter<bool?>
{
	/// <inheritdoc />
	public override bool HandleNull => true;

	/// <inheritdoc />
	public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> TolerantReader.ReadBoolean(ref reader);

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
	{
		if (value is { } flag)
		{
			writer.WriteBooleanValue(flag);
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
