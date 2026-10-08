using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public partial class SavedSearchesTests
{
	private const string ItemPath = "/services/saved/searches/my%20alert";
	private static readonly string SavedSearchFeed = SearchRequestAssert.Feed("my alert", SavedSearchJson.SavedSearchContent);

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetWithEveryOption()
	{
		var stub = TestClient.Stub(SavedSearchFeed);
		using var client = TestClient.Create(stub);

		await client.SavedSearches.ListAsync(
			new SavedSearchListOptions { Count = 5, EarliestTime = "-1h", LatestTime = "now", ListDefaultActionArgs = true, AddOrphanField = false },
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Get,
			"/services/saved/searches",
			"?earliest_time=-1h&latest_time=now&listDefaultActionArgs=true&add_orphan_field=false&count=5&output_mode=json");
	}

	[Fact]
	public async Task GetAsync_SendsGetToTheEscapedName()
	{
		var stub = TestClient.Stub(SavedSearchFeed);
		using var client = TestClient.Create(stub);

		await client.SavedSearches.GetAsync("my alert", null, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath, "?output_mode=json");
	}

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		using var client = TestClient.Create(TestClient.Stub(SavedSearchFeed));

		var saved = (await client.SavedSearches.GetAsync("my alert", null, Ct)).Entries.Should().ContainSingle().Subject.Content!;

		saved.Search.Should().Be("| makeresults count=2");
		saved.QualifiedSearch.Should().Be(" makeresults count=2");
		saved.Description.Should().Be("probe");
		saved.IsScheduled.Should().BeTrue();
		saved.CronSchedule.Should().Be("*/5 * * * *");
		saved.NextScheduledTime.Should().Be("2026-10-08 13:50:00 UTC");
		saved.ScheduledTimes.Should().Equal(DateTimeOffset.FromUnixTimeSeconds(1791467700), DateTimeOffset.FromUnixTimeSeconds(1791468000));
		saved.IsVisible.Should().BeTrue();
		saved.ScheduleWindow.Should().Be("0");
		saved.SchedulePriority.Should().Be("default");
		saved.RealtimeSchedule.Should().BeTrue();
		saved.MaxConcurrent.Should().Be(1);
		saved.RunOnStartup.Should().BeFalse();
		saved.RunNTimes.Should().Be(0);
		saved.Actions.Should().Be("email");
		saved.ActionEmail.Should().BeFalse();
		saved.ActionEmailTo.Should().Be("ops@example.com");
		saved.AutoSummarize.Should().BeFalse();
		saved.RequestUiDispatchApp.Should().Be("search");
		saved.RequestUiDispatchView.Should().Be("search");
		saved.WorkloadPool.Should().BeEmpty();
		saved.Disabled.Should().BeFalse();
		saved.AdditionalProperties.Should().ContainKeys("display.general.type", "action.email.sendpdf");
		AssertDispatchAndAlert(saved);
	}

	private static void AssertDispatchAndAlert(SavedSearch saved)
	{
		saved.DispatchEarliestTime.Should().Be("-15m");
		saved.DispatchLatestTime.Should().Be("now");
		saved.DispatchTtl.Should().Be("2p");
		saved.DispatchMaxCount.Should().Be(500000);
		saved.DispatchMaxTime.Should().Be(0);
		saved.DispatchBuckets.Should().Be(0);
		saved.DispatchLookups.Should().BeTrue();
		saved.DispatchAs.Should().Be("owner");
		saved.AlertType.Should().Be("number of events");
		saved.AlertComparator.Should().Be("greater than");
		saved.AlertThreshold.Should().Be("0");
		saved.AlertCondition.Should().BeEmpty();
		saved.AlertSeverity.Should().Be(4);
		saved.AlertDigestMode.Should().BeTrue();
		saved.AlertExpires.Should().Be("24h");
		saved.AlertSuppress.Should().BeTrue();
		saved.AlertSuppressPeriod.Should().Be("1h");
		saved.AlertSuppressFields.Should().Be("host");
		saved.AlertTrack.Should().Be("true");
	}

	private static readonly SavedSearchCreateRequest EveryTypedSetting = new()
	{
		Name = "my alert",
		Search = "search index=_internal error",
		Description = "d",
		IsScheduled = true,
		CronSchedule = "*/5 * * * *",
		Disabled = false,
		IsVisible = true,
		ScheduleWindow = "auto",
		SchedulePriority = "higher",
		RealtimeSchedule = false,
		MaxConcurrent = 2,
		RunOnStartup = false,
		DispatchEarliestTime = "-15m",
		DispatchLatestTime = "now",
		DispatchTtl = "2p",
		DispatchMaxCount = 1000,
		DispatchMaxTime = 60,
		DispatchAs = "owner",
		RequestUiDispatchApp = "search",
		RequestUiDispatchView = "search",
		WorkloadPool = "pool",
		Actions = "email,webhook",
		ActionEmailTo = "ops@example.com",
		ActionEmailSubject = "Alert",
		AlertType = "number of events",
		AlertComparator = "greater than",
		AlertThreshold = "10",
		AlertCondition = "search count > 10",
		AlertSeverity = 5,
		AlertDigestMode = true,
		AlertExpires = "24h",
		AlertSuppress = true,
		AlertSuppressPeriod = "1h",
		AlertSuppressFields = "host",
		AlertTrack = true,
		AdditionalParameters = { ["action.webhook.param.url"] = "https://example.com/hook" }
	};

	private const string EveryTypedSettingBody = "name=my+alert&search=search+index%3D_internal+error"
		+ "&actions=email%2Cwebhook&action.email.to=ops%40example.com&action.email.subject=Alert&alert_type=number+of+events"
		+ "&alert_comparator=greater+than&alert_threshold=10&alert_condition=search+count+%3E+10&alert.severity=5"
		+ "&alert.digest_mode=true&alert.expires=24h&alert.suppress=true&alert.suppress.period=1h&alert.suppress.fields=host&alert.track=true"
		+ "&description=d&is_scheduled=true&cron_schedule=%2A%2F5+%2A+%2A+%2A+%2A"
		+ "&disabled=false&is_visible=true&schedule_window=auto&schedule_priority=higher&realtime_schedule=false&max_concurrent=2"
		+ "&run_on_startup=false&dispatch.earliest_time=-15m&dispatch.latest_time=now&dispatch.ttl=2p&dispatch.max_count=1000"
		+ "&dispatch.max_time=60&dispatchAs=owner&request.ui_dispatch_app=search&request.ui_dispatch_view=search&workload_pool=pool"
		+ "&action.webhook.param.url=https%3A%2F%2Fexample.com%2Fhook";

	[Fact]
	public async Task CreateAsync_SendsEveryTypedSettingAsForm()
	{
		var stub = TestClient.Stub(SavedSearchFeed);
		using var client = TestClient.Create(stub);

		await client.SavedSearches.CreateAsync(EveryTypedSetting, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, "/services/saved/searches", "?output_mode=json", EveryTypedSettingBody);
	}

	[Fact]
	public async Task UpdateAsync_SendsOnlyWhatIsSet()
	{
		var stub = TestClient.Stub(SavedSearchFeed);
		using var client = TestClient.Create(stub);

		await client.SavedSearches.UpdateAsync("my alert", new SavedSearchUpdateRequest { Search = "| makeresults", Description = "updated" }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, ItemPath, "?output_mode=json", "description=updated&search=%7C+makeresults");
	}

	[Fact]
	public async Task DeleteAsync_SendsDelete()
	{
		var stub = TestClient.Stub("""{"entry":[]}""");
		using var client = TestClient.Create(stub);

		await client.SavedSearches.DeleteAsync("my alert", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Delete, ItemPath, "?output_mode=json");
	}

	[Fact]
	public async Task CreateAsync_Duplicate_RaisesConflict()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Unable to create saved search with name 'x'. A saved search with that name already exists."}]}""", HttpStatusCode.Conflict));

		await SearchRequestAssert.FailsWith(() => client.SavedSearches.CreateAsync(new SavedSearchCreateRequest { Name = "x", Search = "y" }, Ct), HttpStatusCode.Conflict, "Unable to create saved search with name 'x'. A saved search with that name already exists.");
	}
}
