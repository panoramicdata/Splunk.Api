using Splunk.Api.Serialization;
using System.Text.Json;

namespace Splunk.Api.Test.Core;

public class TolerantStringListConverterTests
{
	private static readonly JsonSerializerOptions Options = new() { Converters = { new TolerantStringListConverter() } };

	[Theory]
	[InlineData("\"Available Bytes\"", new[] { "Available Bytes" })]
	[InlineData("\"\"", new string[0])]
	[InlineData("null", new string[0])]
	[InlineData("[\"a\", 1, true, null]", new[] { "a", "1", "true" })]
	public void Read_AcceptsAStringOrAnArray(string json, string[] expected)
		=> JsonSerializer.Deserialize<IReadOnlyList<string>>(json, Options).Should().Equal(expected);

	[Fact]
	public void Write_WritesAnArrayOrNull()
	{
		JsonSerializer.Serialize<IReadOnlyList<string>>(["a", "b"], Options).Should().Be("[\"a\",\"b\"]");
		JsonSerializer.Serialize<IReadOnlyList<string>?>(null, Options).Should().Be("null");
	}
}
