using Splunk.Api.Models.Search;

namespace Splunk.Api.IntegrationTest.Search;

/// <summary>
/// The scheduler status and concurrency settings. The instance is shared, so the writes send back the values just read:
/// they exercise the endpoints without changing anything.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class SearchSettingsIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task Scheduler_GetAndRewriteTheCurrentState()
	{
		var before = (await Client.SearchScheduler.GetStatusAsync(Ct)).Entries.Should().ContainSingle().Subject;
		before.Name.Should().Be("status");

		await Client.SearchScheduler.SetStatusAsync(new SearchSchedulerStatusRequest { Disabled = before.Content!.SavedSearchesDisabled }, Ct);

		var after = (await Client.SearchScheduler.GetStatusAsync(Ct)).Entries[0].Content!;
		after.SavedSearchesDisabled.Should().Be(before.Content.SavedSearchesDisabled);
	}

	[Fact]
	public async Task ConcurrencySettings_ListAndRewriteTheCurrentValues()
	{
		var feed = await Client.SearchConcurrencySettings.ListAsync(Ct);
		var scheduler = feed.Entries.Single(e => e.Name == "scheduler").Content!;
		var search = feed.Entries.Single(e => e.Name == "search").Content!;
		scheduler.MaxSearchesPercent.Should().BeInRange(1, 100);
		search.BaseMaxSearches.Should().BePositive();

		var updatedScheduler = await Client.SearchConcurrencySettings.UpdateSchedulerAsync(
			new SchedulerConcurrencyUpdateRequest { MaxSearchesPercent = scheduler.MaxSearchesPercent, AutoSummaryPercent = scheduler.AutoSummaryPercent },
			Ct);
		var updatedSearch = await Client.SearchConcurrencySettings.UpdateSearchAsync(
			new SearchConcurrencyUpdateRequest
			{
				MaxSearchesPerCpu = search.MaxSearchesPerCpu,
				BaseMaxSearches = search.BaseMaxSearches,
				MaxRealtimeSearchMultiplier = search.MaxRealtimeSearchMultiplier
			},
			Ct);

		updatedScheduler.Entries.Should().ContainSingle().Which.Content!.MaxSearchesPercent.Should().Be(scheduler.MaxSearchesPercent);
		updatedSearch.Entries.Should().ContainSingle().Which.Content!.BaseMaxSearches.Should().Be(search.BaseMaxSearches);
	}
}
