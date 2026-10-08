using Splunk.Api.Models;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class IntrospectionTests
{
	private const string Dispatch = "/services/server/introspection/search/dispatch";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Introspection.ListAsync(Calls.Token), HttpMethod.Get, "/services/server/introspection", Calls.JsonQuery, null);

	[Fact]
	public async Task GetIndexerAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Introspection.GetIndexerAsync(Calls.Token), HttpMethod.Get, "/services/server/introspection/indexer", Calls.JsonQuery, null);

	[Fact]
	public async Task ListSearchDispatchAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Introspection.ListSearchDispatchAsync(Calls.Token), HttpMethod.Get, Dispatch, Calls.JsonQuery, null);

	[Fact]
	public async Task GetBundleDirectoryReaperAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Introspection.GetBundleDirectoryReaperAsync(Calls.Token), HttpMethod.Get, Dispatch + "/Bundle_Directory_Reaper", Calls.JsonQuery, null);

	[Fact]
	public async Task GetComputeUserSearchQuotaAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Introspection.GetComputeUserSearchQuotaAsync(Calls.Token), HttpMethod.Get, Dispatch + "/Compute_User_Search_Quota", Calls.JsonQuery, null);

	[Fact]
	public async Task GetDispatchDirectoryReaperAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Introspection.GetDispatchDirectoryReaperAsync(Calls.Token), HttpMethod.Get, Dispatch + "/Dispatch_Directory_Reaper", Calls.JsonQuery, null);

	[Fact]
	public async Task GetSearchStartUpTimeAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Introspection.GetSearchStartUpTimeAsync(Calls.Token), HttpMethod.Get, Dispatch + "/Search_StartUp_Time", Calls.JsonQuery, null);

	[Fact]
	public async Task GetSearchDistributedAsync_SendsGet()
		=> await Calls.AssertAsync(
			c => c.Introspection.GetSearchDistributedAsync(new ListOptions { Count = 5 }, Calls.Token),
			HttpMethod.Get, "/services/server/introspection/search/distributed", "?count=5&output_mode=json", null);

	[Fact]
	public async Task GetSearchSavedAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Introspection.GetSearchSavedAsync(Calls.Token), HttpMethod.Get, "/services/server/introspection/search/saved", Calls.JsonQuery, null);

	[Fact]
	public async Task GetIndexerAsync_MapsEveryField()
	{
		// Captured from Splunk 10.6.0.5.
		var feed = await Calls.MapAsync(
			c => c.Introspection.GetIndexerAsync(Calls.Token),
			Feed.Of("indexer", """{"average_KBps":22.898836366625424,"eai:acl":null,"reason":"","status":"normal"}"""));

		var indexer = feed.Entries.Should().ContainSingle().Subject.Content!;
		indexer.AverageKBps.Should().BeApproximately(22.8988, 0.001);
		indexer.Reason.Should().BeEmpty();
		indexer.Status.Should().Be("normal");
	}

	[Fact]
	public async Task ListSearchDispatchAsync_ReadsTimingsByKeySuffix()
	{
		// Captured from Splunk 10.6.0.5.
		var feed = await Calls.MapAsync(
			c => c.Introspection.ListSearchDispatchAsync(Calls.Token),
			Feed.Of(
				("Search_StartUp_Time", """{"Search_StartUp_Time_Average_Time(ms)":87,"Search_StartUp_Time_Max_Time(ms)":663,"eai:acl":null}"""),
				("Odd", """{"Odd_Average_Time(ms)":"n/a","eai:acl":null}""")));

		feed.Entries[0].Content!.AverageTimeMs.Should().Be(87);
		feed.Entries[0].Content!.MaxTimeMs.Should().Be(663);
		feed.Entries[1].Content!.AverageTimeMs.Should().BeNull();
		feed.Entries[1].Content!.MaxTimeMs.Should().BeNull();
	}

	[Fact]
	public async Task GetSearchDistributedAsync_MapsEveryField()
	{
		// Captured from Splunk 10.6.0.5 (window_metrics), with non-zero values.
		var feed = await Calls.MapAsync(
			c => c.Introspection.GetSearchDistributedAsync(null, Calls.Token),
			Feed.Of("window_metrics", """
				{"average_baseline_file_size":1,"average_baseline_msecs":2,"average_bytes":3,"average_delta_file_size":4,"average_delta_msecs":5,
				"average_msecs":6,"baseline_count":7,"bundle_file_count":8,"delta_count":9,"eai:acl":null}
				"""));

		var metrics = feed.Entries.Should().ContainSingle().Subject.Content!;
		metrics.AverageBaselineFileSize.Should().Be(1);
		metrics.AverageBaselineMsecs.Should().Be(2);
		metrics.AverageBytes.Should().Be(3);
		metrics.AverageDeltaFileSize.Should().Be(4);
		metrics.AverageDeltaMsecs.Should().Be(5);
		metrics.AverageMsecs.Should().Be(6);
		metrics.BaselineCount.Should().Be(7);
		metrics.BundleFileCount.Should().Be(8);
		metrics.DeltaCount.Should().Be(9);
	}

	[Fact]
	public async Task GetIndexerAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.Introspection.GetIndexerAsync(Calls.Token), HttpStatusCode.Forbidden);
}
