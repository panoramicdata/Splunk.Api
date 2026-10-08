# topology endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/topology-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `stack-explainer/v1/topology` |  |  |
| GET | `stack-explainer/v1/node-identity` |  |  |
| GET | `stack-explainer/v1/node-identity/{guid}` |  |  |
| GET | `stack-explainer/v1/trusted-connections` |  |  |
| GET | `stack-explainer/v1/trusted-connections/{guid}` |  |  |
