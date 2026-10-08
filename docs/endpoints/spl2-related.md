# spl2-related endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/spl2-related-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `orchestrator/v1/datasets` |  |  |
| GET | `orchestrator/v1/datasets/{datasetid}` |  |  |
| POST | `orchestrator/v1/spl2/convert` |  |  |
| POST | `orchestrator/v2/spl2/convert` |  |  |
| GET | `orchestrator/v1/spl2/modules` |  |  |
| GET | `orchestrator/v1/spl2/modules/{resourceName}` |  |  |
| PUT | `orchestrator/v1/spl2/modules/{resourceName}` |  |  |
| DELETE | `orchestrator/v1/spl2/modules/{resourceName}` |  |  |
| POST | `orchestrator/v1/spl2/modules/dispatch` |  |  |
| GET | `orchestrator/v1/spl2/modules/permissions` |  |  |
| PUT | `orchestrator/v1/spl2/modules/permissions` |  |  |
