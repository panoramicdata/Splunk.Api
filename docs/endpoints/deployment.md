# deployment endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/deployment-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `deployment/client` |  |  |
| GET | `deployment/client/config` |  |  |
| GET | `deployment/client/config/listIsDisabled` |  |  |
| POST | `deployment/client/config/reload` |  |  |
| POST | `deployment/client/{name}/reload` |  |  |
| GET | `deployment/server/applications` |  |  |
| GET | `deployment/server/applications/{name}` |  |  |
| POST | `deployment/server/applications/{name}` |  |  |
| GET | `deployment/server/clients` |  |  |
| GET | `deployment/server/clients/countClients_by_machineType` |  |  |
| GET | `deployment/server/clients/countRecentDownloads` |  |  |
| GET | `deployment/server/clients/{name}` |  |  |
| DELETE | `deployment/server/clients/{name}` |  |  |
| POST | `deployment/server/config` |  |  |
| GET | `deployment/server/config/attributesUnsupportedInUI` |  |  |
| GET | `deployment/server/config/listIsDisabled` |  |  |
| GET | `deployment/server/serverclasses` |  |  |
| POST | `deployment/server/serverclasses` |  |  |
| POST | `deployment/server/serverclasses/rename` |  |  |
| GET | `deployment/server/serverclasses/{name}` |  |  |
| POST | `deployment/server/serverclasses/{name}` |  |  |
| DELETE | `deployment/server/serverclasses/{name}` |  |  |
| GET | `search/distributed/bundle/replication/config` |  |  |
| GET | `search/distributed/bundle/replication/cycles` |  |  |
| GET | `search/distributed/bundle-replication-files` |  |  |
| GET | `search/distributed/bundle-replication-files/{name}` |  |  |
| GET | `search/distributed/config` |  |  |
| GET | `search/distributed/peers` |  |  |
| POST | `search/distributed/peers` |  |  |
| POST | `search/distributed/peers/{name}` |  |  |
