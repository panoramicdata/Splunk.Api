using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.IntegrationTest.Search;

[Collection(SplunkTestGroup.Name)]
public class SplunkSearchIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task RunAsync_ReadsEveryPageAndDeletesTheJob()
	{
		var sid = SplunkFixture.UniqueName("run");

		var run = await Client.Search.RunAsync(
			new SearchJobCreateRequest { Search = "| makeresults count=25 | streamstats count as n", Id = sid },
			new SearchRunOptions { PageSize = 10, PollInterval = TimeSpan.FromMilliseconds(200), Timeout = TimeSpan.FromMinutes(2) },
			Ct);

		run.Job.IsDone.Should().BeTrue();
		run.Results.Select(r => r["n"]).Should().Equal(Enumerable.Range(1, 25).Select(n => n.ToString(System.Globalization.CultureInfo.InvariantCulture)));
		run.Fields.Select(f => f.Name).Should().Contain("n");
		var jobs = await Client.SearchJobs.ListAsync(new ListOptions { Count = 0 }, Ct);
		jobs.Entries.Should().NotContain(e => e.Content!.Sid == sid);
	}

	[Fact]
	public async Task RunAsync_SearchThatFailsWhileRunning_RaisesSplunkSearchException()
	{
		var act = () => Client.Search.RunAsync("| makeresults | eval x=nosuchfunc_splunk_api(1)", Ct);

		var thrown = await act.Should().ThrowAsync<SplunkSearchException>();
		thrown.Which.Message.Should().Contain("nosuchfunc_splunk_api");
		thrown.Which.Job!.IsFailed.Should().BeTrue();
	}

	[Fact]
	public async Task OneshotAsync_ReturnsEveryResult()
	{
		var results = await Client.Search.OneshotAsync("| makeresults count=150", Ct);

		results.Results.Should().HaveCount(150);
	}

	[Fact]
	public async Task ExportAsync_StreamsResultsAsTheyArrive()
	{
		var rows = await Client.Search.ExportAsync("search index=_internal earliest=-24h | stats count by sourcetype", Ct).ToListAsync(Ct);

		rows.Should().NotBeEmpty();
		rows.Should().OnlyContain(r => r.Contains("sourcetype") && r.Contains("count"));
	}

	[Fact]
	public async Task ExportAsync_CanBeAbandonedEarly()
	{
		var first = await Client.Search.ExportAsync(new SearchExportRequest { Search = "search index=_internal", EarliestTime = "-24h" }, Ct).FirstAsync(Ct);

		first.Raw.Should().NotBeNullOrEmpty();
	}
}
