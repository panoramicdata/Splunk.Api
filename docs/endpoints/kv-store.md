# kv-store endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/kv-store-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| POST | `kvstore/backup/create` |  |  |
| POST | `kvstore/backup/restore` |  |  |
| POST | `kvstore/control/maintenance` |  |  |
| GET | `kvstore/status` |  |  |
| POST | `shcluster/captain/kvmigrate/start (deprecated)` |  |  |
| GET | `shcluster/captain/kvmigrate/status (deprecated)` |  |  |
| POST | `shcluster/captain/kvmigrate/stop (deprecated)` |  |  |
| GET | `storage/collections/config` |  |  |
| POST | `storage/collections/config` |  |  |
| GET | `storage/collections/config/{collection}` |  |  |
| POST | `storage/collections/config/{collection}` |  |  |
| DELETE | `storage/collections/config/{collection}` |  |  |
| GET | `storage/collections/data/{collection}` |  |  |
| POST | `storage/collections/data/{collection}` |  |  |
| DELETE | `storage/collections/data/{collection}` |  |  |
| GET | `storage/collections/data/{collection}/{key}` |  |  |
| POST | `storage/collections/data/{collection}/{key}` |  |  |
| DELETE | `storage/collections/data/{collection}/{key}` |  |  |
| POST | `storage/collections/data/{collection}/batch_find` |  |  |
| POST | `storage/collections/data/{collection}/batch_save` |  |  |
| GET | `storage/collections/stats` |  |  |
| GET | `storage/collections/stats/fields` |  |  |
