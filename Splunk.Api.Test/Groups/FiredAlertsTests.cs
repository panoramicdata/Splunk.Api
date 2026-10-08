using Splunk.Api.Models;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class FiredAlertsTests
{
	private const string Instance = "scheduler__admin__search__RMD5_at_1791467606_3_1791467606";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetAndMapsTheSummary()
	{
		var stub = TestClient.Stub(SearchRequestAssert.Feed("my_alert", """{"eai:acl":null,"is_streaming_alert":false,"triggered_alert_count":"2"}"""));
		using var client = TestClient.Create(stub);

		var feed = await client.FiredAlerts.ListAsync(new ListOptions { Count = 0 }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/alerts/fired_alerts", "?count=0&output_mode=json");
		var summary = feed.Entries.Should().ContainSingle().Subject.Content!;
		summary.TriggeredAlertCount.Should().Be(2);
		summary.IsStreamingAlert.Should().BeFalse();
	}

	[Fact]
	public async Task GetAsync_SendsGet()
	{
		var stub = TestClient.Stub(SearchRequestAssert.Feed(Instance, SavedSearchJson.FiredAlertContent));
		using var client = TestClient.Create(stub);

		await client.FiredAlerts.GetAsync("my_alert", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/alerts/fired_alerts/my_alert", "?output_mode=json");
	}

	[Fact]
	public async Task GetAsync_MapsTheTriggeredInstances()
	{
		using var client = TestClient.Create(TestClient.Stub(SearchRequestAssert.Feed(Instance, SavedSearchJson.FiredAlertContent)));

		var entry = (await client.FiredAlerts.GetAsync("my_alert", Ct)).Entries.Should().ContainSingle().Subject;

		entry.Name.Should().Be(Instance);
		var fired = entry.Content!;
		fired.SavedSearchName.Should().Be("my_alert");
		fired.Sid.Should().Be("scheduler__admin__search__RMD5_at_1791467606_3");
		fired.TriggerTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467606));
		fired.TriggerTimeRendered.Should().Be("2026-10-08 13:53:26 UTC");
		fired.ExpirationTimeRendered.Should().Be("2026-10-09 13:53:26 UTC");
		fired.AlertType.Should().Be("historical");
		fired.DigestMode.Should().BeTrue();
		fired.Severity.Should().Be(3);
		fired.TriggeredAlerts.Should().Be(1);
		fired.Actions.Should().BeNull();
	}

	[Fact]
	public async Task DeleteAsync_SendsDeleteToTheInstance()
	{
		var stub = TestClient.Stub("""{"entry":[]}""");
		using var client = TestClient.Create(stub);

		await client.FiredAlerts.DeleteAsync(Instance, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Delete, "/services/alerts/fired_alerts/" + Instance, "?output_mode=json");
	}

	[Fact]
	public async Task DeleteAsync_AlertName_RaisesBadRequest()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Unexpected pattern for alert_id=my_alert"}]}""", HttpStatusCode.BadRequest));

		await SearchRequestAssert.FailsWith(() => client.FiredAlerts.DeleteAsync("my_alert", Ct), HttpStatusCode.BadRequest, "Unexpected pattern for alert_id=my_alert");
	}
}
