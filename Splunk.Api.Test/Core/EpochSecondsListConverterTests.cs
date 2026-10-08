using Splunk.Api.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Test.Core;

public class EpochSecondsListConverterTests
{
	private sealed class Holder
	{
		[JsonPropertyName("times")]
		[JsonConverter(typeof(EpochSecondsListConverter))]
		public IReadOnlyList<DateTimeOffset> Times { get; init; } = [];
	}

	private static Holder Read(string json) => JsonSerializer.Deserialize<Holder>(json, SplunkJson.Options)!;

	[Fact]
	public void Read_AcceptsNumbersStringsAndFractionsAndSkipsNulls()
	{
		var holder = Read("""{"times":[1791467700,"1791468000",null,"1791468300.5"]}""");

		holder.Times.Should().Equal(
			DateTimeOffset.FromUnixTimeSeconds(1791467700),
			DateTimeOffset.FromUnixTimeSeconds(1791468000),
			DateTimeOffset.FromUnixTimeMilliseconds(1791468300500));
	}

	[Fact]
	public void Read_NullIsEmpty() => Read("""{"times":null}""").Times.Should().BeEmpty();

	[Fact]
	public void Read_RejectsANonArray()
	{
		var act = () => Read("""{"times":"1791467700"}""");

		act.Should().Throw<JsonException>();
	}

	[Fact]
	public void Write_WritesEpochSeconds()
	{
		var json = JsonSerializer.Serialize(new Holder { Times = [DateTimeOffset.FromUnixTimeMilliseconds(1791467700500)] }, SplunkJson.Options);

		json.Should().Be("""{"times":[1791467700.5]}""");
	}
}
