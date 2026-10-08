using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ServerStatusTests
{
	private const string Path = "/services/server/status";

	// Captured from Splunk 10.6.0.5 (GET server/status/dispatch-artifacts), trimmed; counts made distinct.
	private const string ArtifactsContent = """
		{
			"adhoc_count": "1", "adhoc_size_mb": "1.5", "completed_count": "2", "completed_size_mb": "2.5", "eai:acl": null,
			"incomple_count": "3", "incomple_size_mb": "3.5", "invalid_count": "4", "remote_count": "5", "rsa_count": "0",
			"scheduled_count": "6", "scheduled_size_mb": "6.5", "temp_dispatch_count": "7",
			"top_apps": [{"search": 1}], "top_named_searches": null, "top_users": null
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ServerStatus.ListAsync(Calls.Token), HttpMethod.Get, Path, Calls.JsonQuery, null);

	[Fact]
	public async Task GetDispatchArtifactsAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ServerStatus.GetDispatchArtifactsAsync(Calls.Token), HttpMethod.Get, Path + "/dispatch-artifacts", Calls.JsonQuery, null);

	[Fact]
	public async Task GetFishbucketAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ServerStatus.GetFishbucketAsync(Calls.Token), HttpMethod.Get, Path + "/fishbucket", Calls.JsonQuery, null);

	[Fact]
	public async Task GetInstalledFileIntegrityAsync_SendsTheOptions()
		=> await Calls.AssertAsync(
			c => c.ServerStatus.GetInstalledFileIntegrityAsync(new FileIntegrityOptions { Refresh = true, RegexFilter = "bin" }, Calls.Token),
			HttpMethod.Get, Path + "/installed-file-integrity", "?refresh=true&regex_filter=bin&output_mode=json", null);

	[Fact]
	public async Task GetSearchConcurrencyLimitsAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ServerStatus.GetSearchConcurrencyLimitsAsync(Calls.Token), HttpMethod.Get, Path + "/limits/search-concurrency", Calls.JsonQuery, null);

	[Fact]
	public async Task ListPartitionsSpaceAsync_SendsGet()
		=> await Calls.AssertAsync(
			c => c.ServerStatus.ListPartitionsSpaceAsync(new ListOptions { Count = 1 }, Calls.Token),
			HttpMethod.Get, Path + "/partitions-space", "?count=1&output_mode=json", null);

	[Fact]
	public async Task GetDispatchArtifactsAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.ServerStatus.GetDispatchArtifactsAsync(Calls.Token), Feed.Of("result", ArtifactsContent));

		var artifacts = feed.Entries.Should().ContainSingle().Subject.Content!;
		artifacts.AdhocCount.Should().Be(1);
		artifacts.AdhocSizeMB.Should().Be(1.5);
		artifacts.CompletedCount.Should().Be(2);
		artifacts.CompletedSizeMB.Should().Be(2.5);
		artifacts.IncompleteCount.Should().Be(3);
		artifacts.IncompleteSizeMB.Should().Be(3.5);
		artifacts.InvalidCount.Should().Be(4);
		artifacts.RemoteCount.Should().Be(5);
		artifacts.ScheduledCount.Should().Be(6);
		artifacts.ScheduledSizeMB.Should().Be(6.5);
		artifacts.TempDispatchCount.Should().Be(7);
		artifacts.TopApps!.Value.GetArrayLength().Should().Be(1);
		artifacts.TopNamedSearches.Should().BeNull();
		artifacts.TopUsers.Should().BeNull();
		artifacts.AdditionalProperties.Should().ContainKey("rsa_count");
	}

	[Fact]
	public async Task OtherStatuses_MapEveryField()
	{
		// Captured from Splunk 10.6.0.5.
		var fishbucket = await Calls.MapAsync(c => c.ServerStatus.GetFishbucketAsync(Calls.Token), Feed.Of("result", """{"eai:acl":null,"key_count":"93","total_size":"0.004"}"""));
		var integrity = await Calls.MapAsync(
			c => c.ServerStatus.GetInstalledFileIntegrityAsync(null, Calls.Token),
			Feed.Of("file-integrity", """{"check_failures":["bin/x"],"check_ready":"true","eai:acl":null}"""));
		var limits = await Calls.MapAsync(
			c => c.ServerStatus.GetSearchConcurrencyLimitsAsync(Calls.Token),
			Feed.Of("search-concurrency", """{"max_auto_summary_searches":5,"max_hist_scheduled_searches":11,"max_hist_searches":22,"max_rt_scheduled_searches":12,"max_rt_searches":23}"""));
		var partitions = await Calls.MapAsync(
			c => c.ServerStatus.ListPartitionsSpaceAsync(null, Calls.Token),
			Feed.Of("1", """{"available":"786877.668","capacity":"1031018.426","free":"839322.465","fs_type":"ext4","mount_point":"/opt/splunk/var"}"""));

		fishbucket.Entries[0].Content!.KeyCount.Should().Be(93);
		fishbucket.Entries[0].Content!.TotalSizeMB.Should().Be(0.004);
		integrity.Entries[0].Content!.CheckReady.Should().BeTrue();
		integrity.Entries[0].Content!.CheckFailures!.Value.GetArrayLength().Should().Be(1);
		var limit = limits.Entries[0].Content!;
		(limit.MaxAutoSummarySearches, limit.MaxHistoricalScheduledSearches, limit.MaxHistoricalSearches, limit.MaxRealTimeScheduledSearches, limit.MaxRealTimeSearches)
			.Should().Be((5, 11, 22, 12, 23));
		var partition = partitions.Entries[0].Content!;
		partition.AvailableMB.Should().Be(786877.668);
		partition.CapacityMB.Should().Be(1031018.426);
		partition.FreeMB.Should().Be(839322.465);
		partition.FileSystemType.Should().Be("ext4");
		partition.MountPoint.Should().Be("/opt/splunk/var");
	}

	[Fact]
	public async Task GetFishbucketAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.ServerStatus.GetFishbucketAsync(Calls.Token), HttpStatusCode.Forbidden);
}
