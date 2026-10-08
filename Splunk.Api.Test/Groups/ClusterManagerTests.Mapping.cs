using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

// Representative content built from the reference's examples: a standalone test instance has no cluster manager.
public partial class ClusterManagerTests
{
	private const string Bundle = """{ "bundle_path": "/opt/splunk/var/run/splunk/cluster/remote-bundle/0143dd2c-1758530970.bundle", "checksum": "0B353BAF", "timestamp": "1758530970" }""";

	private static readonly string InfoContent = $$"""
		{
			"active_bundle": {{Bundle}},
			"apply_bundle_status": {
				"invalid_bundle": { "bundle_path": "", "bundle_validation_errors_on_master": [], "checksum": "", "timestamp": "0" },
				"reload_bundle_issued": "0",
				"status": "None"
			},
			"backup_and_restore_primaries": "0",
			"controlled_rolling_restart_flag": "0",
			"eai:acl": null,
			"indexing_ready_flag": "1",
			"initialized_flag": "1",
			"label": "cm1",
			"last_check_restart_bundle_result": "0",
			"last_dry_run_bundle": { "bundle_path": "", "checksum": "", "timestamp": "0" },
			"last_validated_bundle": { "bundle_path": "/b", "checksum": "0B353BAF", "is_valid_bundle": "1", "timestamp": "1758530970" },
			"latest_bundle": {{Bundle}},
			"maintenance_mode": "0",
			"multisite": "0",
			"previous_active_bundle": { "bundle_path": "", "checksum": "", "timestamp": "0" },
			"primaries_backup_status": "No on-going (or) completed primaries backup yet.",
			"quiet_period_flag": "0",
			"rolling_restart_flag": "0",
			"rolling_restart_or_upgrade": "0",
			"service_ready_flag": "1",
			"start_time": "1758530970",
			"summary_replication": "false"
		}
		""";

	private const string HealthContent = """
		{
			"all_data_is_searchable": "1", "all_peers_are_up": "1", "cm_version_is_compatible": "1", "eai:acl": null,
			"multisite": "0", "no_fixup_tasks_in_progress": "1", "pre_flight_check": "1", "replication_factor_met": "1",
			"search_factor_met": "1", "site_replication_factor_met": "1", "site_search_factor_met": "0",
			"splunk_version_peer_count": "{ 10.6.0: 3 }"
		}
		""";

	private const string StatusContent = """
		{
			"decommission_force_timeout": "180",
			"eai:acl": null,
			"maintenance_mode": "1",
			"messages": "",
			"multisite": "0",
			"peers": { "08696C19-548F-4563-BA53-2A18769091DB": { "label": "idx3", "site": "default", "status": "Restarting" } },
			"restart_inactivity_timeout": "600",
			"restart_progress": { "done": ["idx1"], "failed": [], "in_progress": ["idx3"], "to_be_restarted": ["idx2"] },
			"rolling_restart_flag": "1",
			"rolling_restart_or_upgrade": "1",
			"searchable_rolling": "1",
			"service_ready_flag": "1"
		}
		""";

	private const string FixupContent = """
		{
			"eai:acl": null,
			"index": "_audit",
			"initial": { "reason": "add peer=2222 new bucket", "timestamp": "1447099323" },
			"latest": { "reason": "Missing enough suitable candidates. Missing={ site2:1 }", "timestamp": "1447117547" },
			"level": "replication_factor"
		}
		""";

	// From the reference's JSON example.
	private const string RedundancyContent = """
		{
			"active_bundle_id": "075EA8FB2D1172A1A7AD9DA472C63E92",
			"eai:acl": null,
			"generation_id": "21",
			"ha_mode": "Standby",
			"last_heartbeat": 1643099380,
			"manager_switchover_mode": "auto",
			"peers_count": "5",
			"server_name": "cm-standby2",
			"uri": "https://cm2.example.com:8089"
		}
		""";

	[Fact]
	public async Task GetInfoAsync_MapsEveryModelledField()
	{
		var info = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManager.GetInfoAsync(ct), "manager", InfoContent);

		info.Label.Should().Be("cm1");
		info.StartTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1758530970));
		info.ActiveBundle!.BundlePath.Should().EndWith("0143dd2c-1758530970.bundle");
		info.ActiveBundle.Checksum.Should().Be("0B353BAF");
		info.ActiveBundle.Timestamp.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1758530970));
		info.LatestBundle!.Checksum.Should().Be("0B353BAF");
		info.PreviousActiveBundle!.Timestamp.Should().BeNull();
		info.LastValidatedBundle!.IsValidBundle.Should().BeTrue();
		info.LastDryRunBundle!.BundlePath.Should().BeEmpty();
		info.ApplyBundleStatus!.Status.Should().Be("None");
		info.ApplyBundleStatus.ReloadBundleIssued.Should().BeFalse();
		info.ApplyBundleStatus.AdditionalProperties.Should().ContainKey("invalid_bundle");
		info.LastCheckRestartBundleResult.Should().BeFalse();
		info.BackupAndRestorePrimaries.Should().BeFalse();
		info.PrimariesBackupStatus.Should().StartWith("No on-going");
		info.ControlledRollingRestartFlag.Should().BeFalse();
		info.IndexingReadyFlag.Should().BeTrue();
		info.InitializedFlag.Should().BeTrue();
		info.MaintenanceMode.Should().BeFalse();
		info.Multisite.Should().BeFalse();
		info.QuietPeriodFlag.Should().BeFalse();
		info.RollingRestartFlag.Should().BeFalse();
		info.RollingRestartOrUpgrade.Should().BeFalse();
		info.ServiceReadyFlag.Should().BeTrue();
		info.SummaryReplication.Should().BeFalse();
	}

	[Fact]
	public async Task GetHealthAsync_MapsEveryModelledField()
	{
		var health = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManager.GetHealthAsync(ct), "manager", HealthContent);

		health.PreFlightCheck.Should().BeTrue();
		health.AllDataIsSearchable.Should().BeTrue();
		health.AllPeersAreUp.Should().BeTrue();
		health.ManagerVersionIsCompatible.Should().BeTrue();
		health.Multisite.Should().BeFalse();
		health.NoFixupTasksInProgress.Should().BeTrue();
		health.ReplicationFactorMet.Should().BeTrue();
		health.SearchFactorMet.Should().BeTrue();
		health.SiteReplicationFactorMet.Should().BeTrue();
		health.SiteSearchFactorMet.Should().BeFalse();
		health.SplunkVersionPeerCount.Should().Be("{ 10.6.0: 3 }");
	}

	[Fact]
	public async Task GetStatusAsync_MapsEveryModelledField()
	{
		var status = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManager.GetStatusAsync(ct), "manager", StatusContent);

		status.DecommissionForceTimeout.Should().Be(180);
		status.MaintenanceMode.Should().BeTrue();
		status.Messages.Should().BeEmpty();
		status.Multisite.Should().BeFalse();
		var peer = status.Peers["08696C19-548F-4563-BA53-2A18769091DB"];
		peer.Label.Should().Be("idx3");
		peer.Site.Should().Be("default");
		peer.Status.Should().Be(ClusterPeerStatus.Restarting);
		status.RestartInactivityTimeout.Should().Be(600);
		status.RestartProgress!.Done.Should().Equal("idx1");
		status.RestartProgress.Failed.Should().BeEmpty();
		status.RestartProgress.InProgress.Should().Equal("idx3");
		status.RestartProgress.ToBeRestarted.Should().Equal("idx2");
		status.RollingRestartFlag.Should().BeTrue();
		status.RollingRestartOrUpgrade.Should().BeTrue();
		status.SearchableRolling.Should().BeTrue();
		status.ServiceReadyFlag.Should().BeTrue();
	}

	[Fact]
	public async Task ListFixupsAsync_MapsEveryModelledField()
	{
		var bucket = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManager.ListFixupsAsync(ClusterFixupLevel.ReplicationFactor, null, ct), "_audit~212~2222", FixupContent);

		bucket.Index.Should().Be("_audit");
		bucket.Level.Should().Be(ClusterFixupLevel.ReplicationFactor);
		bucket.Initial!.Reason.Should().Be("add peer=2222 new bucket");
		bucket.Initial.Timestamp.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1447099323));
		bucket.Latest!.Reason.Should().StartWith("Missing enough suitable candidates");
		bucket.Latest.Timestamp.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1447117547));
	}

	[Fact]
	public async Task ListRedundancyAsync_MapsEveryModelledField()
	{
		var node = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManager.ListRedundancyAsync(ct), "841BD315-21DB-4589-8813-15199DF02F1F", RedundancyContent);

		node.ActiveBundleId.Should().Be("075EA8FB2D1172A1A7AD9DA472C63E92");
		node.GenerationId.Should().Be(21);
		node.HaMode.Should().Be(ClusterHaMode.Standby);
		node.LastHeartbeat.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1643099380));
		node.ManagerSwitchoverMode.Should().Be("auto");
		node.PeersCount.Should().Be(5);
		node.ServerName.Should().Be("cm-standby2");
		node.Uri.Should().Be("https://cm2.example.com:8089");
	}

	[Theory]
	[InlineData("corruption", ClusterFixupLevel.Corruption)]
	[InlineData("streaming", ClusterFixupLevel.Streaming)]
	[InlineData("data_safety", ClusterFixupLevel.DataSafety)]
	[InlineData("generation", ClusterFixupLevel.Generation)]
	[InlineData("search_factor", ClusterFixupLevel.SearchFactor)]
	[InlineData("checksum_sync", ClusterFixupLevel.ChecksumSync)]
	public async Task ListFixupsAsync_SendsEachLevelByItsWireName(string wire, ClusterFixupLevel level)
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManager.ListFixupsAsync(level, null, ct)))
			.Uri.Query.Should().Be($"?level={wire}&output_mode=json");
}
