# metrics-catalog endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/metrics-catalog-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `catalog/metricstore/metrics` | IMetricsCatalog.ListMetricsAsync | MetricsCatalogTests.ListMetricsAsync_SendsGetWithEveryOption |
| GET | `catalog/metricstore/dimensions` | IMetricsCatalog.ListDimensionsAsync | MetricsCatalogTests.ListDimensionsAsync_SendsTheMetricName |
| GET | `catalog/metricstore/dimensions/{dimension-name}/values` | IMetricsCatalog.ListDimensionValuesAsync | MetricsCatalogTests.ListDimensionValuesAsync_SendsTheDimensionAndMetric |
| GET | `catalog/metricstore/rollup` | IMetricRollups.ListAsync | MetricRollupsTests.ListAsync_SendsGetWithPaging |
| POST | `catalog/metricstore/rollup` | IMetricRollups.CreateAsync | MetricRollupsTests.CreateAsync_SendsEverySettingAsForm |
| GET | `catalog/metricstore/rollup/{index}` | IMetricRollups.GetAsync | MetricRollupsTests.GetAsync_SendsGetAndMapsThePolicy |
| POST | `catalog/metricstore/rollup/{index}` | IMetricRollups.UpdateAsync | MetricRollupsTests.UpdateAsync_SendsOnlyWhatIsSet |
| DELETE | `catalog/metricstore/rollup/{index}` | IMetricRollups.DeleteAsync | MetricRollupsTests.DeleteAsync_SendsDelete |
