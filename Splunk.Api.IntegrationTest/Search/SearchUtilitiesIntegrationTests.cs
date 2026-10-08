using Splunk.Api.Models;
using Splunk.Api.Models.Search;
using System.Net;

namespace Splunk.Api.IntegrationTest.Search;

/// <summary>The parser, time parser, typeahead and custom command endpoints.</summary>
[Collection(SplunkTestGroup.Name)]
public class SearchUtilitiesIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task Parser_ParsesASearch()
	{
		var parsed = await Client.SearchParser.ParseAsync(new SearchParserRequest { Query = "search index=_internal | stats count by host", ParseOnly = true }, Ct);

		parsed.Commands.Select(c => c.Command).Should().Equal("search", "stats");
		parsed.ReportsSearch.Should().Contain("stats");
		parsed.IsStreamingSearch.Should().BeFalse();
	}

	[Fact]
	public async Task Parser_UnknownCommand_RaisesBadRequest()
	{
		var act = () => Client.SearchParser.ParseAsync(new SearchParserRequest { Query = "| nosuchcmd" }, Ct);

		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		thrown.Which.Message.Should().Be("Unknown search command 'nosuchcmd'.");
	}

	[Fact]
	public async Task TimeParser_ResolvesRelativeTimes()
	{
		var parsed = await Client.TimeParser.ParseAsync(["@d", "-1d@d"], new TimeParserOptions { Now = "1791467358", OutputTimeFormat = "%s" }, Ct);

		var midnight = long.Parse(parsed["@d"], System.Globalization.CultureInfo.InvariantCulture);
		(midnight % 3600).Should().Be(0);
		long.Parse(parsed["-1d@d"], System.Globalization.CultureInfo.InvariantCulture).Should().BeLessThan(midnight);
	}

	[Fact]
	public async Task TimeParser_InvalidTime_RaisesBadRequest()
	{
		var act = () => Client.TimeParser.ParseAsync(["garbage"], null, Ct);

		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		thrown.Which.Message.Should().Be("Invalid time.");
	}

	[Fact]
	public async Task Typeahead_SuggestsIndexes()
	{
		var suggestions = await Client.Typeahead.GetAsync("index=_", 50, null, Ct);

		suggestions.Results.Should().Contain(s => s.Content == "index=\"_internal\"");
	}

	[Fact]
	public async Task SearchCommands_ListAndGet()
	{
		var list = await Client.SearchCommands.ListAsync(new ListOptions { Count = 5 }, Ct);
		var first = list.Entries.Should().NotBeEmpty().And.Subject.First();

		var feed = await Client.SearchCommands.GetAsync(first.Name, Ct);

		var command = feed.Entries.Should().ContainSingle().Subject.Content!;
		command.Type.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task SearchCommands_BuiltInCommand_IsNotFound()
	{
		var act = () => Client.SearchCommands.GetAsync("eval", Ct);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
