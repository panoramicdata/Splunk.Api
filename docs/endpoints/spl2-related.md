# spl2-related endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/spl2-related-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `orchestrator/v1/datasets` | ISpl2Datasets.ListAsync | Spl2DatasetsTests.ListAsync_SendsGetWithEveryOption |
| GET | `orchestrator/v1/datasets/{datasetid}` | ISpl2Datasets.GetAsync | Spl2DatasetsTests.GetAsync_SendsGetWithConnection |
| POST | `orchestrator/v1/spl2/convert` | ISpl2Conversion.ConvertAsync | Spl2ConversionTests.ConvertAsync_SendsJson |
| POST | `orchestrator/v2/spl2/convert` | ISpl2Conversion.ConvertV2Async | Spl2ConversionTests.ConvertV2Async_SendsJson |
| GET | `orchestrator/v1/spl2/modules` | ISpl2Modules.ListAsync | Spl2ModulesTests.ListAsync_SendsGetWithEveryOption |
| GET | `orchestrator/v1/spl2/modules/{resourceName}` | ISpl2Modules.GetAsync | Spl2ModulesTests.GetAsync_SendsGetAndMapsTheModule |
| PUT | `orchestrator/v1/spl2/modules/{resourceName}` | ISpl2Modules.PutAsync | Spl2ModulesTests.PutAsync_SendsTheModuleAsJson |
| DELETE | `orchestrator/v1/spl2/modules/{resourceName}` | ISpl2Modules.DeleteAsync | Spl2ModulesTests.DeleteAsync_SendsDelete |
| POST | `orchestrator/v1/spl2/modules/dispatch` | ISpl2Modules.DispatchAsync | Spl2ModulesTests.DispatchAsync_SendsTheModuleAndStatementsAsJson |
| GET | `orchestrator/v1/spl2/modules/permissions` | ISpl2Modules.GetPermissionsAsync | Spl2ModulesTests.GetPermissionsAsync_SendsTheResourceName |
| PUT | `orchestrator/v1/spl2/modules/permissions` | ISpl2Modules.UpdatePermissionsAsync | Spl2ModulesTests.UpdatePermissionsAsync_SendsThePermissionsAsJson |
