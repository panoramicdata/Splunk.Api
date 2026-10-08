using Splunk.Api.Models.Introspection;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class IndexesTests
{
	// Captured from Splunk 10.6.0.5 (GET data/indexes/main), trimmed.
	internal const string MainContent = """
		{
			"assureUTF8": false,
			"coldPath": "$SPLUNK_DB/defaultdb/colddb",
			"coldPath_expanded": "/opt/splunk/var/lib/splunk/defaultdb/colddb",
			"coldToFrozenDir": "",
			"coldToFrozenScript": "",
			"compressRawdata": true,
			"currentDBSizeMB": "1",
			"datatype": "event",
			"defaultDatabase": "main",
			"disabled": false,
			"eai:acl": null,
			"enableDataIntegrityControl": false,
			"enableOnlineBucketRepair": true,
			"frozenTimePeriodInSecs": 188697600,
			"homePath": "$SPLUNK_DB/defaultdb/db",
			"homePath_expanded": "/opt/splunk/var/lib/splunk/defaultdb/db",
			"isInternal": false,
			"isReady": true,
			"isVirtual": false,
			"journalCompression": "zstd",
			"lastInitTime": 1791465098,
			"maxDataSize": "auto_high_volume",
			"maxHotBuckets": "10",
			"maxHotIdleSecs": 86400,
			"maxHotSpanSecs": 7776000,
			"maxTime": "2026-10-08T13:11:38+0000",
			"maxTotalDataSizeMB": 500000,
			"maxWarmDBCount": 300,
			"minTime": "",
			"repFactor": 0,
			"thawedPath": "$SPLUNK_DB/defaultdb/thaweddb",
			"thawedPath_expanded": "/opt/splunk/var/lib/splunk/defaultdb/thaweddb",
			"totalEventCount": 42,
			"tstatsHomePath": "volume:_splunk_summaries/defaultdb/datamodel_summary"
		}
		""";

	private const string Path = "/services/data/indexes";

	[Fact]
	public async Task ListAsync_SendsTheDataType()
		=> await Calls.AssertAsync(
			c => c.Indexes.ListAsync(new IndexListOptions { DataType = IndexDataType.All, Count = 0 }, Calls.Token),
			HttpMethod.Get, Path, "?datatype=all&count=0&output_mode=json", null);

	[Fact]
	public async Task CreateAsync_PostsTheNameAndSettings()
		=> await Calls.AssertAsync(
			c => c.Indexes.CreateAsync(
				new IndexCreateRequest
				{
					Name = "web",
					DataType = IndexDataType.Metric,
					HomePath = "$SPLUNK_DB/web/db",
					ColdPath = "$SPLUNK_DB/web/colddb",
					ThawedPath = "$SPLUNK_DB/web/thaweddb",
					MaxTotalDataSizeMB = 100,
					FrozenTimePeriodInSecs = 86400
				},
				Calls.Token),
			HttpMethod.Post, Path, Calls.JsonQuery,
			"name=web&datatype=metric&homePath=%24SPLUNK_DB%2Fweb%2Fdb&coldPath=%24SPLUNK_DB%2Fweb%2Fcolddb&thawedPath=%24SPLUNK_DB%2Fweb%2Fthaweddb"
			+ "&frozenTimePeriodInSecs=86400&maxTotalDataSizeMB=100");

	[Fact]
	public async Task GetAsync_SendsSummarize()
		=> await Calls.AssertAsync(
			c => c.Indexes.GetAsync("main", new IndexGetOptions { Summarize = true }, Calls.Token),
			HttpMethod.Get, Path + "/main", "?summarize=true&output_mode=json", null);

	[Fact]
	public async Task UpdateAsync_PostsEverySetting()
		=> await Calls.AssertAsync(
			c => c.Indexes.UpdateAsync(
				"web",
				new IndexUpdateRequest
				{
					BlockSignSize = 0,
					BucketRebuildMemoryHint = "auto",
					ColdToFrozenDir = "/frozen",
					ColdToFrozenScript = "a.sh",
					EnableOnlineBucketRepair = true,
					FrozenTimePeriodInSecs = 1,
					MaxBloomBackfillBucketAge = "30d",
					MaxConcurrentOptimizes = 2,
					MaxDataSize = "auto",
					MaxHotBuckets = "auto",
					MaxHotIdleSecs = 3,
					MaxHotSpanSecs = 4,
					MaxMemMB = 5,
					MaxMetaEntries = 6,
					MaxTimeUnreplicatedNoAcks = 7,
					MaxTimeUnreplicatedWithAcks = 8,
					MaxTotalDataSizeMB = 9,
					MaxWarmDBCount = 10,
					MinRawFileSyncSecs = "disable",
					MinStreamGroupQueueSize = 11,
					PartialServiceMetaPeriod = 12,
					ProcessTrackerServiceInterval = 13,
					QuarantineFutureSecs = 14,
					QuarantinePastSecs = 15,
					RawChunkSizeBytes = 16,
					RepFactor = "auto",
					RotatePeriodInSecs = 17,
					ServiceMetaPeriod = 18,
					SyncMeta = false,
					ThrottleCheckPeriod = 19,
					TstatsHomePath = "volume:x"
				},
				Calls.Token),
			HttpMethod.Post, Path + "/web", Calls.JsonQuery,
			"blockSignSize=0&bucketRebuildMemoryHint=auto&coldToFrozenDir=%2Ffrozen&coldToFrozenScript=a.sh&enableOnlineBucketRepair=true"
			+ "&frozenTimePeriodInSecs=1&maxBloomBackfillBucketAge=30d&maxConcurrentOptimizes=2&maxDataSize=auto&maxHotBuckets=auto"
			+ "&maxHotIdleSecs=3&maxHotSpanSecs=4&maxMemMB=5&maxMetaEntries=6&maxTimeUnreplicatedNoAcks=7&maxTimeUnreplicatedWithAcks=8"
			+ "&maxTotalDataSizeMB=9&maxWarmDBCount=10&minRawFileSyncSecs=disable&minStreamGroupQueueSize=11&partialServiceMetaPeriod=12"
			+ "&processTrackerServiceInterval=13&quarantineFutureSecs=14&quarantinePastSecs=15&rawChunkSizeBytes=16&repFactor=auto"
			+ "&rotatePeriodInSecs=17&serviceMetaPeriod=18&syncMeta=false&throttleCheckPeriod=19&tstatsHomePath=volume%3Ax");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> await Calls.AssertAsync(c => c.Indexes.DeleteAsync("web", Calls.Token), HttpMethod.Delete, Path + "/web", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.Indexes.GetAsync("main", null, Calls.Token), Feed.Of("main", MainContent));

		AssertMain(feed.Entries.Should().ContainSingle().Subject.Content!);
	}

	[Fact]
	public async Task CreateAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.Indexes.CreateAsync(new IndexCreateRequest { Name = "_bad" }, Calls.Token), HttpStatusCode.BadRequest);

	internal static void AssertMain(SplunkIndex index)
	{
		index.DataType.Should().Be(IndexDataType.Event);
		index.HomePath.Should().Be("$SPLUNK_DB/defaultdb/db");
		index.HomePathExpanded.Should().Be("/opt/splunk/var/lib/splunk/defaultdb/db");
		index.ColdPath.Should().Be("$SPLUNK_DB/defaultdb/colddb");
		index.ColdPathExpanded.Should().Be("/opt/splunk/var/lib/splunk/defaultdb/colddb");
		index.ThawedPath.Should().Be("$SPLUNK_DB/defaultdb/thaweddb");
		index.ThawedPathExpanded.Should().Be("/opt/splunk/var/lib/splunk/defaultdb/thaweddb");
		index.TstatsHomePath.Should().Be("volume:_splunk_summaries/defaultdb/datamodel_summary");
		index.ColdToFrozenDir.Should().BeEmpty();
		index.ColdToFrozenScript.Should().BeEmpty();
		index.CurrentDBSizeMB.Should().Be(1);
		index.MaxTotalDataSizeMB.Should().Be(500000);
		index.TotalEventCount.Should().Be(42);
		index.FrozenTimePeriodInSecs.Should().Be(188697600);
		index.MaxDataSize.Should().Be("auto_high_volume");
		index.MaxHotBuckets.Should().Be("10");
		index.MaxWarmDBCount.Should().Be(300);
		index.MaxHotSpanSecs.Should().Be(7776000);
		index.MaxHotIdleSecs.Should().Be(86400);
		index.MinTime.Should().BeEmpty();
		index.MaxTime.Should().Be("2026-10-08T13:11:38+0000");
		index.IsInternal.Should().BeFalse();
		index.IsReady.Should().BeTrue();
		index.IsVirtual.Should().BeFalse();
		index.LastInitTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791465098));
		index.CompressRawdata.Should().BeTrue();
		index.EnableOnlineBucketRepair.Should().BeTrue();
		index.EnableDataIntegrityControl.Should().BeFalse();
		index.RepFactor.Should().Be("0");
		index.DefaultDatabase.Should().Be("main");
		index.JournalCompression.Should().Be("zstd");
		index.AdditionalProperties.Should().ContainKey("assureUTF8");
	}
}
