using Splunk.Api.Models;
using Splunk.Api.Models.MetricsCatalog;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class MetricRollupsTests
{
	private const string ItemPath = "/services/catalog/metricstore/rollup/my_metrics";

	// Captured from Splunk Enterprise 10.6.0.5, trimmed.
	private static readonly string PolicyFeed = SearchRequestAssert.Feed("my_metrics", """
		{"aggregation.app.cpu":"sum","aggregation.odd":1,"appName":"search","defaultAggregation":"avg#max","dimensionList":"n","dimensionListType":"excluded","disabled":false,"eai:acl":null,"metricList":"app.cpu","metricListType":"included","minSpanAllowed":300,"summaries":{"0":{"rollupIndex":"my_metrics_1h","span":"1h"},"1":{"rollupIndex":"my_metrics_1d","span":"1d"}}}
		""", "nobody");

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetWithPaging()
	{
		var stub = TestClient.Stub(PolicyFeed);
		using var client = TestClient.Create(stub);

		await client.MetricRollups.ListAsync(new ListOptions { Count = 0 }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/catalog/metricstore/rollup", "?count=0&output_mode=json");
	}

	[Fact]
	public async Task CreateAsync_SendsEverySettingAsForm()
	{
		var stub = TestClient.Stub(PolicyFeed);
		using var client = TestClient.Create(stub);

		await client.MetricRollups.CreateAsync(
			new MetricRollupCreateRequest
			{
				Name = "my_metrics",
				Summaries = "1h|my_metrics_1h,1d|my_metrics_1d",
				DefaultAggregation = "avg#max",
				MetricList = "app.cpu",
				MetricListType = "included",
				DimensionList = "n",
				DimensionListType = "excluded",
				MetricOverrides = "app.cpu|sum"
			},
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Post,
			"/services/catalog/metricstore/rollup",
			"?output_mode=json",
			"name=my_metrics&summaries=1h%7Cmy_metrics_1h%2C1d%7Cmy_metrics_1d&default_agg=avg%23max&metric_list=app.cpu&metric_list_type=included"
				+ "&dimension_list=n&dimension_list_type=excluded&metric_overrides=app.cpu%7Csum");
	}

	[Fact]
	public async Task GetAsync_SendsGetAndMapsThePolicy()
	{
		var stub = TestClient.Stub(PolicyFeed);
		using var client = TestClient.Create(stub);

		var policy = (await client.MetricRollups.GetAsync("my_metrics", Ct)).Entries.Should().ContainSingle().Subject.Content!;

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath, "?output_mode=json");
		policy.DefaultAggregation.Should().Be("avg#max");
		policy.MetricList.Should().Be("app.cpu");
		policy.MetricListType.Should().Be("included");
		policy.DimensionList.Should().Be("n");
		policy.DimensionListType.Should().Be("excluded");
		policy.MinSpanAllowed.Should().Be(300);
		policy.AppName.Should().Be("search");
		policy.Summaries.Should().HaveCount(2);
		policy.Summaries["1"].RollupIndex.Should().Be("my_metrics_1d");
		policy.Summaries["1"].Span.Should().Be("1d");
		policy.MetricOverrides.Should().Equal(new Dictionary<string, string> { ["app.cpu"] = "sum" });
	}

	[Fact]
	public async Task UpdateAsync_SendsOnlyWhatIsSet()
	{
		var stub = TestClient.Stub(PolicyFeed);
		using var client = TestClient.Create(stub);

		await client.MetricRollups.UpdateAsync("my_metrics", new MetricRollupUpdateRequest { DefaultAggregation = "min", Summaries = "1h|my_metrics_1h" }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, ItemPath, "?output_mode=json", "default_agg=min&summaries=1h%7Cmy_metrics_1h");
	}

	[Fact]
	public async Task DeleteAsync_SendsDelete()
	{
		var stub = TestClient.Stub("""{"entry":[]}""");
		using var client = TestClient.Create(stub);

		await client.MetricRollups.DeleteAsync("my_metrics", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Delete, ItemPath, "?output_mode=json");
	}

	[Fact]
	public async Task CreateAsync_Error_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Index 'nope' is not a metric index."}]}""", HttpStatusCode.BadRequest));

		await SearchRequestAssert.FailsWith(
			() => client.MetricRollups.CreateAsync(new MetricRollupCreateRequest { Name = "nope", Summaries = "1h|x" }, Ct),
			HttpStatusCode.BadRequest,
			"Index 'nope' is not a metric index.");
	}
}
