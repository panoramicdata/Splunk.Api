using Splunk.Api.Models.Search;
using System.Text.Json;

namespace Splunk.Api.Test.Groups;

public class SearchResultTests
{
	private static SearchResult Read(string json) => JsonSerializer.Deserialize<SearchResult>(json, SplunkJson.Options)!;

	[Fact]
	public void Read_KeepsNumbersBooleansAndOtherShapesAsText()
	{
		var result = Read("""{"n":42,"b":true,"o":{"a":1},"mv":["x",null,7],"_raw":"line"}""");

		result["n"].Should().Be("42");
		result["b"].Should().Be("true");
		result["o"].Should().Be("""{"a":1}""");
		result.GetValues("mv").Should().Equal("x", "7");
		result.Raw.Should().Be("line");
	}

	[Fact]
	public void Accessors_HandleAbsentFields()
	{
		var result = Read("""{"a":"1"}""");

		result["missing"].Should().BeNull();
		result.GetValues("missing").Should().BeEmpty();
		result.Contains("missing").Should().BeFalse();
		result.IsMultivalue("missing").Should().BeFalse();
		result.Raw.Should().BeNull();
		result.Time.Should().BeNull();
	}

	[Theory]
	[InlineData("2026-10-08T13:47:15.000+01:00", "2026-10-08T12:47:15Z")]
	[InlineData("2026-10-08 13:48:34.000 GMT", "2026-10-08T13:48:34Z")]
	[InlineData("not a time", null)]
	[InlineData("", null)]
	public void Time_ReadsBothSplunkFormats(string text, string? expected)
	{
		var result = new SearchResult(new Dictionary<string, IReadOnlyList<string>> { ["_time"] = [text] });

		result.Time.Should().Be(expected is null ? null : DateTimeOffset.Parse(expected, System.Globalization.CultureInfo.InvariantCulture));
	}

	[Fact]
	public void Write_RoundTripsSingleAndMultivalueFields()
	{
		var result = Read("""{"a":"1","mv":["x","y"],"none":null}""");

		var json = JsonSerializer.Serialize(result, SplunkJson.Options);

		json.Should().Be("""{"a":"1","mv":["x","y"],"none":[]}""");
	}

	[Fact]
	public void Read_RejectsANonObject()
	{
		var act = () => Read("[1]");

		act.Should().Throw<JsonException>();
	}

	[Fact]
	public void Constructor_RejectsNull()
	{
		var act = () => new SearchResult(null!);

		act.Should().Throw<ArgumentNullException>();
	}
}
