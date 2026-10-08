# cluster endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/cluster-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `cluster/config` |  |  |
| GET | `cluster/config/config` |  |  |
| POST | `cluster/config/config` |  |  |
| GET | `cluster/manager/buckets` |  |  |
| GET | `cluster/manager/buckets/{name}` |  |  |
| POST | `cluster/manager/buckets/{bucket_id}/fix` |  |  |
| POST | `cluster/manager/buckets/{bucket_id}/fix_corrupt_bucket` |  |  |
| POST | `cluster/manager/buckets/{bucket_id}/freeze` |  |  |
| POST | `cluster/manager/buckets/{bucket_id}/remove_all` |  |  |
| POST | `cluster/manager/buckets/{bucket_id}/remove_from_peer` |  |  |
| POST | `cluster/manager/control/control/prune_index` |  |  |
| POST | `cluster/manager/control/control/rebalance_primaries` |  |  |
| POST | `cluster/manager/control/control/remove_peers` |  |  |
| POST | `cluster/manager/control/control/resync_bucket_from_peer` |  |  |
| POST | `cluster/manager/control/control/roll-hot-buckets` |  |  |
| POST | `cluster/manager/control/control/rolling_upgrade_finalize` |  |  |
| POST | `cluster/manager/control/control/rolling_upgrade_init` |  |  |
| POST | `cluster/manager/control/default/abort_restart` |  |  |
| POST | `cluster/manager/control/default/apply` |  |  |
| POST | `cluster/manager/control/default/cancel_bundle_push` |  |  |
| POST | `cluster/manager/control/default/maintenance` |  |  |
| POST | `cluster/manager/control/default/rollback` |  |  |
| POST | `cluster/manager/control/default/validate_bundle` |  |  |
| GET | `cluster/manager/fixup` |  |  |
| GET | `cluster/manager/generation` |  |  |
| POST | `cluster/manager/generation` |  |  |
| GET | `cluster/manager/generation/{name}` |  |  |
| POST | `cluster/manager/generation/{name}` |  |  |
| GET | `cluster/manager/ha_active_status` |  |  |
| GET | `cluster/manager/health` |  |  |
| GET | `cluster/manager/indexes` |  |  |
| GET | `cluster/manager/indexes/{name}` |  |  |
| GET | `cluster/manager/info` |  |  |
| GET | `cluster/manager/peers` |  |  |
| GET | `cluster/manager/peers/{name}` |  |  |
| GET | `cluster/manager/redundancy` |  |  |
| POST | `cluster/manager/redundancy` |  |  |
| GET | `cluster/manager/sites` |  |  |
| GET | `cluster/manager/sites/{name}` |  |  |
| GET | `cluster/manager/status` |  |  |
| GET | `cluster/searchhead/generation` |  |  |
| GET | `cluster/searchhead/generation/{name}` |  |  |
| GET | `cluster/searchhead/searchheadconfig` |  |  |
| POST | `cluster/searchhead/searchheadconfig` |  |  |
| GET | `cluster/searchhead/searchheadconfig/{name}` |  |  |
| POST | `cluster/searchhead/searchheadconfig/{name}` |  |  |
| DELETE | `cluster/searchhead/searchheadconfig/{name}` |  |  |
| GET | `cluster/peer/buckets` |  |  |
| GET | `cluster/peer/buckets/{name}` |  |  |
| DELETE | `cluster/peer/buckets/{name}` |  |  |
| POST | `cluster/peer/control/control/decommission` |  |  |
| POST | `cluster/peer/control/control/re-add-peer` |  |  |
| POST | `cluster/peer/control/control/set_detention_override (deprecated)` |  |  |
| POST | `cluster/peer/control/control/set_manual_detention` |  |  |
| GET | `cluster/peer/info` |  |  |
| GET | `replication/configuration/health` |  |  |
| GET | `replication/configuration/quarantined-assets` |  |  |
| GET | `shcluster/captain/artifacts` |  |  |
| GET | `shcluster/captain/artifacts/{name}` |  |  |
| POST | `shcluster/captain/control/default/restart` |  |  |
| POST | `shcluster/captain/control/control/rotate-splunk-secret` |  |  |
| POST | `shcluster/captain/control/control/upgrade-init` |  |  |
| POST | `shcluster/captain/control/control/upgrade-finalize` |  |  |
| GET | `shcluster/captain/info` |  |  |
| GET | `shcluster/captain/jobs` |  |  |
| GET | `shcluster/captain/jobs/{name}` |  |  |
| GET | `shcluster/captain/members` |  |  |
| GET | `shcluster/captain/members/{name}` |  |  |
| GET | `shcluster/config` |  |  |
| POST | `shcluster/config/config` |  |  |
| GET | `shcluster/member/artifacts` |  |  |
| GET | `shcluster/member/artifacts/{name}` |  |  |
| POST | `shcluster/member/control/control/set_manual_detention` |  |  |
| GET | `shcluster/member/consensus` |  |  |
| GET | `shcluster/member/info` |  |  |
| GET | `shcluster/status` |  |  |
| POST | `upgrade/shc/recovery` |  |  |
| GET | `upgrade/shc/status` |  |  |
| POST | `upgrade/shc/upgrade` |  |  |
