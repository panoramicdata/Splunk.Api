# metrics-catalog endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/metrics-catalog-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `catalog/metricstore/metrics` |  |  |
| GET | `catalog/metricstore/dimensions` |  |  |
| GET | `catalog/metricstore/dimensions/{dimension-name}/values` |  |  |
| GET | `catalog/metricstore/rollup` |  |  |
| POST | `catalog/metricstore/rollup` |  |  |
| GET | `catalog/metricstore/rollup/{index}` |  |  |
| POST | `catalog/metricstore/rollup/{index}` |  |  |
| DELETE | `catalog/metricstore/rollup/{index}` |  |  |
