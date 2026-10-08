# kv-store endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/kv-store-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| POST | `kvstore/backup/create` | IKvStore.CreateBackupAsync | KvStoreTests.CreateBackupAsync_PostsEveryField |
| POST | `kvstore/backup/restore` | IKvStore.RestoreBackupAsync | KvStoreTests.RestoreBackupAsync_PostsEveryField |
| POST | `kvstore/control/maintenance` | IKvStore.SetMaintenanceModeAsync | KvStoreTests.SetMaintenanceModeAsync_PostsTheMode |
| GET | `kvstore/status` | IKvStore.GetStatusAsync | KvStoreTests.GetStatusAsync_SendsGet |
| POST | `shcluster/captain/kvmigrate/start (deprecated)` |  |  |
| GET | `shcluster/captain/kvmigrate/status (deprecated)` |  |  |
| POST | `shcluster/captain/kvmigrate/stop (deprecated)` |  |  |
| GET | `storage/collections/config` | IKvStoreCollections.ListAsync | KvStoreCollectionsTests.ListAsync_SendsGetInTheNamespace |
| POST | `storage/collections/config` | IKvStoreCollections.CreateAsync | KvStoreCollectionsTests.CreateAsync_PostsSettingsFieldsAndAccelerations |
| GET | `storage/collections/config/{collection}` | IKvStoreCollections.GetAsync | KvStoreCollectionsTests.GetAsync_SendsGet |
| POST | `storage/collections/config/{collection}` | IKvStoreCollections.UpdateAsync | KvStoreCollectionsTests.UpdateAsync_PostsTheChanges |
| DELETE | `storage/collections/config/{collection}` | IKvStoreCollections.DeleteAsync | KvStoreCollectionsTests.DeleteAsync_SendsDelete |
| GET | `storage/collections/data/{collection}` | IKvStoreData.QueryAsync | KvStoreDataTests.QueryAsync_SendsEveryQueryOption |
| POST | `storage/collections/data/{collection}` | IKvStoreData.InsertAsync | KvStoreDataTests.InsertAsync_PostsTheDocumentAsJson |
| DELETE | `storage/collections/data/{collection}` | IKvStoreData.DeleteWhereAsync | KvStoreDataTests.DeleteWhereAsync_SendsTheFilter |
| GET | `storage/collections/data/{collection}/{key}` | IKvStoreData.GetAsync | KvStoreDataTests.GetAsync_SendsGetForTheKey |
| POST | `storage/collections/data/{collection}/{key}` | IKvStoreData.UpdateAsync | KvStoreDataTests.UpdateAsync_PostsTheWholeDocument |
| DELETE | `storage/collections/data/{collection}/{key}` | IKvStoreData.DeleteAsync | KvStoreDataTests.DeleteAsync_SendsDeleteForTheKey |
| POST | `storage/collections/data/{collection}/batch_find` | IKvStoreData.BatchFindAsync | KvStoreDataTests.BatchFindAsync_PostsTheQueries |
| POST | `storage/collections/data/{collection}/batch_save` | IKvStoreData.BatchSaveAsync | KvStoreDataTests.BatchSaveAsync_PostsTheDocuments |
| GET | `storage/collections/stats` | IKvStoreStatistics.GetCollectionAsync | KvStoreStatisticsTests.GetCollectionAsync_SendsTheCollectionInTheQuery |
| GET | `storage/collections/stats/fields` | IKvStoreStatistics.GetFieldsAsync | KvStoreStatisticsTests.GetFieldsAsync_SendsEveryOption |
