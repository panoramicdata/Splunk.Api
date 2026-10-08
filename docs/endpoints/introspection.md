# introspection endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/introspection-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `data/index-volumes` | IIndexVolumes.ListAsync | IndexVolumesTests.ListAsync_SendsGet |
| GET | `data/index-volumes/{name}` | IIndexVolumes.GetAsync | IndexVolumesTests.GetAsync_SendsGet |
| GET | `data/indexes` | IIndexes.ListAsync | IndexesTests.ListAsync_SendsTheDataType |
| POST | `data/indexes` | IIndexes.CreateAsync | IndexesTests.CreateAsync_PostsTheNameAndSettings |
| GET | `data/indexes/{name}` | IIndexes.GetAsync | IndexesTests.GetAsync_SendsSummarize |
| POST | `data/indexes/{name}` | IIndexes.UpdateAsync | IndexesTests.UpdateAsync_PostsEverySetting |
| DELETE | `data/indexes/{name}` | IIndexes.DeleteAsync | IndexesTests.DeleteAsync_SendsDelete |
| GET | `data/indexes-extended` | IIndexesExtended.ListAsync | IndexesExtendedTests.ListAsync_SendsTheDataType |
| GET | `data/indexes-extended/{name}` | IIndexesExtended.GetAsync | IndexesExtendedTests.GetAsync_SendsGet |
| GET | `data/summaries` | IDataSummaries.ListAsync | DataSummariesTests.ListAsync_SendsTheSummaryKinds |
| GET | `data/summaries/{summary_name}` | IDataSummaries.GetAsync | DataSummariesTests.GetAsync_SendsGet |
| GET | `server/health/deployment` | IHealth.GetDeploymentAsync | HealthTests.GetDeploymentAsync_SendsGet |
| GET | `server/health/deployment/details` | IHealth.GetDeploymentDetailsAsync | HealthTests.GetDeploymentDetailsAsync_SendsGet |
| GET | `server/health/splunkd` | IHealth.GetSplunkdAsync | HealthTests.GetSplunkdAsync_SendsGet |
| GET | `server/health/splunkd/details` | IHealth.GetSplunkdDetailsAsync | HealthTests.GetSplunkdDetailsAsync_SendsGet |
| GET | `server/health-config` | IHealthConfig.ListAsync | HealthConfigTests.ListAsync_SendsGet |
| POST | `server/health-config/alert_action:{action_name}` | IHealthConfig.UpdateAlertActionAsync | HealthConfigTests.UpdateAlertActionAsync_PostsToTheAlertActionStanza |
| POST | `server/health-config/feature:{feature_name}` | IHealthConfig.UpdateFeatureAsync | HealthConfigTests.UpdateFeatureAsync_PostsToTheFeatureStanza |
| GET | `server/info` | IServerInfo.GetAsync | ServerInfoTests.GetAsync_SendsGetToServerInfoAsJson |
| GET | `server/introspection` | IIntrospection.ListAsync | IntrospectionTests.ListAsync_SendsGet |
| GET | `server/introspection/indexer` | IIntrospection.GetIndexerAsync | IntrospectionTests.GetIndexerAsync_SendsGet |
| GET | `server/introspection/kvstore` | IKvStoreIntrospection.ListAsync | KvStoreIntrospectionTests.ListAsync_SendsGet |
| GET | `server/introspection/kvstore/collectionstats` | IKvStoreIntrospection.GetCollectionStatsAsync | KvStoreIntrospectionTests.GetCollectionStatsAsync_SendsGet |
| GET | `server/introspection/kvstore/replicasetstats` | IKvStoreIntrospection.GetReplicaSetStatsAsync | KvStoreIntrospectionTests.GetReplicaSetStatsAsync_SendsGet |
| GET | `server/introspection/kvstore/serverstatus` | IKvStoreIntrospection.GetServerStatusAsync | KvStoreIntrospectionTests.GetServerStatusAsync_SendsGet |
| GET | `server/introspection/search/dispatch` | IIntrospection.ListSearchDispatchAsync | IntrospectionTests.ListSearchDispatchAsync_SendsGet |
| GET | `server/introspection/search/dispatch/Bundle_Directory_Reaper` | IIntrospection.GetBundleDirectoryReaperAsync | IntrospectionTests.GetBundleDirectoryReaperAsync_SendsGet |
| GET | `server/introspection/search/dispatch/Compute_User_Search_Quota` | IIntrospection.GetComputeUserSearchQuotaAsync | IntrospectionTests.GetComputeUserSearchQuotaAsync_SendsGet |
| GET | `server/introspection/search/dispatch/Dispatch_Directory_Reaper` | IIntrospection.GetDispatchDirectoryReaperAsync | IntrospectionTests.GetDispatchDirectoryReaperAsync_SendsGet |
| GET | `server/introspection/search/dispatch/Search_StartUp_Time` | IIntrospection.GetSearchStartUpTimeAsync | IntrospectionTests.GetSearchStartUpTimeAsync_SendsGet |
| GET | `server/introspection/search/distributed` | IIntrospection.GetSearchDistributedAsync | IntrospectionTests.GetSearchDistributedAsync_SendsGet |
| GET | `server/introspection/search/saved` | IIntrospection.GetSearchSavedAsync | IntrospectionTests.GetSearchSavedAsync_SendsGet |
| GET | `server/status` | IServerStatus.ListAsync | ServerStatusTests.ListAsync_SendsGet |
| GET | `server/status/dispatch-artifacts` | IServerStatus.GetDispatchArtifactsAsync | ServerStatusTests.GetDispatchArtifactsAsync_SendsGet |
| GET | `server/status/fishbucket` | IServerStatus.GetFishbucketAsync | ServerStatusTests.GetFishbucketAsync_SendsGet |
| GET | `server/status/installed-file-integrity` | IServerStatus.GetInstalledFileIntegrityAsync | ServerStatusTests.GetInstalledFileIntegrityAsync_SendsTheOptions |
| GET | `server/status/limits/search-concurrency` | IServerStatus.GetSearchConcurrencyLimitsAsync | ServerStatusTests.GetSearchConcurrencyLimitsAsync_SendsGet |
| GET | `server/status/partitions-space` | IServerStatus.ListPartitionsSpaceAsync | ServerStatusTests.ListPartitionsSpaceAsync_SendsGet |
| GET | `server/status/resource-usage` | IResourceUsage.ListAsync | ResourceUsageTests.ListAsync_SendsGet |
| GET | `server/status/resource-usage/hostwide` | IResourceUsage.GetHostwideAsync | ResourceUsageTests.GetHostwideAsync_SendsGet |
| GET | `server/status/resource-usage/iostats` | IResourceUsage.ListIoStatsAsync | ResourceUsageTests.ListIoStatsAsync_SendsGet |
| GET | `server/status/resource-usage/splunk-processes` | IResourceUsage.ListSplunkProcessesAsync | ResourceUsageTests.ListSplunkProcessesAsync_SendsGet |
| GET | `server/sysinfo` | ISystemInfo.GetAsync | SystemInfoTests.GetAsync_SendsGet |
| GET | `saved/bookmarks/monitoring_console` | IMonitoringConsoleBookmarks.ListAsync | MonitoringConsoleBookmarksTests.ListAsync_SendsSorting |
| POST | `saved/bookmarks/monitoring_console` | IMonitoringConsoleBookmarks.CreateAsync | MonitoringConsoleBookmarksTests.CreateAsync_PostsNameAndUrl |
| DELETE | `saved/bookmarks/monitoring_console/{name}` | IMonitoringConsoleBookmarks.DeleteAsync | MonitoringConsoleBookmarksTests.DeleteAsync_SendsDeleteToTheBookmark |
