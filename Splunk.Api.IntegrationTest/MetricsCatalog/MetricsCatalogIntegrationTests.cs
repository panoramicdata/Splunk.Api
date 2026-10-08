using Splunk.Api.IntegrationTest.Search;
using Splunk.Api.Models;
using Splunk.Api.Models.MetricsCatalog;
using System.Net;

namespace Splunk.Api.IntegrationTest.MetricsCatalog;

/// <summary>
/// The metrics catalog and rollup policies, against two throwaway metric indexes the test creates, fills with
/// <c>mcollect</c> and deletes.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class MetricsCatalogIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task CatalogAndRollups_OverThrowawayMetricIndexes()
	{
		var source = SplunkFixture.UniqueName("msrc");
		var target = SplunkFixture.UniqueName("mdst");
		var metric = "splunk_api_it." + Guid.NewGuid().ToString("N")[..8];
		await CreateMetricIndexAsync(source);
		await CreateMetricIndexAsync(target);
		try
		{
			var emitted = await Client.Search.OneshotAsync(
				$"| makeresults count=3 | streamstats count as n | eval metric_name=\"{metric}\", _value=n*10, region=\"eu\" | mcollect index={source}",
				Ct);
			emitted.Messages.Should().Contain(m => m.Text.Contains("3 metric data points", StringComparison.Ordinal));

			var range = new MetricCatalogOptions { Earliest = "-1h", Latest = "now", Filter = "index=" + source };
			var metrics = await WaitForAsync(() => Client.MetricsCatalog.ListMetricsAsync(new MetricListOptions { Earliest = "-1h", Filter = "index=" + source, ListIndexes = true }, Ct));
			metrics.Entries.Should().ContainSingle(e => e.Name == metric).Which.Content!.Indexes.Should().Equal(source);

			var dimensions = await WaitForAsync(() => Client.MetricsCatalog.ListDimensionsAsync(metric, range, Ct));
			dimensions.Entries.Select(e => e.Name).Should().Contain("region");

			var values = await WaitForAsync(() => Client.MetricsCatalog.ListDimensionValuesAsync("region", metric, range, Ct));
			values.Entries.Select(e => e.Name).Should().Equal("eu");

			await RollupRoundTripAsync(source, target, metric);
		}
		finally
		{
			await DeleteIndexAsync(source);
			await DeleteIndexAsync(target);
		}
	}

	private async Task RollupRoundTripAsync(string source, string target, string metric)
	{
		var created = await Client.MetricRollups.CreateAsync(
			new MetricRollupCreateRequest
			{
				Name = source,
				Summaries = "1h|" + target,
				DefaultAggregation = "avg#max",
				MetricList = metric,
				MetricListType = "included",
				MetricOverrides = metric + "|sum"
			},
			Ct);
		try
		{
			var policy = created.Entries.Should().ContainSingle().Subject.Content!;
			policy.Summaries.Values.Should().ContainSingle().Which.RollupIndex.Should().Be(target);
			policy.MetricOverrides.Should().ContainKey(metric).WhoseValue.Should().Be("sum");

			(await Client.MetricRollups.ListAsync(new ListOptions { Count = 0 }, Ct)).Entries.Should().Contain(e => e.Name == source);
			(await Client.MetricRollups.GetAsync(source, Ct)).Entries[0].Content!.DefaultAggregation.Should().Be("avg#max");

			var updated = await Client.MetricRollups.UpdateAsync(source, new MetricRollupUpdateRequest { DefaultAggregation = "min" }, Ct);
			updated.Entries[0].Content!.DefaultAggregation.Should().Be("min");
		}
		finally
		{
			await Client.MetricRollups.DeleteAsync(source, CancellationToken.None);
		}

		var act = () => Client.MetricRollups.GetAsync(source, Ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	/// <summary>The catalog is built in the background: poll until it lists something.</summary>
	private static async Task<SplunkFeed<MetricCatalogItem>> WaitForAsync(Func<Task<SplunkFeed<MetricCatalogItem>>> read)
	{
		for (var attempt = 0; ; attempt++)
		{
			var feed = await read();
			if (feed.Entries.Count > 0 || attempt == 30)
			{
				return feed;
			}

			await Task.Delay(TimeSpan.FromSeconds(2), Ct);
		}
	}

	private Task CreateMetricIndexAsync(string name)
		=> SearchRawRequests.SendAsync(fixture, HttpMethod.Post, "services/data/indexes", new Dictionary<string, string> { ["name"] = name, ["datatype"] = "metric" });

	private Task DeleteIndexAsync(string name)
		=> SearchRawRequests.SendAsync(fixture, HttpMethod.Delete, "services/data/indexes/" + name, null);
}
