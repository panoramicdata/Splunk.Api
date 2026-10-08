using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SearchConcurrencySettingsTests
{
	// Captured from Splunk Enterprise 10.6.0.5, trimmed.
	private const string SettingsJson = """
		{
			"entry": [
				{ "name": "scheduler", "author": "system", "content": { "auto_summary_perc": 50, "eai:acl": null, "max_searches_perc": 50 } },
				{ "name": "search", "author": "system", "content": { "base_max_searches": 6, "eai:acl": null, "max_rt_search_multiplier": 1, "max_searches_per_cpu": 1, "shc_adhoc_quota_enforcement": "off", "total_search_concurrency_limit": "auto" } }
			],
			"paging": { "total": 2, "perPage": 30, "offset": 0 }
		}
		""";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGet()
	{
		var stub = TestClient.Stub(SettingsJson);
		using var client = TestClient.Create(stub);

		await client.SearchConcurrencySettings.ListAsync(Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/search/concurrency-settings", "?output_mode=json");
	}

	[Fact]
	public async Task ListAsync_MapsBothEntries()
	{
		using var client = TestClient.Create(TestClient.Stub(SettingsJson));

		var feed = await client.SearchConcurrencySettings.ListAsync(Ct);

		var scheduler = feed.Entries[0].Content!;
		scheduler.MaxSearchesPercent.Should().Be(50);
		scheduler.AutoSummaryPercent.Should().Be(50);
		scheduler.BaseMaxSearches.Should().BeNull();
		var search = feed.Entries[1].Content!;
		search.BaseMaxSearches.Should().Be(6);
		search.MaxSearchesPerCpu.Should().Be(1);
		search.MaxRealtimeSearchMultiplier.Should().Be(1);
		search.TotalSearchConcurrencyLimit.Should().Be("auto");
		search.ShcAdhocQuotaEnforcement.Should().Be("off");
		search.MaxSearchesPercent.Should().BeNull();
	}

	[Fact]
	public async Task UpdateSchedulerAsync_SendsThePercentages()
	{
		var stub = TestClient.Stub(SettingsJson);
		using var client = TestClient.Create(stub);

		await client.SearchConcurrencySettings.UpdateSchedulerAsync(new SchedulerConcurrencyUpdateRequest { MaxSearchesPercent = 60, AutoSummaryPercent = 40 }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, "/services/search/concurrency-settings/scheduler", "?output_mode=json", "max_searches_perc=60&auto_summary_perc=40");
	}

	[Fact]
	public async Task UpdateSearchAsync_SendsTheLimits()
	{
		var stub = TestClient.Stub(SettingsJson);
		using var client = TestClient.Create(stub);

		await client.SearchConcurrencySettings.UpdateSearchAsync(new SearchConcurrencyUpdateRequest { MaxSearchesPerCpu = 2, BaseMaxSearches = 8, MaxRealtimeSearchMultiplier = 1.5 }, Ct);

		SearchRequestAssert.Sent(stub,
			HttpMethod.Post,
			"/services/search/concurrency-settings/search",
			"?output_mode=json",
			"max_searches_per_cpu=2&base_max_searches=8&max_rt_search_multiplier=1.5");
	}

	[Fact]
	public async Task UpdateSearchAsync_Error_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Invalid value for base_max_searches."}]}""", HttpStatusCode.BadRequest));

		await SearchRequestAssert.FailsWith(() => client.SearchConcurrencySettings.UpdateSearchAsync(new SearchConcurrencyUpdateRequest { BaseMaxSearches = -1 }, Ct), HttpStatusCode.BadRequest, "Invalid value for base_max_searches.");
	}
}
