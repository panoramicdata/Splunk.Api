using Splunk.Api.Models.KvStore;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class KvStoreStatisticsTests
{
	private const string Path = "/servicesNS/nobody/search/storage/collections/stats";

	// Captured from Splunk 10.6.0.5.
	private const string CollectionJson = """
		{"collection":"assets","app":"search","stats":{"record_count":3,"estimated_size_bytes":16384,"index_count":5,"last_updated":"2026-10-08T13:53:50Z"}}
		""";

	private const string FieldsJson = """
		{"collection":"assets","filter":{"n":{"$gt":2}},"stats":{"count":3,"aggregations":{"n":{"count":3,"max":7}},"execution_time_ms":3,"last_updated":"2026-10-08T13:53:50Z"}}
		""";

	private static SplunkClient App(SplunkClient client) => client.InNamespace("nobody", "search");

	[Fact]
	public async Task GetCollectionAsync_SendsTheCollectionInTheQuery()
		=> await Calls.AssertAsync(
			c => App(c).KvStoreStatistics.GetCollectionAsync("assets", Calls.Token),
			HttpMethod.Get, Path, "?collection=assets&output_mode=json", null);

	[Fact]
	public async Task GetFieldsAsync_SendsEveryOption()
	{
		var call = await Calls.RecordAsync(
			c => App(c).KvStoreStatistics.GetFieldsAsync(
				new KvStoreFieldStatisticsOptions { Collection = "assets", Filter = "{}", Fields = ["n", "m"], Aggregations = ["count", "max"] },
				Calls.Token),
			FieldsJson);

		call.Method.Should().Be(HttpMethod.Get);
		call.Uri.AbsolutePath.Should().Be(Path + "/fields");
		Uri.UnescapeDataString(call.Uri.Query).Should().Be("?collection=assets&filter={}&fields=n,m&agg=count,max&output_mode=json");
	}

	[Fact]
	public async Task GetCollectionAsync_MapsEveryField()
	{
		var statistics = await Calls.MapAsync(c => App(c).KvStoreStatistics.GetCollectionAsync("assets", Calls.Token), CollectionJson);

		statistics.Collection.Should().Be("assets");
		statistics.App.Should().Be("search");
		statistics.Stats!.RecordCount.Should().Be(3);
		statistics.Stats.EstimatedSizeBytes.Should().Be(16384);
		statistics.Stats.IndexCount.Should().Be(5);
		statistics.Stats.LastUpdated.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 53, 50, TimeSpan.Zero));
	}

	[Fact]
	public async Task GetFieldsAsync_MapsEveryField()
	{
		var statistics = await Calls.MapAsync(
			c => App(c).KvStoreStatistics.GetFieldsAsync(new KvStoreFieldStatisticsOptions { Collection = "assets", Filter = "{}" }, Calls.Token),
			FieldsJson);

		statistics.Collection.Should().Be("assets");
		statistics.Filter!.Value.GetProperty("n").GetProperty("$gt").GetInt32().Should().Be(2);
		statistics.Stats!.Count.Should().Be(3);
		statistics.Stats.Aggregations["n"]["max"].GetInt32().Should().Be(7);
		statistics.Stats.ExecutionTimeMs.Should().Be(3);
		statistics.Stats.LastUpdated.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 53, 50, TimeSpan.Zero));
	}

	[Fact]
	public async Task GetCollectionAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.KvStoreStatistics.GetCollectionAsync("nope", Calls.Token), HttpStatusCode.Forbidden);
}
