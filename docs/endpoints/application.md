# application endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/application-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| POST | `apps/appinstall (deprecated)` |  |  |
| GET | `apps/apptemplates` | `IAppTemplates.ListAsync` | `AppTemplatesTests.ListAsync_SendsGet` |
| GET | `apps/apptemplates/{name}` | `IAppTemplates.GetAsync` | `AppTemplatesTests.GetAsync_SendsGetForTheName` |
| GET | `apps/local` | `IApps.ListAsync` | `AppsTests.ListAsync_SendsGet` |
| POST | `apps/local` | `IApps.CreateAsync` | `AppsTests.CreateAsync_PostsTheApp` |
| GET | `apps/local/{name}` | `IApps.GetAsync` | `AppsTests.GetAsync_SendsRefresh` |
| POST | `apps/local/{name}` | `IApps.UpdateAsync` | `AppsTests.UpdateAsync_PostsTheChanges` |
| DELETE | `apps/local/{name}` | `IApps.DeleteAsync` | `AppsTests.DeleteAsync_SendsDelete` |
| GET | `apps/local/{name}/package (deprecated)` |  |  |
| GET | `apps/local/{name}/setup` | `IApps.GetSetupAsync` | `AppsTests.GetSetupAsync_SendsGet` |
| GET | `apps/local/{name}/update` | `IApps.CheckForUpdateAsync` | `AppsTests.CheckForUpdateAsync_SendsGet` |
