using Splunk.Api.Models;
using Splunk.Api.Serialization;
using Splunk.Api.Test.Support;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Test.Core;

public class EnumAndEpochConverterTests
{
	public enum Colour
	{
		Unknown = 0,

		[JsonStringEnumMemberName("light-red")]
		Red = 1,

		Green = 2,

#pragma warning disable CA1069 // A deliberate alias: WireNames writes the first member's name and reads both.
		[JsonStringEnumMemberName("verde")]
		Verde = 2
#pragma warning restore CA1069
	}

	public enum Small : byte
	{
		Unknown = 0,
		One = 1
	}

	public enum Big : long
	{
		Unknown = 0,
		Huge = 1L << 40
	}

	private static T ReadEnum<T>(string json) where T : struct, Enum
		=> JsonSerializer.Deserialize<T>(json, SplunkJson.Options);

	[Theory]
	[InlineData("\"light-red\"", Colour.Red)]
	[InlineData("\"LIGHT-RED\"", Colour.Red)]
	[InlineData("\"Green\"", Colour.Green)]
	[InlineData("\"verde\"", Colour.Green)]
	[InlineData("\"Red\"", Colour.Unknown)]
	[InlineData("\"purple\"", Colour.Unknown)]
	[InlineData("\"\"", Colour.Unknown)]
	[InlineData("1", Colour.Red)]
	[InlineData("7", Colour.Unknown)]
	[InlineData("1.5", Colour.Unknown)]
	[InlineData("null", Colour.Unknown)]
	public void Enum_ReadsWireNamesAndNumbers(string json, Colour expected)
		=> ReadEnum<Colour>(json).Should().Be(expected);

	[Theory]
	[InlineData("1", Small.One)]
	[InlineData("257", Small.Unknown)]
	[InlineData("-1", Small.Unknown)]
	public void Enum_ReadsNumbersForAByteEnum(string json, Small expected)
		=> ReadEnum<Small>(json).Should().Be(expected);

	[Fact]
	public void Enum_ReadsNumbersForALongEnum()
		=> ReadEnum<Big>("1099511627776").Should().Be(Big.Huge);

	[Fact]
	public void Enum_ReadsNullableEnums()
	{
		JsonSerializer.Deserialize<Colour?>("null", SplunkJson.Options).Should().BeNull();
		JsonSerializer.Deserialize<SortDirection?>("\"desc\"", SplunkJson.Options).Should().Be(SortDirection.Descending);
	}

	[Theory]
	[InlineData("true")]
	[InlineData("{}")]
	public void Enum_OtherTokens_Throw(string json)
	{
		var act = () => ReadEnum<Colour>(json);

		act.Should().Throw<JsonException>().WithMessage("*Cannot read Colour from a JSON*");
	}

	[Theory]
	[InlineData(Colour.Red, "\"light-red\"")]
	[InlineData(Colour.Green, "\"Green\"")]
	[InlineData((Colour)9, "\"9\"")]
	public void Enum_WritesTheWireName(Colour value, string json)
		=> JsonSerializer.Serialize(value, SplunkJson.Options).Should().Be(json);

	[Theory]
	[InlineData("1791465085", 1791465085000L)]
	[InlineData("\"1791465085\"", 1791465085000L)]
	[InlineData("1791465085.1234", 1791465085123L)]
	[InlineData("\"1791465085.5\"", 1791465085500L)]
	[InlineData("-1", -1000L)]
	[InlineData("\"2026-10-08T14:18:12+01:00\"", 1791465492000L)]
	[InlineData("\"2026-10-08T13:18:12\"", 1791465492000L)]
	[InlineData("253402300799.999", 253402300799999L)]
	[InlineData("-62135596800", -62135596800000L)]
	public void Epoch_ReadsSecondsAndIsoText(string json, long unixMilliseconds)
	{
		var value = Json.Read(new EpochSecondsConverter(), json);

		value.Should().Be(DateTimeOffset.FromUnixTimeMilliseconds(unixMilliseconds));
	}

	[Theory]
	[InlineData("null")]
	[InlineData("\"\"")]
	[InlineData("0")]
	[InlineData("\"0.0\"")]
	public void Epoch_NullEmptyOrZero_IsNull(string json)
		=> Json.Read(new EpochSecondsConverter(), json).Should().BeNull();

	[Theory]
	[InlineData("\"NaN\"")]
	[InlineData("\"Infinity\"")]
	[InlineData("1e300")]
	[InlineData("253402300800")]
	[InlineData("-62135596801")]
	[InlineData("\"soon\"")]
	public void Epoch_OutOfRangeOrUnreadable_Throws(string json)
	{
		var act = () => Json.Read(new EpochSecondsConverter(), json);

		act.Should().Throw<JsonException>();
	}

	[Fact]
	public void Epoch_WritesSecondsOrNull()
	{
		Json.Write<DateTimeOffset?>(new EpochSecondsConverter(), DateTimeOffset.FromUnixTimeMilliseconds(1791465085500)).Should().Be("1791465085.5");
		Json.Write<DateTimeOffset?>(new EpochSecondsConverter(), null).Should().Be("null");
	}

	[Fact]
	public void WireNames_UndefinedValuesAreNumbers_AndUnknownNamesTheDefault()
	{
		WireNames.Of((Small)200).Should().Be("200");
		WireNames.Of(SortMode.AlphabeticalCaseSensitive).Should().Be("alpha_case");
		WireNames.Parse<SortMode>("NUM").Should().Be(SortMode.Numeric);
		WireNames.Parse<SortMode>("nope").Should().Be((SortMode)0);
	}
}
