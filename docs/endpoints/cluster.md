# cluster endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/cluster-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `cluster/config` | `IClusterConfig.ListAsync` | `ClusterConfigTests.ListAsync_SendsGet` |
| GET | `cluster/config/config` | `IClusterConfig.GetAsync` | `ClusterConfigTests.GetAsync_SendsGet` |
| POST | `cluster/config/config` | `IClusterConfig.UpdateAsync` | `ClusterConfigTests.UpdateAsync_SendsPostWithTheSetFields` |
| GET | `cluster/manager/buckets` | `IClusterManagerBuckets.ListAsync` | `ClusterManagerBucketsTests.ListAsync_SendsGetWithRepeatedFilters` |
| GET | `cluster/manager/buckets/{name}` | `IClusterManagerBuckets.GetAsync` | `ClusterManagerBucketsTests.GetAsync_SendsGetWithTheBucketId` |
| POST | `cluster/manager/buckets/{bucket_id}/fix` | `IClusterManagerBuckets.FixAsync` | `ClusterManagerBucketsTests.FixAsync_SendsPost` |
| POST | `cluster/manager/buckets/{bucket_id}/fix_corrupt_bucket` | `IClusterManagerBuckets.FixCorruptAsync` | `ClusterManagerBucketsTests.FixCorruptAsync_SendsPost` |
| POST | `cluster/manager/buckets/{bucket_id}/freeze` | `IClusterManagerBuckets.FreezeAsync` | `ClusterManagerBucketsTests.FreezeAsync_SendsPost` |
| POST | `cluster/manager/buckets/{bucket_id}/remove_all` | `IClusterManagerBuckets.RemoveAllAsync` | `ClusterManagerBucketsTests.RemoveAllAsync_SendsPost` |
| POST | `cluster/manager/buckets/{bucket_id}/remove_from_peer` | `IClusterManagerBuckets.RemoveFromPeerAsync` | `ClusterManagerBucketsTests.RemoveFromPeerAsync_SendsPostWithThePeer` |
| POST | `cluster/manager/control/control/prune_index` | `IClusterManagerControl.PruneIndexAsync` | `ClusterManagerControlTests.PruneIndexAsync_SendsPostWithTheIndex` |
| POST | `cluster/manager/control/control/rebalance_primaries` | `IClusterManagerControl.RebalancePrimariesAsync` | `ClusterManagerControlTests.RebalancePrimariesAsync_SendsPost` |
| POST | `cluster/manager/control/control/remove_peers` | `IClusterManagerControl.RemovePeersAsync` | `ClusterManagerControlTests.RemovePeersAsync_SendsPostWithThePeers` |
| POST | `cluster/manager/control/control/resync_bucket_from_peer` | `IClusterManagerControl.ResyncBucketFromPeerAsync` | `ClusterManagerControlTests.ResyncBucketFromPeerAsync_SendsPostWithBucketAndPeer` |
| POST | `cluster/manager/control/control/roll-hot-buckets` | `IClusterManagerControl.RollHotBucketAsync` | `ClusterManagerControlTests.RollHotBucketAsync_SendsPostWithTheBucket` |
| POST | `cluster/manager/control/control/rolling_upgrade_finalize` | `IClusterManagerControl.FinalizeRollingUpgradeAsync` | `ClusterManagerControlTests.FinalizeRollingUpgradeAsync_SendsPost` |
| POST | `cluster/manager/control/control/rolling_upgrade_init` | `IClusterManagerControl.InitializeRollingUpgradeAsync` | `ClusterManagerControlTests.InitializeRollingUpgradeAsync_SendsPost` |
| POST | `cluster/manager/control/default/abort_restart` | `IClusterManagerOperations.AbortRestartAsync` | `ClusterManagerOperationsTests.AbortRestartAsync_SendsPost` |
| POST | `cluster/manager/control/default/apply` | `IClusterManagerOperations.ApplyBundleAsync` | `ClusterManagerOperationsTests.ApplyBundleAsync_SendsPostWithTheOptions` |
| POST | `cluster/manager/control/default/cancel_bundle_push` | `IClusterManagerOperations.CancelBundlePushAsync` | `ClusterManagerOperationsTests.CancelBundlePushAsync_SendsPost` |
| POST | `cluster/manager/control/default/maintenance` | `IClusterManagerOperations.SetMaintenanceModeAsync` | `ClusterManagerOperationsTests.SetMaintenanceModeAsync_SendsPostWithTheMode` |
| POST | `cluster/manager/control/default/rollback` | `IClusterManagerOperations.RollbackBundleAsync` | `ClusterManagerOperationsTests.RollbackBundleAsync_SendsPost` |
| POST | `cluster/manager/control/default/validate_bundle` | `IClusterManagerOperations.ValidateBundleAsync` | `ClusterManagerOperationsTests.ValidateBundleAsync_SendsPostWithCheckRestart` |
| GET | `cluster/manager/fixup` | `IClusterManager.ListFixupsAsync` | `ClusterManagerTests.ListFixupsAsync_SendsGetWithTheLevelAndIndex` |
| GET | `cluster/manager/generation` | `IClusterManagerGenerations.ListAsync` | `ClusterManagerGenerationsTests.ListAsync_SendsGet` |
| POST | `cluster/manager/generation` | `IClusterManagerGenerations.CreateAsync` | `ClusterManagerGenerationsTests.CreateAsync_SendsPostWithTheSearchHead` |
| GET | `cluster/manager/generation/{name}` | `IClusterManagerGenerations.GetAsync` | `ClusterManagerGenerationsTests.GetAsync_SendsGetWithTheName` |
| POST | `cluster/manager/generation/{name}` | `IClusterManagerGenerations.UpdateAsync` | `ClusterManagerGenerationsTests.UpdateAsync_SendsPostWithTheSettings` |
| GET | `cluster/manager/ha_active_status` | `IClusterManager.GetHaActiveStatusAsync` | `ClusterManagerTests.GetHaActiveStatusAsync_SendsGet` |
| GET | `cluster/manager/health` | `IClusterManager.GetHealthAsync` | `ClusterManagerTests.GetHealthAsync_SendsGet` |
| GET | `cluster/manager/indexes` | `IClusterManagerIndexes.ListAsync` | `ClusterManagerIndexesTests.ListAsync_SendsGet` |
| GET | `cluster/manager/indexes/{name}` | `IClusterManagerIndexes.GetAsync` | `ClusterManagerIndexesTests.GetAsync_SendsGetWithTheName` |
| GET | `cluster/manager/info` | `IClusterManager.GetInfoAsync` | `ClusterManagerTests.GetInfoAsync_SendsGet` |
| GET | `cluster/manager/peers` | `IClusterManagerPeers.ListAsync` | `ClusterManagerPeersTests.ListAsync_SendsGet` |
| GET | `cluster/manager/peers/{name}` | `IClusterManagerPeers.GetAsync` | `ClusterManagerPeersTests.GetAsync_SendsGetWithListBuckets` |
| GET | `cluster/manager/redundancy` | `IClusterManager.ListRedundancyAsync` | `ClusterManagerTests.ListRedundancyAsync_SendsGet` |
| POST | `cluster/manager/redundancy` | `IClusterManager.SwitchHaModeAsync` | `ClusterManagerTests.SwitchHaModeAsync_SendsPostWithTheActionAndMode` |
| GET | `cluster/manager/sites` | `IClusterManagerSites.ListAsync` | `ClusterManagerSitesTests.ListAsync_SendsGet` |
| GET | `cluster/manager/sites/{name}` | `IClusterManagerSites.GetAsync` | `ClusterManagerSitesTests.GetAsync_SendsGetWithTheSite` |
| GET | `cluster/manager/status` | `IClusterManager.GetStatusAsync` | `ClusterManagerTests.GetStatusAsync_SendsGet` |
| GET | `cluster/searchhead/generation` | `IClusterSearchHeadGenerations.ListAsync` | `ClusterSearchHeadGenerationsTests.ListAsync_SendsGet` |
| GET | `cluster/searchhead/generation/{name}` | `IClusterSearchHeadGenerations.GetAsync` | `ClusterSearchHeadGenerationsTests.GetAsync_SendsGetWithTheDoubleEncodedManagerUri` |
| GET | `cluster/searchhead/searchheadconfig` | `IClusterSearchHeadConfigs.ListAsync` | `ClusterSearchHeadConfigsTests.ListAsync_SendsGet` |
| POST | `cluster/searchhead/searchheadconfig` | `IClusterSearchHeadConfigs.CreateAsync` | `ClusterSearchHeadConfigsTests.CreateAsync_SendsPostWithManagerAndSecret` |
| GET | `cluster/searchhead/searchheadconfig/{name}` | `IClusterSearchHeadConfigs.GetAsync` | `ClusterSearchHeadConfigsTests.GetAsync_SendsGetWithTheName` |
| POST | `cluster/searchhead/searchheadconfig/{name}` | `IClusterSearchHeadConfigs.UpdateAsync` | `ClusterSearchHeadConfigsTests.UpdateAsync_SendsPostWithTheSettings` |
| DELETE | `cluster/searchhead/searchheadconfig/{name}` | `IClusterSearchHeadConfigs.DeleteAsync` | `ClusterSearchHeadConfigsTests.DeleteAsync_SendsDelete` |
| GET | `cluster/peer/buckets` | `IClusterPeerBuckets.ListAsync` | `ClusterPeerBucketsTests.ListAsync_SendsGetWithTheGeneration` |
| GET | `cluster/peer/buckets/{name}` | `IClusterPeerBuckets.GetAsync` | `ClusterPeerBucketsTests.GetAsync_SendsGetWithTheGeneration` |
| DELETE | `cluster/peer/buckets/{name}` | `IClusterPeerBuckets.DeleteAsync` | `ClusterPeerBucketsTests.DeleteAsync_SendsDeleteWithTheBucketIdField` |
| POST | `cluster/peer/control/control/decommission` | `IClusterPeer.DecommissionAsync` | `ClusterPeerTests.DecommissionAsync_SendsPost` |
| POST | `cluster/peer/control/control/re-add-peer` | `IClusterPeer.ReAddAsync` | `ClusterPeerTests.ReAddAsync_SendsPostWithClearMasks` |
| POST | `cluster/peer/control/control/set_detention_override (deprecated)` |  |  |
| POST | `cluster/peer/control/control/set_manual_detention` | `IClusterPeer.SetManualDetentionAsync` | `ClusterPeerTests.SetManualDetentionAsync_SendsPostWithTheState` |
| GET | `cluster/peer/info` | `IClusterPeer.GetInfoAsync` | `ClusterPeerTests.GetInfoAsync_SendsGet` |
| GET | `replication/configuration/health` | `IConfigurationReplication.GetHealthAsync` | `ConfigurationReplicationTests.GetHealthAsync_SendsGetWithTheChecks` |
| GET | `replication/configuration/quarantined-assets` | `IConfigurationReplication.ListQuarantinedAssetsAsync` | `ConfigurationReplicationTests.ListQuarantinedAssetsAsync_SendsGet` |
| GET | `shcluster/captain/artifacts` | `IShClusterCaptainArtifacts.ListAsync` | `ShClusterCaptainArtifactsTests.ListAsync_SendsGetWithRemoteSids` |
| GET | `shcluster/captain/artifacts/{name}` | `IShClusterCaptainArtifacts.GetAsync` | `ShClusterCaptainArtifactsTests.GetAsync_SendsGetWithTheSid` |
| POST | `shcluster/captain/control/default/restart` | `IShClusterCaptain.RestartAsync` | `ShClusterCaptainTests.RestartAsync_SendsPostWithTheOptions` |
| POST | `shcluster/captain/control/control/rotate-splunk-secret` | `IShClusterCaptain.RotateSplunkSecretAsync` | `ShClusterCaptainTests.RotateSplunkSecretAsync_SendsPost` |
| POST | `shcluster/captain/control/control/upgrade-init` | `IShClusterCaptain.InitializeUpgradeAsync` | `ShClusterCaptainTests.InitializeUpgradeAsync_SendsPost` |
| POST | `shcluster/captain/control/control/upgrade-finalize` | `IShClusterCaptain.FinalizeUpgradeAsync` | `ShClusterCaptainTests.FinalizeUpgradeAsync_SendsPost` |
| GET | `shcluster/captain/info` | `IShClusterCaptain.GetInfoAsync` | `ShClusterCaptainTests.GetInfoAsync_SendsGet` |
| GET | `shcluster/captain/jobs` | `IShClusterCaptainJobs.ListAsync` | `ShClusterCaptainJobsTests.ListAsync_SendsGet` |
| GET | `shcluster/captain/jobs/{name}` | `IShClusterCaptainJobs.GetAsync` | `ShClusterCaptainJobsTests.GetAsync_SendsGetWithTheEscapedName` |
| GET | `shcluster/captain/members` | `IShClusterCaptainMembers.ListAsync` | `ShClusterCaptainMembersTests.ListAsync_SendsGet` |
| GET | `shcluster/captain/members/{name}` | `IShClusterCaptainMembers.GetAsync` | `ShClusterCaptainMembersTests.GetAsync_SendsGetWithTheGuid` |
| GET | `shcluster/config` | `IShClusterConfig.ListAsync` | `ShClusterConfigTests.ListAsync_SendsGet` |
| POST | `shcluster/config/config` | `IShClusterConfig.UpdateAsync` | `ShClusterConfigTests.UpdateAsync_SendsPostWithTheSettings` |
| GET | `shcluster/member/artifacts` | `IShClusterMember.ListArtifactsAsync` | `ShClusterMemberTests.ListArtifactsAsync_SendsGet` |
| GET | `shcluster/member/artifacts/{name}` | `IShClusterMember.GetArtifactAsync` | `ShClusterMemberTests.GetArtifactAsync_SendsGetWithTheSid` |
| POST | `shcluster/member/control/control/set_manual_detention` | `IShClusterMember.SetManualDetentionAsync` | `ShClusterMemberTests.SetManualDetentionAsync_SendsPostWithTheState` |
| GET | `shcluster/member/consensus` | `IShClusterMember.GetConsensusAsync` | `ShClusterMemberTests.GetConsensusAsync_SendsGet` |
| GET | `shcluster/member/info` | `IShClusterMember.GetInfoAsync` | `ShClusterMemberTests.GetInfoAsync_SendsGet` |
| GET | `shcluster/status` | `IShClusterStatus.GetAsync` | `ShClusterStatusTests.GetAsync_SendsGetWithAdvanced` |
| POST | `upgrade/shc/recovery` | `IShClusterUpgrades.RecoverAsync` | `ShClusterUpgradesTests.RecoverAsync_SendsPost` |
| GET | `upgrade/shc/status` | `IShClusterUpgrades.GetStatusAsync` | `ShClusterUpgradesTests.GetStatusAsync_SendsGet` |
| POST | `upgrade/shc/upgrade` | `IShClusterUpgrades.StartAsync` | `ShClusterUpgradesTests.StartAsync_SendsPost` |
