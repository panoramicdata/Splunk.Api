# workload-management endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/workload-management-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `workloads/categories` |  |  |
| POST | `workloads/categories` |  |  |
| GET | `workloads/pools` |  |  |
| POST | `workloads/pools` |  |  |
| GET | `workloads/rules` |  |  |
| POST | `workloads/rules` |  |  |
| DELETE | `workloads/rules` |  |  |
| POST | `workloads/config/enable` |  |  |
| POST | `workloads/config/disable` |  |  |
| GET | `workloads/config/get-base-dirname` |  |  |
| GET | `workloads/config/preflight-checks` |  |  |
| POST | `workloads/config/set-base-dirname` |  |  |
| GET | `workloads/policy/search_admission_control` |  |  |
| POST | `workloads/policy/search_admission_control` |  |  |
| GET | `workloads/status` |  |  |
