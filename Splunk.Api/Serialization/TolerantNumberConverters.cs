using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// Reads a number from a JSON number or a numeric string. An empty string or JSON <c>null</c> reads as the default
/// (zero). Writes a JSON number.
/// </summary>
/// <typeparam name="T">The numeric type.</typeparam>
internal abstract class TolerantNumberConverter<T> : JsonConverter<T> where T : struct
{
	private readonly Func<string, T> _parse;
	private readonly Action<Utf8JsonWriter, T> _write;

	protected TolerantNumberConverter(Func<string, T> parse, Action<Utf8JsonWriter, T> write)
	{
		_parse = parse;
		_write = write;
	}

	/// <inheritdoc />
	public override bool HandleNull => true;

	/// <inheritdoc />
	public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> TolerantReader.ReadNumberText(ref reader) is { } text ? _parse(text) : default;

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => _write(writer, value);
}

/// <summary>
/// Reads a nullable number from a JSON number or a numeric string. An empty string or JSON <c>null</c> reads as
/// <see langword="null"/>.
/// </summary>
/// <typeparam name="T">The numeric type.</typeparam>
internal abstract class TolerantNullableNumberConverter<T> : JsonConverter<T?> where T : struct
{
	private readonly Func<string, T> _parse;
	private readonly Action<Utf8JsonWriter, T> _write;

	protected TolerantNullableNumberConverter(Func<string, T> parse, Action<Utf8JsonWriter, T> write)
	{
		_parse = parse;
		_write = write;
	}

	/// <inheritdoc />
	public override bool HandleNull => true;

	/// <inheritdoc />
	public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> TolerantReader.ReadNumberText(ref reader) is { } text ? _parse(text) : null;

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
	{
		if (value is { } number)
		{
			_write(writer, number);
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}

/// <summary>Tolerant <see cref="int"/> reading.</summary>
internal sealed class TolerantInt32Converter() : TolerantNumberConverter<int>(TolerantReader.ParseInt32, static (w, v) => w.WriteNumberValue(v));

/// <summary>Tolerant nullable <see cref="int"/> reading.</summary>
internal sealed class TolerantNullableInt32Converter() : TolerantNullableNumberConverter<int>(TolerantReader.ParseInt32, static (w, v) => w.WriteNumberValue(v));

/// <summary>Tolerant <see cref="long"/> reading.</summary>
internal sealed class TolerantInt64Converter() : TolerantNumberConverter<long>(TolerantReader.ParseInt64, static (w, v) => w.WriteNumberValue(v));

/// <summary>Tolerant nullable <see cref="long"/> reading.</summary>
internal sealed class TolerantNullableInt64Converter() : TolerantNullableNumberConverter<long>(TolerantReader.ParseInt64, static (w, v) => w.WriteNumberValue(v));

/// <summary>Tolerant <see cref="double"/> reading.</summary>
internal sealed class TolerantDoubleConverter() : TolerantNumberConverter<double>(TolerantReader.ParseDouble, WriteDouble)
{
	/// <summary>
	/// Writes a JSON number, or NaN and the infinities as the strings <c>"NaN"</c>, <c>"Infinity"</c> and
	/// <c>"-Infinity"</c> (as <see cref="JsonNumberHandling.AllowNamedFloatingPointLiterals"/> does), which
	/// <see cref="Utf8JsonWriter.WriteNumberValue(double)"/> would reject.
	/// </summary>
	internal static void WriteDouble(Utf8JsonWriter writer, double value)
	{
		if (double.IsFinite(value))
		{
			writer.WriteNumberValue(value);
		}
		else
		{
			writer.WriteStringValue(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
		}
	}
}

/// <summary>Tolerant nullable <see cref="double"/> reading.</summary>
internal sealed class TolerantNullableDoubleConverter() : TolerantNullableNumberConverter<double>(TolerantReader.ParseDouble, TolerantDoubleConverter.WriteDouble);
