using Splunk.Api.Models.Search;
using System.Net;

namespace Splunk.Api.IntegrationTest.Search;

[Collection(SplunkTestGroup.Name)]
public class SavedSearchesIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task RoundTrip_CreateReadUpdateDispatchAndDelete()
	{
		var name = SplunkFixture.UniqueName("ss");
		var created = await Client.SavedSearches.CreateAsync(NewSavedSearch(name), Ct);
		try
		{
			await ReadAndUpdateAsync(name, created.Entries.Should().ContainSingle().Subject.Content!);
			await RescheduleAsync(name);
			await DispatchAndCheckAlertStateAsync(name);
		}
		finally
		{
			await Client.SavedSearches.DeleteAsync(name, CancellationToken.None);
		}

		var act = () => Client.SavedSearches.GetAsync(name, null, Ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	private static SavedSearchCreateRequest NewSavedSearch(string name)
		=> new()
		{
			Name = name,
			Search = "| makeresults count=3",
			Description = "created by Splunk.Api integration tests",
			IsScheduled = true,
			CronSchedule = "0 0 1 1 *",
			DispatchEarliestTime = "-15m",
			DispatchLatestTime = "now",
			AlertType = "number of events",
			AlertComparator = "greater than",
			AlertThreshold = "0",
			AlertSeverity = 4,
			AlertSuppress = true,
			AlertSuppressPeriod = "1h",
			AlertTrack = false,
			Actions = string.Empty
		};

	private async Task ReadAndUpdateAsync(string name, SavedSearch saved)
	{
		saved.Search.Should().Be("| makeresults count=3");
		saved.IsScheduled.Should().BeTrue();
		saved.AlertSeverity.Should().Be(4);
		saved.AlertSuppressPeriod.Should().Be("1h");

		var listed = await Client.SavedSearches.ListAsync(new SavedSearchListOptions { Search = name, Count = 0 }, Ct);
		listed.Entries.Should().ContainSingle(e => e.Name == name);

		var read = await Client.SavedSearches.GetAsync(name, new SavedSearchListOptions { ListDefaultActionArgs = false }, Ct);
		read.Entries[0].Content!.CronSchedule.Should().Be("0 0 1 1 *");

		var updated = await Client.SavedSearches.UpdateAsync(name, new SavedSearchUpdateRequest { Description = "updated", Search = "| makeresults count=2" }, Ct);
		updated.Entries[0].Content!.Description.Should().Be("updated");
	}

	private async Task RescheduleAsync(string name)
	{
		var times = await Client.SavedSearches.GetScheduledTimesAsync(name, "now", "+2y", Ct);
		times.Entries[0].Content!.ScheduledTimes.Should().NotBeEmpty();

		await Client.SavedSearches.RescheduleAsync(name, RescheduleRequest.At(DateTimeOffset.UtcNow.AddDays(1)), Ct);
		var rescheduled = await Client.SavedSearches.GetAsync(name, null, Ct);
		rescheduled.Entries[0].Content!.NextScheduledTime.Should().NotBeNullOrEmpty();
	}

	private async Task DispatchAndCheckAlertStateAsync(string name)
	{
		var dispatched = await Client.SavedSearches.DispatchAsync(name, new SavedSearchDispatchRequest { TriggerActions = false, ForceDispatch = true }, Ct);
		var job = await Client.Search.WaitForCompletionAsync(dispatched.Sid, new SearchWaitOptions { PollInterval = TimeSpan.FromMilliseconds(200), Timeout = TimeSpan.FromMinutes(2) }, Ct);
		job.ResultCount.Should().Be(2);

		var history = await Client.SavedSearches.GetHistoryAsync(name, null, Ct);
		history.Entries.Should().Contain(e => e.Name == dispatched.Sid).Which.Content!.IsDone.Should().BeTrue();

		var suppression = await Client.SavedSearches.GetSuppressionAsync(name, null, Ct);
		suppression.Entries.Should().ContainSingle().Which.Content!.Suppressed.Should().BeFalse();

		var acknowledged = await Client.SavedSearches.AcknowledgeAsync(name, new SavedSearchAcknowledgeRequest { Key = string.Empty }, Ct);
		acknowledged.Entries[0].Content!.SuppressionKey.Should().Contain(name);
	}

	[Fact]
	public async Task Reschedule_IsoTimeWithColon_IsRejected()
	{
		var name = SplunkFixture.UniqueName("ss_rs");
		await Client.SavedSearches.CreateAsync(new SavedSearchCreateRequest { Name = name, Search = "| makeresults", IsScheduled = true, CronSchedule = "0 0 1 1 *" }, Ct);
		try
		{
			var act = () => Client.SavedSearches.RescheduleAsync(name, new RescheduleRequest { ScheduleTime = "2030-01-01T00:00:00.000+00:00" }, Ct);

			var thrown = await act.Should().ThrowAsync<SplunkApiException>();
			thrown.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			thrown.Which.Message.Should().Be("Invalid schedule_time format");
		}
		finally
		{
			await Client.SavedSearches.DeleteAsync(name, CancellationToken.None);
		}
	}
}
