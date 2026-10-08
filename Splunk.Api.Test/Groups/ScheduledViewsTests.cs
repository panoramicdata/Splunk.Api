using Splunk.Api.Models;
using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ScheduledViewsTests
{
	private const string Name = "_ScheduledView__my_view";
	private const string ItemPath = "/services/scheduled/views/" + Name;
	private static readonly string ViewFeed = SearchRequestAssert.Feed(Name, SavedSearchJson.ScheduledViewContent);

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetWithPaging()
	{
		var stub = TestClient.Stub(ViewFeed);
		using var client = TestClient.Create(stub);

		await client.ScheduledViews.ListAsync(new ListOptions { Count = 2 }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/scheduled/views", "?count=2&output_mode=json");
	}

	[Fact]
	public async Task GetAsync_SendsGet()
	{
		var stub = TestClient.Stub(ViewFeed);
		using var client = TestClient.Create(stub);

		await client.ScheduledViews.GetAsync(Name, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath, "?output_mode=json");
	}

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		using var client = TestClient.Create(TestClient.Stub(ViewFeed));

		var view = (await client.ScheduledViews.GetAsync(Name, Ct)).Entries.Should().ContainSingle().Subject.Content!;

		view.Description.Should().Be("probe");
		view.IsScheduled.Should().BeTrue();
		view.CronSchedule.Should().Be("0 3 * * *");
		view.NextScheduledTime.Should().Be("2026-10-09 03:00:00 UTC");
		view.ScheduledTimes.Should().Equal(DateTimeOffset.FromUnixTimeSeconds(1791514800), DateTimeOffset.FromUnixTimeSeconds(1791601200));
		view.ScheduleWindow.Should().Be("0");
		view.SchedulePriority.Should().Be("default");
		view.ActionEmail.Should().BeTrue();
		view.ActionEmailTo.Should().Be("nobody@example.com");
		view.ActionEmailPdfView.Should().Be("my_view");
		view.ActionEmailSubjectView.Should().Be("Splunk Dashboard: my_view");
		view.AdditionalProperties.Should().ContainKey("action.email.useNSSubject");
	}

	[Fact]
	public async Task UpdateAsync_SendsTheSchedule()
	{
		var stub = TestClient.Stub(ViewFeed);
		using var client = TestClient.Create(stub);

		await client.ScheduledViews.UpdateAsync(
			Name,
			new ScheduledViewUpdateRequest
			{
				CronSchedule = "0 3 * * *",
				IsScheduled = true,
				ActionEmailTo = "a@example.com",
				Description = "d",
				Disabled = false,
				NextScheduledTime = "1791514800",
				AdditionalParameters = { ["action.email.subject.view"] = "Daily" }
			},
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Post,
			ItemPath,
			"?output_mode=json",
			"cron_schedule=0+3+%2A+%2A+%2A&is_scheduled=true&action.email.to=a%40example.com&description=d&disabled=false&next_scheduled_time=1791514800"
				+ "&action.email.subject.view=Daily");
	}

	[Fact]
	public async Task DeleteAsync_SendsDelete()
	{
		var stub = TestClient.Stub("""{"entry":[]}""");
		using var client = TestClient.Create(stub);

		await client.ScheduledViews.DeleteAsync(Name, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Delete, ItemPath, "?output_mode=json");
	}

	[Fact]
	public async Task DispatchAsync_SendsTheOverrides()
	{
		var stub = TestClient.Stub("""{"sid":"admin__admin__search__RMD5f_at_1791467550_2"}""");
		using var client = TestClient.Create(stub);

		var created = await client.ScheduledViews.DispatchAsync(Name, new ScheduledViewDispatchRequest { DispatchNow = "1791467550", TriggerActions = false, ForceDispatch = true }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, ItemPath + "/dispatch", "?output_mode=json", "dispatch.now=1791467550&trigger_actions=false&force_dispatch=true");
		created.Sid.Should().Be("admin__admin__search__RMD5f_at_1791467550_2");
	}

	[Fact]
	public async Task GetHistoryAsync_SendsGet()
	{
		var stub = TestClient.Stub(SearchRequestAssert.Feed("sid_2", """{"eai:acl":null}"""));
		using var client = TestClient.Create(stub);

		var history = await client.ScheduledViews.GetHistoryAsync(Name, null, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath + "/history", "?output_mode=json");
		history.Entries.Should().ContainSingle().Which.Content!.IsDone.Should().BeFalse();
	}

	[Fact]
	public async Task RescheduleAsync_SendsTheTime()
	{
		var stub = TestClient.Stub("""{"entry":[]}""");
		using var client = TestClient.Create(stub);

		await client.ScheduledViews.RescheduleAsync(Name, new RescheduleRequest { ScheduleTime = "+1h" }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, ItemPath + "/reschedule", "?output_mode=json", "schedule_time=%2B1h");
	}

	[Fact]
	public async Task GetScheduledTimesAsync_SendsTheRange()
	{
		var stub = TestClient.Stub(ViewFeed);
		using var client = TestClient.Create(stub);

		await client.ScheduledViews.GetScheduledTimesAsync(Name, "now", "+3d", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath + "/scheduled_times", "?earliest_time=now&latest_time=%2B3d&output_mode=json");
	}

	[Fact]
	public async Task UpdateAsync_MissingArguments_RaisesBadRequest()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"The following required arguments are missing: cron_schedule, is_scheduled."}]}""", HttpStatusCode.BadRequest));

		await SearchRequestAssert.FailsWith(() => client.ScheduledViews.UpdateAsync(Name, new ScheduledViewUpdateRequest { CronSchedule = string.Empty, IsScheduled = false }, Ct), HttpStatusCode.BadRequest, "The following required arguments are missing: cron_schedule, is_scheduled.");
	}
}
