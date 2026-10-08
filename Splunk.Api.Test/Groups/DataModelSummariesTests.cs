using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class DataModelSummariesTests
{
	private const string EntryName = "tstats:DM_search_web";

	// Shape captured from Splunk Enterprise 10.6.0.5 (GET admin/summarization/tstats:DM_{app}_{model}); the values and
	// run_stats of a built summary follow the reference's example.
	private static readonly string SummaryJson = KnowledgeTestKit.Feed("admin/summarization", EntryName, """
		{
			"eai:acl": null,
			"search": " summarize tstats=t override=partial id=DM_search_web [ search index=_internal ]",
			"summary.access_count": 2,
			"summary.access_time": 1565733421,
			"summary.average_time": 3.028,
			"summary.buckets": 11,
			"summary.buckets_size": 461,
			"summary.complete": "1.000000",
			"summary.earliest_time": 1565398764,
			"summary.id": "DM_search_web",
			"summary.is_inprogress": false,
			"summary.last_error": "",
			"summary.last_sid": "scheduler__nobody__search__RMD5837da1d4b8a764d1_at_1565733480_379",
			"summary.latest_dispatch_time": 1565733481,
			"summary.latest_run_duration": 5.691,
			"summary.latest_time": 1565730106,
			"summary.mod_time": 1565733421,
			"summary.p50": 1.287,
			"summary.p90": 5.859,
			"summary.run_stats": { "1565730661": { "dispatch_time": 1565730661, "run_duration": "0.357" } },
			"summary.size": 614400,
			"summary.time_range": 86400
		}
		""");

	[Fact]
	public async Task ListAsync_SendsByTstats()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModelSummaries.ListAsync(new DataModelSummaryListOptions { ByTstats = true }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/admin/summarization", "?by_tstats=true&output_mode=json");

	[Fact]
	public async Task GetAsync_BuildsTheTstatsSummaryName()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModelSummaries.GetAsync("search", "web", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/admin/summarization/tstats:DM_search_web");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.DataModelSummaries.GetAsync("search", "web", TestContext.Current.CancellationToken), SummaryJson);

		entry.ShouldBeTheCapturedEntry(EntryName);
		var summary = entry.Content!;
		summary.Search.Should().Contain("summarize tstats=t");
		summary.SummaryId.Should().Be("DM_search_web");
		summary.Complete.Should().Be(1);
		summary.IsInProgress.Should().BeFalse();
		summary.Size.Should().Be(614400);
		summary.Buckets.Should().Be(11);
		summary.BucketsSizeMB.Should().Be(461);
		summary.TimeRangeSeconds.Should().Be(86400);
		summary.EarliestTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1565398764));
		summary.LatestTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1565730106));
		summary.ModifiedTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1565733421));
		summary.AccessTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1565733421));
		summary.AccessCount.Should().Be(2);
		summary.LastSearchId.Should().StartWith("scheduler__nobody__search__");
		summary.LastError.Should().BeEmpty();
		summary.LatestDispatchTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1565733481));
		summary.LatestRunDuration.Should().Be(5.691);
		summary.AverageTime.Should().Be(3.028);
		summary.P50.Should().Be(1.287);
		summary.P90.Should().Be(5.859);
		var run = summary.RunStats.Should().ContainKey("1565730661").WhoseValue;
		run.DispatchTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1565730661));
		run.RunDuration.Should().Be(0.357);
	}

	[Fact]
	public Task GetAsync_NotAccelerated_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.DataModelSummaries.GetAsync("search", "missing", TestContext.Current.CancellationToken));
}
