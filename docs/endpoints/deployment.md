# deployment endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/deployment-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `deployment/client` | `IDeploymentClientConfig.ListAsync` | `DeploymentClientConfigTests.ListAsync_SendsGetWithOptions` |
| GET | `deployment/client/config` | `IDeploymentClientConfig.GetAsync` | `DeploymentClientConfigTests.GetAsync_SendsGet` |
| GET | `deployment/client/config/listIsDisabled` | `IDeploymentClientConfig.GetDisabledStatusAsync` | `DeploymentClientConfigTests.GetDisabledStatusAsync_SendsGet` |
| POST | `deployment/client/config/reload` | `IDeploymentClientConfig.ReloadConfigAsync` | `DeploymentClientConfigTests.ReloadConfigAsync_SendsPost` |
| POST | `deployment/client/{name}/reload` | `IDeploymentClientConfig.ReloadAsync` | `DeploymentClientConfigTests.ReloadAsync_SendsPostWithTheName` |
| GET | `deployment/server/applications` | `IDeploymentServerApplications.ListAsync` | `DeploymentServerApplicationsTests.ListAsync_SendsGetWithTheFilters` |
| GET | `deployment/server/applications/{name}` | `IDeploymentServerApplications.GetAsync` | `DeploymentServerApplicationsTests.GetAsync_SendsGetWithTheName` |
| POST | `deployment/server/applications/{name}` | `IDeploymentServerApplications.UpdateAsync` | `DeploymentServerApplicationsTests.UpdateAsync_SendsPostWithTheSetFields` |
| GET | `deployment/server/clients` | `IDeploymentServerClients.ListAsync` | `DeploymentServerClientsTests.ListAsync_SendsGetWithTheFilters` |
| GET | `deployment/server/clients/countClients_by_machineType` | `IDeploymentServerClients.CountByMachineTypeAsync` | `DeploymentServerClientsTests.CountByMachineTypeAsync_SendsGet` |
| GET | `deployment/server/clients/countRecentDownloads` | `IDeploymentServerClients.CountRecentDownloadsAsync` | `DeploymentServerClientsTests.CountRecentDownloadsAsync_SendsGetWithMaxAgeSecs` |
| GET | `deployment/server/clients/{name}` | `IDeploymentServerClients.GetAsync` | `DeploymentServerClientsTests.GetAsync_SendsGetWithTheFilters` |
| DELETE | `deployment/server/clients/{name}` | `IDeploymentServerClients.DeleteAsync` | `DeploymentServerClientsTests.DeleteAsync_SendsDelete` |
| POST | `deployment/server/config` | `IDeploymentServerConfig.PostAsync` | `DeploymentServerConfigTests.PostAsync_SendsPostWithTheFields` |
| GET | `deployment/server/config/attributesUnsupportedInUI` | `IDeploymentServerConfig.ListUnsupportedAttributesAsync` | `DeploymentServerConfigTests.ListUnsupportedAttributesAsync_SendsGet` |
| GET | `deployment/server/config/listIsDisabled` | `IDeploymentServerConfig.GetDisabledStatusAsync` | `DeploymentServerConfigTests.GetDisabledStatusAsync_SendsGet` |
| GET | `deployment/server/serverclasses` | `IDeploymentServerClasses.ListAsync` | `DeploymentServerClassesTests.ListAsync_SendsGet` |
| POST | `deployment/server/serverclasses` | `IDeploymentServerClasses.CreateAsync` | `DeploymentServerClassesTests.CreateAsync_SendsPostWithTheServerClass` |
| POST | `deployment/server/serverclasses/rename` | `IDeploymentServerClasses.RenameAsync` | `DeploymentServerClassesTests.RenameAsync_SendsPostWithBothNames` |
| GET | `deployment/server/serverclasses/{name}` | `IDeploymentServerClasses.GetAsync` | `DeploymentServerClassesTests.GetAsync_SendsGetWithTheFilter` |
| POST | `deployment/server/serverclasses/{name}` | `IDeploymentServerClasses.UpdateAsync` | `DeploymentServerClassesTests.UpdateAsync_SendsPostWithTheSetFields` |
| DELETE | `deployment/server/serverclasses/{name}` | `IDeploymentServerClasses.DeleteAsync` | `DeploymentServerClassesTests.DeleteAsync_SendsDelete` |
| GET | `search/distributed/bundle/replication/config` | `IBundleReplication.GetConfigAsync` | `BundleReplicationTests.GetConfigAsync_SendsGet` |
| GET | `search/distributed/bundle/replication/cycles` | `IBundleReplication.ListCyclesAsync` | `BundleReplicationTests.ListCyclesAsync_SendsGetWithLatest` |
| GET | `search/distributed/bundle-replication-files` | `IBundleReplication.ListFilesAsync` | `BundleReplicationTests.ListFilesAsync_SendsGetWithOptions` |
| GET | `search/distributed/bundle-replication-files/{name}` | `IBundleReplication.GetFileAsync` | `BundleReplicationTests.GetFileAsync_SendsGetWithForceListAll` |
| GET | `search/distributed/config` | `IDistributedSearch.GetConfigAsync` | `DistributedSearchTests.GetConfigAsync_SendsGet` |
| GET | `search/distributed/peers` | `IDistributedSearch.ListPeersAsync` | `DistributedSearchTests.ListPeersAsync_SendsGet` |
| POST | `search/distributed/peers` | `IDistributedSearch.AddPeerAsync` | `DistributedSearchTests.AddPeerAsync_SendsPostWithThePeer` |
| POST | `search/distributed/peers/{name}` | `IDistributedSearch.UpdatePeerAsync` | `DistributedSearchTests.UpdatePeerAsync_SendsPostWithTheCredentials` |
