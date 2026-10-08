using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class SavedSearchesTests
{
	[Fact]
	public async Task DispatchAsync_SendsEveryOverrideAndReturnsTheSid()
	{
		var stub = TestClient.Stub("""{"sid":"admin__admin__search__RMD5_at_1791467430_1"}""");
		using var client = TestClient.Create(stub);

		var created = await client.SavedSearches.DispatchAsync(
			"my alert",
			new SavedSearchDispatchRequest
			{
				DispatchNow = "1791467430",
				DispatchEarliestTime = "-1h",
				DispatchLatestTime = "now",
				TriggerActions = false,
				ForceDispatch = true,
				DispatchAs = "user",
				ReplaySpeed = 2.5,
				ReplayEarliestTime = "-2h",
				ReplayLatestTime = "-1h",
				AdditionalParameters = { ["args.host"] = "web01" }
			},
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Post,
			ItemPath + "/dispatch",
			"?output_mode=json",
			"dispatch.now=1791467430&dispatch.earliest_time=-1h&dispatch.latest_time=now&trigger_actions=false&force_dispatch=true"
				+ "&dispatchAs=user&replay_speed=2.5&replay_et=-2h&replay_lt=-1h&args.host=web01");
		created.Sid.Should().Be("admin__admin__search__RMD5_at_1791467430_1");
	}

	[Fact]
	public async Task GetHistoryAsync_SendsGetWithTheTriplet()
	{
		var stub = TestClient.Stub(SearchRequestAssert.Feed("admin__admin__search__RMD5_at_1791467430_1", SavedSearchJson.HistoryContent));
		using var client = TestClient.Create(stub);

		await client.SavedSearches.GetHistoryAsync("my alert", new SavedSearchHistoryOptions { SavedSearch = "admin:search:my alert", Count = 10 }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath + "/history", "?savedsearch=admin%3Asearch%3Amy%20alert&count=10&output_mode=json");
	}

	[Fact]
	public async Task GetHistoryAsync_MapsTheJobs()
	{
		using var client = TestClient.Create(TestClient.Stub(SearchRequestAssert.Feed("sid_1", SavedSearchJson.HistoryContent)));

		var entry = (await client.SavedSearches.GetHistoryAsync("my alert", null, Ct)).Entries.Should().ContainSingle().Subject;

		entry.Name.Should().Be("sid_1");
		var job = entry.Content!;
		job.IsDone.Should().BeTrue();
		job.IsFinalized.Should().BeFalse();
		job.IsRealTimeSearch.Should().BeFalse();
		job.IsSaved.Should().BeFalse();
		job.IsScheduled.Should().BeTrue();
		job.IsZombie.Should().BeFalse();
		job.Start.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467430));
		job.Ttl.Should().Be(600);
	}

	[Fact]
	public async Task RescheduleAsync_SendsTheTime()
	{
		var stub = TestClient.Stub(SavedSearchFeed);
		using var client = TestClient.Create(stub);

		await client.SavedSearches.RescheduleAsync("my alert", RescheduleRequest.At(DateTimeOffset.FromUnixTimeSeconds(1893456000)), Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, ItemPath + "/reschedule", "?output_mode=json", "schedule_time=1893456000");
	}

	[Fact]
	public async Task GetScheduledTimesAsync_SendsTheRange()
	{
		var stub = TestClient.Stub(SavedSearchFeed);
		using var client = TestClient.Create(stub);

		var feed = await client.SavedSearches.GetScheduledTimesAsync("my alert", "now", "+20m", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath + "/scheduled_times", "?earliest_time=now&latest_time=%2B20m&output_mode=json");
		feed.Entries[0].Content!.ScheduledTimes.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetSuppressionAsync_SendsGetWithTheKey()
	{
		var stub = TestClient.Stub(SearchRequestAssert.Feed("my alert", SavedSearchJson.SuppressionContent));
		using var client = TestClient.Create(stub);

		var state = (await client.SavedSearches.GetSuppressionAsync("my alert", new SavedSearchSuppressionOptions { Key = "k", Expiration = "1h" }, Ct)).Entries[0].Content!;

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath + "/suppress", "?key=k&expiration=1h&output_mode=json");
		state.Suppressed.Should().BeTrue();
		state.SuppressionKey.Should().Be("admin;search;my_alert;;");
		state.Expiration.Should().Be("2026-10-08 14:50:00 UTC");
	}

	[Fact]
	public async Task AcknowledgeAsync_SendsTheKey()
	{
		var stub = TestClient.Stub(SearchRequestAssert.Feed("my alert", SavedSearchJson.SuppressionContent));
		using var client = TestClient.Create(stub);

		await client.SavedSearches.AcknowledgeAsync("my alert", new SavedSearchAcknowledgeRequest { Key = "admin;search;my_alert;;" }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, ItemPath + "/acknowledge", "?output_mode=json", "key=admin%3Bsearch%3Bmy_alert%3B%3B");
	}
}
