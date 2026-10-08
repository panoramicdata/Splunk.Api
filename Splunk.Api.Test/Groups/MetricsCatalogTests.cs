using Splunk.Api.Models.MetricsCatalog;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class MetricsCatalogTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (catalog/metricstore/metrics?list_indexes=true), trimmed.
	private static readonly string MetricFeed = SearchRequestAssert.Feed("app.cpu", """{"eai:acl":null,"index":["my_metrics"]}""", "system");
	private static readonly string DimensionFeed = SearchRequestAssert.Feed("host", """{"eai:acl":null}""", "system");

	private static readonly MetricCatalogOptions Range = new() { Earliest = "-1h", Latest = "now", Filter = "index=my_metrics", Count = 10 };

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListMetricsAsync_SendsGetWithEveryOption()
	{
		var stub = TestClient.Stub(MetricFeed);
		using var client = TestClient.Create(stub);

		await client.MetricsCatalog.ListMetricsAsync(new MetricListOptions { Earliest = "-1h", Latest = "now", Filter = "index=my_metrics", ListIndexes = true, Count = 10 }, Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Get,
			"/services/catalog/metricstore/metrics",
			"?list_indexes=true&earliest=-1h&latest=now&filter=index%3Dmy_metrics&count=10&output_mode=json");
	}

	[Fact]
	public async Task ListMetricsAsync_MapsTheMetricAndItsIndexes()
	{
		using var client = TestClient.Create(TestClient.Stub(MetricFeed));

		var entry = (await client.MetricsCatalog.ListMetricsAsync(null, Ct)).Entries.Should().ContainSingle().Subject;

		entry.Name.Should().Be("app.cpu");
		entry.Content!.Indexes.Should().Equal("my_metrics");
	}

	[Fact]
	public async Task ListDimensionsAsync_SendsTheMetricName()
	{
		var stub = TestClient.Stub(DimensionFeed);
		using var client = TestClient.Create(stub);

		var feed = await client.MetricsCatalog.ListDimensionsAsync("app.*", Range, Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Get,
			"/services/catalog/metricstore/dimensions",
			"?metric_name=app.%2A&earliest=-1h&latest=now&filter=index%3Dmy_metrics&count=10&output_mode=json");
		feed.Entries[0].Content!.Indexes.Should().BeEmpty();
	}

	[Fact]
	public async Task ListDimensionValuesAsync_SendsTheDimensionAndMetric()
	{
		var stub = TestClient.Stub(SearchRequestAssert.Feed("web01", """{"eai:acl":null}""", "system"));
		using var client = TestClient.Create(stub);

		var feed = await client.MetricsCatalog.ListDimensionValuesAsync("host", "*", null, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/catalog/metricstore/dimensions/host/values", "?metric_name=%2A&output_mode=json");
		feed.Entries[0].Name.Should().Be("web01");
	}

	[Fact]
	public async Task ListDimensionsAsync_Error_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"metric_name is required."}]}""", HttpStatusCode.BadRequest));

		await SearchRequestAssert.FailsWith(() => client.MetricsCatalog.ListDimensionsAsync(string.Empty, null, Ct), HttpStatusCode.BadRequest, "metric_name is required.");
	}
}
