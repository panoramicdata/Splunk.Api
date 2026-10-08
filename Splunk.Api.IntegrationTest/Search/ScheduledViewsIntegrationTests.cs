using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.IntegrationTest.Search;

[Collection(SplunkTestGroup.Name)]
public class ScheduledViewsIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task RoundTrip_ScheduleDispatchAndUnschedule()
	{
		var view = SplunkFixture.UniqueName("view");
		var name = "_ScheduledView__" + view;
		await SearchRawRequests.SendAsync(fixture, HttpMethod.Post, "services/data/ui/views", new Dictionary<string, string>
		{
			["name"] = view,
			["eai:data"] = "<dashboard><label>Splunk.Api integration test</label></dashboard>"
		});
		try
		{
			var updated = await Client.ScheduledViews.UpdateAsync(
				name,
				new ScheduledViewUpdateRequest { CronSchedule = "0 3 * * *", IsScheduled = true, ActionEmailTo = "nobody@example.com", Description = "integration test" },
				Ct);
			var scheduled = updated.Entries.Should().ContainSingle().Subject.Content!;
			scheduled.IsScheduled.Should().BeTrue();
			scheduled.ActionEmailTo.Should().Be("nobody@example.com");
			scheduled.ActionEmailPdfView.Should().Be(view);

			(await Client.ScheduledViews.GetAsync(name, Ct)).Entries[0].Content!.CronSchedule.Should().Be("0 3 * * *");
			(await Client.ScheduledViews.ListAsync(new ListOptions { Count = 0 }, Ct)).Entries.Should().Contain(e => e.Name == name);

			var times = await Client.ScheduledViews.GetScheduledTimesAsync(name, "now", "+3d", Ct);
			times.Entries.Should().ContainSingle().Which.Content!.CronSchedule.Should().Be("0 3 * * *");
			times.Entries[0].Content!.ScheduledTimes.Should().BeEmpty("Splunk 10.6 does not list a scheduled view's times");

			await Client.ScheduledViews.RescheduleAsync(name, new RescheduleRequest { ScheduleTime = "+1h" }, Ct);

			var dispatched = await Client.ScheduledViews.DispatchAsync(name, new ScheduledViewDispatchRequest { TriggerActions = false }, Ct);
			dispatched.Sid.Should().NotBeNullOrEmpty();
			// The job appears in the view's history once it has started.
			await Client.Search.WaitForCompletionAsync(dispatched.Sid, new SearchWaitOptions { PollInterval = TimeSpan.FromMilliseconds(200), Timeout = TimeSpan.FromMinutes(2) }, Ct);
			var history = await Client.ScheduledViews.GetHistoryAsync(name, null, Ct);
			history.Entries.Should().Contain(e => e.Name == dispatched.Sid);
			await Client.SearchJobs.DeleteAsync(dispatched.Sid, CancellationToken.None);

			await Client.ScheduledViews.DeleteAsync(name, Ct);
			(await Client.ScheduledViews.GetAsync(name, Ct)).Entries[0].Content!.IsScheduled.Should().BeFalse();
		}
		finally
		{
			await SearchRawRequests.SendAsync(fixture, HttpMethod.Delete, "services/data/ui/views/" + view, null);
		}
	}
}
