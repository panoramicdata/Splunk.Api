using Splunk.Api.Models;
using Splunk.Api.Models.Search;
using System.Net;

namespace Splunk.Api.IntegrationTest.Search;

/// <summary>Alert actions, fired alerts and metric alerts.</summary>
[Collection(SplunkTestGroup.Name)]
public class AlertsIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task AlertActions_IncludeEmail()
	{
		var feed = await Client.AlertActions.ListAsync(new ListOptions { Count = 0 }, Ct);

		feed.Entries.Should().Contain(e => e.Name == "email").Which.Content!.Label.Should().Be("Send email");
	}

	[Fact]
	public async Task FiredAlerts_ScheduledAlertFiresAndItsInstanceIsDeleted()
	{
		var name = SplunkFixture.UniqueName("fa");
		await Client.SavedSearches.CreateAsync(
			new SavedSearchCreateRequest
			{
				Name = name,
				Search = "| makeresults count=1",
				IsScheduled = true,
				CronSchedule = "0 0 1 1 *",
				AlertType = "always",
				AlertTrack = true,
				AlertSeverity = 3,
				Actions = string.Empty
			},
			Ct);
		try
		{
			// Run it through the scheduler now: only scheduler-run instances can be deleted.
			await Client.SavedSearches.RescheduleAsync(name, new RescheduleRequest { ScheduleTime = "+5s" }, Ct);
			var instance = await WaitForInstanceAsync(name);
			instance.Content!.SavedSearchName.Should().Be(name);
			instance.Content.Severity.Should().Be(3);
			instance.Content.TriggerTime.Should().NotBeNull();
			instance.Name.Should().StartWith("scheduler__");

			var summary = await Client.FiredAlerts.ListAsync(new ListOptions { Count = 0 }, Ct);
			summary.Entries.Should().Contain(e => e.Name == name).Which.Content!.TriggeredAlertCount.Should().BeGreaterThanOrEqualTo(1);

			var byAlertName = () => Client.FiredAlerts.DeleteAsync(name, Ct);
			(await byAlertName.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);

			await Client.FiredAlerts.DeleteAsync(instance.Name, Ct);
			(await Client.FiredAlerts.GetAsync(name, Ct)).Entries.Should().NotContain(e => e.Name == instance.Name);
		}
		finally
		{
			await Client.SavedSearches.DeleteAsync(name, CancellationToken.None);
		}
	}

	private async Task<SplunkEntry<FiredAlert>> WaitForInstanceAsync(string name)
	{
		var deadline = DateTime.UtcNow.AddMinutes(3);
		while (true)
		{
			var feed = await Client.FiredAlerts.GetAsync(name, Ct);
			if (feed.Entries.Count > 0)
			{
				return feed.Entries[0];
			}

			DateTime.UtcNow.Should().BeBefore(deadline, "the scheduler should run the rescheduled alert within three minutes");
			await Task.Delay(TimeSpan.FromSeconds(5), Ct);
		}
	}

	[Fact]
	public async Task MetricAlerts_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("ma");
		var created = await Client.MetricAlerts.CreateAsync(
			new MetricAlertCreateRequest
			{
				Name = name,
				Condition = "'avg(splunk_api_it.cpu)' > 99",
				MetricIndexes = "_metrics",
				GroupBy = "host",
				Filter = "host=*",
				Description = "created by Splunk.Api integration tests",
				TriggerMaxTracked = 5
			},
			Ct);
		try
		{
			created.Entries.Should().ContainSingle().Which.Content!.Condition.Should().Be("'avg(splunk_api_it.cpu)' > 99");

			var read = await Client.MetricAlerts.GetAsync(name, Ct);
			var alert = read.Entries.Should().ContainSingle().Subject.Content!;
			alert.MetricIndexes.Should().Be("_metrics");
			alert.GroupBy.Should().Be("host");
			alert.TriggerMaxTracked.Should().Be(5);

			(await Client.MetricAlerts.ListAsync(new ListOptions { Count = 0 }, Ct)).Entries.Should().Contain(e => e.Name == name);

			var updated = await Client.MetricAlerts.UpdateAsync(name, new MetricAlertUpdateRequest { Description = "updated", Disabled = true }, Ct);
			updated.Entries[0].Content!.Description.Should().Be("updated");
			updated.Entries[0].Content!.Disabled.Should().BeTrue();
		}
		finally
		{
			await Client.MetricAlerts.DeleteAsync(name, CancellationToken.None);
		}

		var act = () => Client.MetricAlerts.GetAsync(name, Ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
