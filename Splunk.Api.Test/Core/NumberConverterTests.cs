using Splunk.Api.Serialization;
using Splunk.Api.Test.Support;
using System.Text.Json;

namespace Splunk.Api.Test.Core;

public class NumberConverterTests
{
	[Theory]
	[InlineData("5", 5L)]
	[InlineData("-5", -5L)]
	[InlineData("\" 7 \"", 7L)]
	[InlineData("5.0", 5L)]
	[InlineData("\"1e3\"", 1000L)]
	[InlineData("true", 1L)]
	[InlineData("false", 0L)]
	[InlineData("9223372036854775807", long.MaxValue)]
	[InlineData("-9223372036854775808", long.MinValue)]
	[InlineData("-9.223372036854775808e18", long.MinValue)]
	public void Int64_ReadsNumbersAndNumericText(string json, long expected)
	{
		Json.Read(new TolerantInt64Converter(), json).Should().Be(expected);
		Json.Read(new TolerantNullableInt64Converter(), json).Should().Be(expected);
		Json.Read(new TolerantInt64Converter(), json, segmented: true).Should().Be(expected);
	}

	[Theory]
	[InlineData("5", 5)]
	[InlineData("\"-2147483648\"", int.MinValue)]
	[InlineData("2147483647.0", int.MaxValue)]
	public void Int32_ReadsNumbersAndNumericText(string json, int expected)
	{
		Json.Read(new TolerantInt32Converter(), json).Should().Be(expected);
		Json.Read(new TolerantNullableInt32Converter(), json).Should().Be(expected);
	}

	[Theory]
	[InlineData("1.5", 1.5)]
	[InlineData("\"-2.25\"", -2.25)]
	[InlineData("\"1e-3\"", 0.001)]
	[InlineData("\"NaN\"", double.NaN)]
	[InlineData("\"Infinity\"", double.PositiveInfinity)]
	[InlineData("\"-Infinity\"", double.NegativeInfinity)]
	public void Double_ReadsNumbersAndNamedLiterals(string json, double expected)
	{
		Json.Read(new TolerantDoubleConverter(), json).Should().Be(expected);
		Json.Read(new TolerantNullableDoubleConverter(), json).Should().Be(expected);
	}

	[Theory]
	[InlineData("null")]
	[InlineData("\"\"")]
	[InlineData("\" \"")]
	public void NullOrEmpty_IsZero_OrNullWhenNullable(string json)
	{
		Json.Read(new TolerantInt32Converter(), json).Should().Be(0);
		Json.Read(new TolerantInt64Converter(), json).Should().Be(0);
		Json.Read(new TolerantDoubleConverter(), json).Should().Be(0);
		Json.Read(new TolerantNullableInt32Converter(), json).Should().BeNull();
		Json.Read(new TolerantNullableInt64Converter(), json).Should().BeNull();
		Json.Read(new TolerantNullableDoubleConverter(), json).Should().BeNull();
	}

	[Theory]
	[InlineData("5.5", "Cannot read an integer from \"5.5\".")]
	[InlineData("\"abc\"", "Cannot read an integer from \"abc\".")]
	[InlineData("9223372036854775808", "Cannot read an integer from \"9223372036854775808\".")]
	[InlineData("1e19", "Cannot read an integer from \"1e19\".")]
	[InlineData("-1e19", "Cannot read an integer from \"-1e19\".")]
	[InlineData("\"NaN\"", "Cannot read an integer from \"NaN\".")]
	[InlineData("\"Infinity\"", "Cannot read an integer from \"Infinity\".")]
	[InlineData("{}", "Cannot read a number from a JSON StartObject.")]
	[InlineData("[1]", "Cannot read a number from a JSON StartArray.")]
	public void Int64_Unreadable_Throws(string json, string message)
	{
		var act = () => Json.Read(new TolerantInt64Converter(), json);

		act.Should().Throw<JsonException>().WithMessage(message);
	}

	[Theory]
	[InlineData("2147483648")]
	[InlineData("\"-2147483649\"")]
	public void Int32_OutOfRange_Throws(string json)
	{
		var act = () => Json.Read(new TolerantNullableInt32Converter(), json);

		act.Should().Throw<JsonException>().WithMessage("*out of range for a 32-bit integer.");
	}

	[Fact]
	public void Double_Unreadable_Throws()
	{
		var act = () => Json.Read(new TolerantDoubleConverter(), "\"1,5\"");

		act.Should().Throw<JsonException>().WithMessage("Cannot read a number from \"1,5\".");
	}

	[Fact]
	public void Writers_WriteJsonNumbers()
	{
		Json.Write(new TolerantInt32Converter(), 7).Should().Be("7");
		Json.Write<int?>(new TolerantNullableInt32Converter(), 7).Should().Be("7");
		Json.Write(new TolerantInt64Converter(), long.MaxValue).Should().Be("9223372036854775807");
		Json.Write<long?>(new TolerantNullableInt64Converter(), -1).Should().Be("-1");
		Json.Write(new TolerantDoubleConverter(), 1.5).Should().Be("1.5");
		Json.Write<double?>(new TolerantNullableDoubleConverter(), 0.25).Should().Be("0.25");
	}

	[Theory]
	[InlineData(double.NaN, "\"NaN\"")]
	[InlineData(double.PositiveInfinity, "\"Infinity\"")]
	[InlineData(double.NegativeInfinity, "\"-Infinity\"")]
	public void Double_WritesNonFiniteValuesAsNamedLiterals(double value, string json)
	{
		Json.Write(new TolerantDoubleConverter(), value).Should().Be(json);
		Json.Write<double?>(new TolerantNullableDoubleConverter(), value).Should().Be(json);
		JsonSerializer.Deserialize<double>(json, SplunkJson.Options).Should().Be(value);
	}

	[Fact]
	public void NullableWriters_WriteNull()
	{
		Json.Write<int?>(new TolerantNullableInt32Converter(), null).Should().Be("null");
		Json.Write<long?>(new TolerantNullableInt64Converter(), null).Should().Be("null");
		Json.Write<double?>(new TolerantNullableDoubleConverter(), null).Should().Be("null");
	}
}
