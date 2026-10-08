using Splunk.Api.Models;
using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class MetricAlertsTests
{
	private const string ItemPath = "/services/alerts/metric_alerts/cpu_high";
	private static readonly string AlertFeed = SearchRequestAssert.Feed("cpu_high", SavedSearchJson.MetricAlertContent);

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetWithPaging()
	{
		var stub = TestClient.Stub(AlertFeed);
		using var client = TestClient.Create(stub);

		await client.MetricAlerts.ListAsync(new ListOptions { Offset = 1 }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/alerts/metric_alerts", "?offset=1&output_mode=json");
	}

	[Fact]
	public async Task CreateAsync_SendsEveryTypedSettingAsForm()
	{
		var stub = TestClient.Stub(AlertFeed);
		using var client = TestClient.Create(stub);

		await client.MetricAlerts.CreateAsync(
			new MetricAlertCreateRequest
			{
				Name = "cpu_high",
				Condition = "'avg(cpu.usage)' > 99",
				MetricIndexes = "_metrics",
				GroupBy = "host",
				Filter = "host=*",
				Description = "d",
				Disabled = false,
				TriggerExpires = "24h",
				TriggerMaxTracked = 5,
				TriggerSuppress = "1h",
				AdditionalParameters = { ["action.logevent"] = "1" }
			},
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Post,
			"/services/alerts/metric_alerts",
			"?output_mode=json",
			"name=cpu_high&condition=%27avg%28cpu.usage%29%27+%3E+99&metric_indexes=_metrics&groupby=host&filter=host%3D%2A&description=d"
				+ "&disabled=false&trigger.expires=24h&trigger.max_tracked=5&trigger.suppress=1h&action.logevent=1");
	}

	[Fact]
	public async Task GetAsync_SendsGetAndMapsTheAlert()
	{
		var stub = TestClient.Stub(AlertFeed);
		using var client = TestClient.Create(stub);

		var alert = (await client.MetricAlerts.GetAsync("cpu_high", Ct)).Entries.Should().ContainSingle().Subject.Content!;

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath, "?output_mode=json");
		alert.Condition.Should().Be("'avg(cpu.usage)' > 99");
		alert.MetricIndexes.Should().Be("_metrics");
		alert.GroupBy.Should().Be("host");
		alert.Filter.Should().Be("host=*");
		alert.Description.Should().Be("probe");
		alert.TriggerExpires.Should().Be("24h");
		alert.TriggerMaxTracked.Should().Be(5);
		alert.TriggerSuppress.Should().BeEmpty();
		alert.GroupKey.Should().Be("streamalert_556fd83e401db47b");
		alert.Disabled.Should().BeFalse();
		alert.AdditionalProperties.Should().ContainKey("action.logevent");
	}

	[Fact]
	public async Task UpdateAsync_SendsOnlyWhatIsSet()
	{
		var stub = TestClient.Stub(AlertFeed);
		using var client = TestClient.Create(stub);

		await client.MetricAlerts.UpdateAsync("cpu_high", new MetricAlertUpdateRequest { Condition = "'avg(cpu.usage)' > 98", MetricIndexes = "_metrics", Disabled = true }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, ItemPath, "?output_mode=json", "disabled=true&condition=%27avg%28cpu.usage%29%27+%3E+98&metric_indexes=_metrics");
	}

	[Fact]
	public async Task DeleteAsync_SendsDelete()
	{
		var stub = TestClient.Stub("""{"entry":[]}""");
		using var client = TestClient.Create(stub);

		await client.MetricAlerts.DeleteAsync("cpu_high", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Delete, ItemPath, "?output_mode=json");
	}

	[Fact]
	public async Task CreateAsync_BadCondition_RaisesBadRequest()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Failed to parse metric alert condition."}]}""", HttpStatusCode.BadRequest));

		await SearchRequestAssert.FailsWith(() => client.MetricAlerts.CreateAsync(new MetricAlertCreateRequest { Name = "x", Condition = "?", MetricIndexes = "_metrics" }, Ct), HttpStatusCode.BadRequest, "Failed to parse metric alert condition.");
	}
}
