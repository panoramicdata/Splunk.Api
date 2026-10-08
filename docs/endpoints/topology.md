# topology endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/topology-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `stack-explainer/v1/topology` | `ITopology.GetAsync`, `ITopology.GetWithUnmanagedActorsAsync` | `TopologyTests.GetAsync_SendsGetWithoutOutputMode`, `TopologyTests.GetWithUnmanagedActorsAsync_SendsTheFlagWithoutAValue` |
| GET | `stack-explainer/v1/node-identity` | `ITopology.GetNodeIdentityAsync` | `TopologyTests.GetNodeIdentityAsync_SendsGet` |
| GET | `stack-explainer/v1/node-identity/{guid}` | `ITopology.GetRemoteNodeIdentityAsync` | `TopologyTests.GetRemoteNodeIdentityAsync_SendsGetWithTheGuid` |
| GET | `stack-explainer/v1/trusted-connections` | `ITopology.GetTrustedConnectionsAsync` | `TopologyTests.GetTrustedConnectionsAsync_SendsGet` |
| GET | `stack-explainer/v1/trusted-connections/{guid}` | `ITopology.GetRemoteTrustedConnectionsAsync` | `TopologyTests.GetRemoteTrustedConnectionsAsync_SendsGetWithTheGuid` |
