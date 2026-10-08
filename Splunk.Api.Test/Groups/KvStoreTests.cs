using Splunk.Api.Models.KvStore;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class KvStoreTests
{
	// Captured from Splunk 10.6.0.5 (standalone; members added as the reference documents them for a cluster).
	private const string StatusContent = """
		{
			"cohosted": { "serviceInfo": { "service": "https://127.0.0.1:43729/", "type": "Pdl" }, "status": "ready" },
			"current": {
				"backupRestoreStatus": "Ready",
				"disabled": false,
				"guid": "00000000-0000-0000-0000-000000000001",
				"migrationStatus": "NotStarted",
				"port": 8191,
				"replicaSet": "splunkrs",
				"replicationStatus": "KV store captain",
				"standalone": true,
				"status": "ready",
				"storageEngine": "wiredTiger",
				"versionUpgradeInProgress": "0",
				"oplogEndTimestamp": "Thu Oct  8 13:11:38 2026"
			},
			"eai:acl": null,
			"members": { "member1": { "hostAndPort": "splunk01:8191" } }
		}
		""";

	[Fact]
	public async Task GetStatusAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.KvStore.GetStatusAsync(Calls.Token), HttpMethod.Get, "/services/kvstore/status", Calls.JsonQuery, null);

	[Fact]
	public async Task CreateBackupAsync_PostsEveryField()
		=> await Calls.AssertAsync(
			c => c.KvStore.CreateBackupAsync(
				new KvStoreBackupRequest { ArchiveName = "nightly", AppName = "search", CollectionName = "assets", PointInTime = false, Cancel = false, ParallelCollections = 2 },
				Calls.Token),
			HttpMethod.Post, "/services/kvstore/backup/create", Calls.JsonQuery,
			"archiveName=nightly&appName=search&collectionName=assets&pointInTime=false&cancel=false&parallelCollections=2");

	[Fact]
	public async Task RestoreBackupAsync_PostsEveryField()
		=> await Calls.AssertAsync(
			c => c.KvStore.RestoreBackupAsync(
				new KvStoreRestoreRequest { ArchiveName = "nightly", PointInTime = true, ParallelCollections = 4, InsertionsWorkersPerCollection = 3 },
				Calls.Token),
			HttpMethod.Post, "/services/kvstore/backup/restore", Calls.JsonQuery,
			"archiveName=nightly&pointInTime=true&parallelCollections=4&insertionsWorkersPerCollection=3");

	[Fact]
	public async Task SetMaintenanceModeAsync_PostsTheMode()
		=> await Calls.AssertAsync(
			c => c.KvStore.SetMaintenanceModeAsync(new KvStoreMaintenanceRequest { Mode = true }, Calls.Token),
			HttpMethod.Post, "/services/kvstore/control/maintenance", Calls.JsonQuery, "mode=true");

	[Fact]
	public async Task GetStatusAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.KvStore.GetStatusAsync(Calls.Token), Feed.Of("status", StatusContent));

		var status = feed.Entries.Should().ContainSingle().Subject.Content!;
		status.Cohosted!.Status.Should().Be("ready");
		status.Cohosted.ServiceInfo!.Service.Should().Be("https://127.0.0.1:43729/");
		status.Cohosted.ServiceInfo.Type.Should().Be("Pdl");
		var current = status.Current!;
		current.BackupRestoreStatus.Should().Be("Ready");
		current.Disabled.Should().BeFalse();
		current.MemberGuid.Should().Be("00000000-0000-0000-0000-000000000001");
		current.MigrationStatus.Should().Be("NotStarted");
		current.Port.Should().Be(8191);
		current.ReplicaSet.Should().Be("splunkrs");
		current.ReplicationStatus.Should().Be("KV store captain");
		current.Standalone.Should().BeTrue();
		current.Status.Should().Be("ready");
		current.StorageEngine.Should().Be("wiredTiger");
		current.VersionUpgradeInProgress.Should().BeFalse();
		current.AdditionalProperties.Should().ContainKey("oplogEndTimestamp");
		status.Members!["member1"].GetProperty("hostAndPort").GetString().Should().Be("splunk01:8191");
	}

	[Fact]
	public async Task GetStatusAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.KvStore.GetStatusAsync(Calls.Token), HttpStatusCode.ServiceUnavailable);
}
