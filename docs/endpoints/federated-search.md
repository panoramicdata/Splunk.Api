# federated-search endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/federated-search-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `data/federated/settings/general` |  |  |
| POST | `data/federated/settings/general` |  |  |
| GET | `data/federated/provider` |  |  |
| POST | `data/federated/provider` |  |  |
| POST | `data/federated/provider/turnOffProvidersInBatch` |  |  |
| GET | `data/federated/provider/{federated_provider_name}` |  |  |
| POST | `data/federated/provider/{federated_provider_name}` |  |  |
| DELETE | `data/federated/provider/{federated_provider_name}` |  |  |
| POST | `data/federated/provider/{federated_provider_name}/disable` |  |  |
| POST | `data/federated/provider/{federated_provider_name}/enable` |  |  |
| GET | `data/federated/index` |  |  |
| POST | `data/federated/index` |  |  |
| GET | `data/federated/index/{federated_index_name}` |  |  |
| POST | `data/federated/index/{federated_index_name}` |  |  |
| DELETE | `data/federated/index/{federated_index_name}` |  |  |
| POST | `data/federated/index/{federated_index_name}/disable` |  |  |
| POST | `data/federated/index/{federated_index_name}/enable` |  |  |
