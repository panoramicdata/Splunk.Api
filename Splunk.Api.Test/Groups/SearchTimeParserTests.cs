using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SearchTimeParserTests
{
	private const string ParsedJson = """{"-1d":"2026-10-07T13:49:18.000+00:00","now":"2026-10-08T13:49:18.000+00:00","@d":"2026-10-08T00:00:00.000+00:00"}""";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ParseAsync_SendsEachTimeAndTheOptions()
	{
		var stub = TestClient.Stub(ParsedJson);
		using var client = TestClient.Create(stub);

		await client.TimeParser.ParseAsync(["-1d", "now", "@d"], new TimeParserOptions { Now = "1791467358", OutputTimeFormat = "%s", TimeFormat = "%s" }, Ct);

		stub.ShouldHaveSent(
			HttpMethod.Get,
			"/services/search/timeparser",
			"?time=-1d&time=now&time=%40d&now=1791467358&output_time_format=%25s&time_format=%25s&output_mode=json");
	}

	[Fact]
	public async Task ParseAsync_MapsEachTime()
	{
		using var client = TestClient.Create(TestClient.Stub(ParsedJson));

		var parsed = await client.TimeParser.ParseAsync(["-1d", "now", "@d"], null, Ct);

		parsed.Should().HaveCount(3);
		parsed["@d"].Should().Be("2026-10-08T00:00:00.000+00:00");
		parsed["-1d"].Should().Be("2026-10-07T13:49:18.000+00:00");
	}

	[Fact]
	public async Task ParseAsync_InvalidTime_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"FATAL","text":"Invalid time."}]}""", HttpStatusCode.BadRequest));

		var act = () => client.TimeParser.ParseAsync(["garbage"], null, Ct);

		await act.ShouldFailWith(HttpStatusCode.BadRequest, "Invalid time.");
	}
}
