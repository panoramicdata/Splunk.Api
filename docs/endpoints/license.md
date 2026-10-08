# license endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/license-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `licenser/groups` | `ILicenseGroups.ListAsync` | `LicenseGroupsTests.ListAsync_SendsGet` |
| GET | `licenser/groups/{name}` | `ILicenseGroups.GetAsync` | `LicenseGroupsTests.GetAsync_SendsGetForTheName` |
| POST | `licenser/groups/{name}` | `ILicenseGroups.UpdateAsync` | `LicenseGroupsTests.UpdateAsync_PostsIsActive` |
| GET | `licenser/licenses` | `ILicenses.ListAsync` | `LicensesTests.ListAsync_SendsGet` |
| POST | `licenser/licenses` | `ILicenses.AddAsync` | `LicensesTests.AddAsync_PostsThePayload` |
| GET | `licenser/licenses/{name}` | `ILicenses.GetAsync` | `LicensesTests.GetAsync_SendsGetForTheHash` |
| DELETE | `licenser/licenses/{name}` | `ILicenses.DeleteAsync` | `LicensesTests.DeleteAsync_SendsDelete` |
| GET | `licenser/localpeer` | `ILicenseLocalPeer.GetAsync` | `LicenseLocalPeerTests.GetAsync_SendsGet` |
| GET | `licenser/messages` | `ILicenseMessages.ListAsync` | `LicenseMessagesTests.ListAsync_SendsGet` |
| GET | `licenser/messages/{name}` | `ILicenseMessages.GetAsync` | `LicenseMessagesTests.GetAsync_SendsGetForTheId` |
| GET | `licenser/pools` | `ILicensePools.ListAsync` | `LicensePoolsTests.ListAsync_SendsGet` |
| POST | `licenser/pools` | `ILicensePools.CreateAsync` | `LicensePoolsTests.CreateAsync_PostsThePool` |
| GET | `licenser/pools/{name}` | `ILicensePools.GetAsync` | `LicensePoolsTests.GetAsync_SendsGetForTheName` |
| POST | `licenser/pools/{name}` | `ILicensePools.UpdateAsync` | `LicensePoolsTests.UpdateAsync_PostsTheChanges` |
| DELETE | `licenser/pools/{name}` | `ILicensePools.DeleteAsync` | `LicensePoolsTests.DeleteAsync_SendsDelete` |
| GET | `licenser/peers` | `ILicensePeers.ListAsync` | `LicensePeersTests.ListAsync_SendsGet` |
| GET | `licenser/peers/{name}` | `ILicensePeers.GetAsync` | `LicensePeersTests.GetAsync_SendsGetForTheGuid` |
| GET | `licenser/stacks` | `ILicenseStacks.ListAsync` | `LicenseStacksTests.ListAsync_SendsGet` |
| GET | `licenser/stacks/{name}` | `ILicenseStacks.GetAsync` | `LicenseStacksTests.GetAsync_SendsGetForTheId` |
| GET | `licenser/usage` | `ILicenseUsage.GetAsync` | `LicenseUsageTests.GetAsync_SendsGet` |
