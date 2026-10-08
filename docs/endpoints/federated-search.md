# federated-search endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/federated-search-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `data/federated/settings/general` | `IFederatedSearchSettings.GetAsync` | `FederatedSearchSettingsTests.GetAsync_SendsGet` |
| POST | `data/federated/settings/general` | `IFederatedSearchSettings.UpdateAsync` | `FederatedSearchSettingsTests.UpdateAsync_SendsPostWithTheSetFields` |
| GET | `data/federated/provider` | `IFederatedProviders.ListAsync` | `FederatedProvidersTests.ListAsync_SendsGetWithOptions` |
| POST | `data/federated/provider` | `IFederatedProviders.CreateAsync` | `FederatedProvidersTests.CreateAsync_SendsPostWithTheProvider` |
| POST | `data/federated/provider/turnOffProvidersInBatch` | `IFederatedProviders.DisableAllAsync` | `FederatedProvidersTests.DisableAllAsync_SendsPostWithTheType` |
| GET | `data/federated/provider/{federated_provider_name}` | `IFederatedProviders.GetAsync` | `FederatedProvidersTests.GetAsync_SendsGetWithTheName` |
| POST | `data/federated/provider/{federated_provider_name}` | `IFederatedProviders.UpdateAsync` | `FederatedProvidersTests.UpdateAsync_SendsPostWithTheSetFields` |
| DELETE | `data/federated/provider/{federated_provider_name}` | `IFederatedProviders.DeleteAsync` | `FederatedProvidersTests.DeleteAsync_SendsDelete` |
| POST | `data/federated/provider/{federated_provider_name}/disable` | `IFederatedProviders.DisableAsync` | `FederatedProvidersTests.DisableAsync_SendsPost` |
| POST | `data/federated/provider/{federated_provider_name}/enable` | `IFederatedProviders.EnableAsync` | `FederatedProvidersTests.EnableAsync_SendsPost` |
| GET | `data/federated/index` | `IFederatedIndexes.ListAsync` | `FederatedIndexesTests.ListAsync_SendsGet` |
| POST | `data/federated/index` | `IFederatedIndexes.CreateAsync` | `FederatedIndexesTests.CreateAsync_SendsPostWithTheIndex` |
| GET | `data/federated/index/{federated_index_name}` | `IFederatedIndexes.GetAsync` | `FederatedIndexesTests.GetAsync_SendsGetWithTheEscapedName` |
| POST | `data/federated/index/{federated_index_name}` | `IFederatedIndexes.UpdateAsync` | `FederatedIndexesTests.UpdateAsync_SendsPostWithTheSetFields` |
| DELETE | `data/federated/index/{federated_index_name}` | `IFederatedIndexes.DeleteAsync` | `FederatedIndexesTests.DeleteAsync_SendsDelete` |
| POST | `data/federated/index/{federated_index_name}/disable` | `IFederatedIndexes.DisableAsync` | `FederatedIndexesTests.DisableAsync_SendsPost` |
| POST | `data/federated/index/{federated_index_name}/enable` | `IFederatedIndexes.EnableAsync` | `FederatedIndexesTests.EnableAsync_SendsPost` |
