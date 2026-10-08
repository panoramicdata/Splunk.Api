using Splunk.Api.Serialization;
using Splunk.Api.Test.Support;
using System.Text.Json;

namespace Splunk.Api.Test.Core;

/// <summary>The boolean and string converters, driven directly so that every JSON token type reaches them.</summary>
public class TolerantConverterTests
{
	[Theory]
	[InlineData("true", true)]
	[InlineData("false", false)]
	[InlineData("1", true)]
	[InlineData("0", false)]
	[InlineData("0.0", false)]
	[InlineData("-2.5", true)]
	[InlineData("\"1\"", true)]
	[InlineData("\" TRUE \"", true)]
	[InlineData("\"t\"", true)]
	[InlineData("\"Yes\"", true)]
	[InlineData("\"y\"", true)]
	[InlineData("\"on\"", true)]
	[InlineData("\"0\"", false)]
	[InlineData("\"False\"", false)]
	[InlineData("\"f\"", false)]
	[InlineData("\"no\"", false)]
	[InlineData("\"N\"", false)]
	[InlineData("\"off\"", false)]
	[InlineData("\"2\"", true)]
	[InlineData("\"0.0\"", false)]
	public void Boolean_ReadsEveryForm(string json, bool expected)
	{
		Json.Read(new TolerantBooleanConverter(), json).Should().Be(expected);
		Json.Read(new TolerantNullableBooleanConverter(), json).Should().Be(expected);
	}

	[Theory]
	[InlineData("null")]
	[InlineData("\"\"")]
	[InlineData("\"  \"")]
	public void Boolean_NullOrEmpty_IsFalse_OrNullWhenNullable(string json)
	{
		Json.Read(new TolerantBooleanConverter(), json).Should().BeFalse();
		Json.Read(new TolerantNullableBooleanConverter(), json).Should().BeNull();
	}

	[Theory]
	[InlineData("\"maybe\"", "Cannot read a boolean from \"maybe\".")]
	[InlineData("{}", "Cannot read a boolean from a JSON StartObject.")]
	[InlineData("[]", "Cannot read a boolean from a JSON StartArray.")]
	public void Boolean_Unreadable_Throws(string json, string message)
	{
		var act = () => Json.Read(new TolerantBooleanConverter(), json);

		act.Should().Throw<JsonException>().WithMessage(message);
	}

	[Theory]
	[InlineData(true, "true")]
	[InlineData(false, "false")]
	public void Boolean_WritesJsonBooleans(bool value, string json)
	{
		Json.Write(new TolerantBooleanConverter(), value).Should().Be(json);
		Json.Write<bool?>(new TolerantNullableBooleanConverter(), value).Should().Be(json);
	}

	[Fact]
	public void NullableBoolean_WritesNull()
		=> Json.Write<bool?>(new TolerantNullableBooleanConverter(), null).Should().Be("null");

	[Theory]
	[InlineData("\"text\"", "text")]
	[InlineData("\"\"", "")]
	[InlineData("42", "42")]
	[InlineData("-1.50e3", "-1.50e3")]
	[InlineData("true", "true")]
	[InlineData("false", "false")]
	[InlineData("null", null)]
	[InlineData("{\"a\": [1, 2]}", "{\"a\": [1, 2]}")]
	[InlineData("[\"x\"]", "[\"x\"]")]
	public void String_ReadsAnyTokenAsText(string json, string? expected)
		=> Json.Read(new TolerantStringConverter(), json).Should().Be(expected);

	[Fact]
	public void String_WritesAJsonString()
		=> Json.Write(new TolerantStringConverter(), "a\"b").Should().Be("\"a\\u0022b\"");

	private sealed class Shapes
	{
		public string? Name { get; init; }

		public bool Flag { get; init; }

		public bool? Maybe { get; init; }

		public IReadOnlyDictionary<string, string> Links { get; init; } = new Dictionary<string, string>();
	}

	[Fact]
	public void Options_ReadLooseShapesInAModel()
	{
		var shapes = JsonSerializer.Deserialize<Shapes>(
			"""{"name": 12, "FLAG": "1", "maybe": "", "links": {"a": true, "b": {"c": 1}}}""",
			SplunkJson.Options)!;

		shapes.Name.Should().Be("12");
		shapes.Flag.Should().BeTrue();
		shapes.Maybe.Should().BeNull();
		shapes.Links.Should().Equal(new Dictionary<string, string> { ["a"] = "true", ["b"] = """{"c": 1}""" });
	}

	[Fact]
	public void Options_AreReadOnly()
	{
		var act = () => SplunkJson.Options.PropertyNameCaseInsensitive = false;

		act.Should().Throw<InvalidOperationException>();
	}
}
