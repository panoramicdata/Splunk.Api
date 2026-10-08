# introspection endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/introspection-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `data/index-volumes` |  |  |
| GET | `data/index-volumes/{name}` |  |  |
| GET | `data/indexes` |  |  |
| POST | `data/indexes` |  |  |
| GET | `data/indexes/{name}` |  |  |
| POST | `data/indexes/{name}` |  |  |
| DELETE | `data/indexes/{name}` |  |  |
| GET | `data/indexes-extended` |  |  |
| GET | `data/indexes-extended/{name}` |  |  |
| GET | `data/summaries` |  |  |
| GET | `data/summaries/{summary_name}` |  |  |
| GET | `server/health/deployment` |  |  |
| GET | `server/health/deployment/details` |  |  |
| GET | `server/health/splunkd` |  |  |
| GET | `server/health/splunkd/details` |  |  |
| GET | `server/health-config` |  |  |
| POST | `server/health-config/{alert_action}` |  |  |
| POST | `server/health-config/{feature_name}` |  |  |
| GET | `server/info` | IServerInfo.GetAsync | ServerInfoTests.GetAsync_SendsGetToServerInfoAsJson |
| GET | `server/introspection` |  |  |
| GET | `server/introspection/indexer` |  |  |
| GET | `server/introspection/kvstore` |  |  |
| GET | `server/introspection/kvstore/collectionstats` |  |  |
| GET | `server/introspection/kvstore/replicasetstats` |  |  |
| GET | `server/introspection/kvstore/serverstatus` |  |  |
| GET | `server/introspection/search/dispatch` |  |  |
| GET | `server/introspection/search/dispatch/Bundle_Directory_Reaper` |  |  |
| GET | `server/introspection/search/dispatch/Compute_User_Search_Quota` |  |  |
| GET | `server/introspection/search/dispatch/Dispatch_Directory_Reaper` |  |  |
| GET | `server/introspection/search/dispatch/Search_StartUp_Time` |  |  |
| GET | `server/introspection/search/distributed` |  |  |
| GET | `server/introspection/search/saved` |  |  |
| GET | `server/status` |  |  |
| GET | `server/status/dispatch-artifacts` |  |  |
| GET | `server/status/fishbucket` |  |  |
| GET | `server/status/installed-file-integrity` |  |  |
| GET | `server/status/limits/search-concurrency` |  |  |
| GET | `server/status/partitions-space` |  |  |
| GET | `server/status/resource-usage` |  |  |
| GET | `server/status/resource-usage/hostwide` |  |  |
| GET | `server/status/resource-usage/iostats` |  |  |
| GET | `server/status/resource-usage/splunk-processes` |  |  |
| GET | `server/sysinfo` |  |  |
| GET | `saved/bookmarks/monitoring_console` |  |  |
| POST | `saved/bookmarks/monitoring_console` |  |  |
| DELETE | `saved/bookmarks/monitoring_console` |  |  |
