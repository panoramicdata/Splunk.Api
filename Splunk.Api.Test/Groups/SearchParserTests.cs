using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SearchParserTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (POST search/v2/parser, parse_only=true).
	private const string ParseJson = """
		{"remoteSearch":"search index=_internal  | fields  keepcolorder=t \"host\"","normalizedSearch":"search index=_internal | fields keepcolorder=t \"host\"","remoteTimeOrdered":true,"eventsSearch":"search index=_internal ","eventsTimeOrdered":true,"eventsStreaming":true,"reportsSearch":"stats  count by host","isStreamingSearch":false,"canSummarize":false,"commands":[{"command":"search","rawargs":"index=_internal ","pipeline":"streaming","args":{"search":["index=_internal "]},"isGenerating":true,"streamType":"SP_STREAM"},{"command":"stats","rawargs":"count by host","pipeline":"report","args":{"stat-specifiers":[{"function":"count","rename":"count"}],"groupby-fields":["host"]},"isGenerating":false,"streamType":"SP_STREAMREPORT","isStreamingOpRequired":false,"preStreamingOp":"prestats count by host"}],"futureProperty":1}
		""";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ParseAsync_SendsPostWithEveryOption()
	{
		var stub = TestClient.Stub(ParseJson);
		using var client = TestClient.Create(stub);

		await client.SearchParser.ParseAsync(
			new SearchParserRequest { Query = "search index=_internal | stats count by host", ParseOnly = true, EnableLookups = false, ReloadMacros = true },
			Ct);

		stub.ShouldHaveSent(
			HttpMethod.Post,
			"/services/search/v2/parser",
			"?output_mode=json",
			"q=search+index%3D_internal+%7C+stats+count+by+host&parse_only=true&enable_lookups=false&reload_macros=true");
	}

	[Fact]
	public async Task ParseAsync_MapsThePhasesAndCommands()
	{
		using var client = TestClient.Create(TestClient.Stub(ParseJson));

		var parsed = await client.SearchParser.ParseAsync(new SearchParserRequest { Query = "x" }, Ct);

		parsed.NormalizedSearch.Should().Be("search index=_internal | fields keepcolorder=t \"host\"");
		parsed.RemoteSearch.Should().StartWith("search index=_internal");
		parsed.RemoteTimeOrdered.Should().BeTrue();
		parsed.EventsSearch.Should().Be("search index=_internal ");
		parsed.EventsTimeOrdered.Should().BeTrue();
		parsed.EventsStreaming.Should().BeTrue();
		parsed.ReportsSearch.Should().Be("stats  count by host");
		parsed.IsStreamingSearch.Should().BeFalse();
		parsed.CanSummarize.Should().BeFalse();
		parsed.AdditionalProperties.Should().ContainKey("futureProperty");
		parsed.Commands.Select(c => c.Command).Should().Equal("search", "stats");
		var stats = parsed.Commands[1];
		stats.RawArguments.Should().Be("count by host");
		stats.Pipeline.Should().Be("report");
		stats.IsGenerating.Should().BeFalse();
		stats.StreamType.Should().Be("SP_STREAMREPORT");
		stats.Arguments.GetProperty("groupby-fields")[0].GetString().Should().Be("host");
		stats.AdditionalProperties["preStreamingOp"].GetString().Should().Be("prestats count by host");
		parsed.Commands[0].IsGenerating.Should().BeTrue();
	}

	[Fact]
	public async Task ParseAsync_SyntaxError_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"FATAL","text":"Unknown search command 'nosuchcmd'."}]}""", HttpStatusCode.BadRequest));

		var act = () => client.SearchParser.ParseAsync(new SearchParserRequest { Query = "| nosuchcmd" }, Ct);

		await act.ShouldFailWith(HttpStatusCode.BadRequest, "Unknown search command 'nosuchcmd'.");
	}
}
