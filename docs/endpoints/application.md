# application endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/application-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| POST | `apps/appinstall (deprecated)` |  |  |
| GET | `apps/apptemplates` |  |  |
| GET | `apps/apptemplates/{name}` |  |  |
| GET | `apps/local` |  |  |
| POST | `apps/local` |  |  |
| GET | `apps/local/{name}` |  |  |
| POST | `apps/local/{name}` |  |  |
| DELETE | `apps/local/{name}` |  |  |
| GET | `apps/local/{name}/package (deprecated)` |  |  |
| GET | `apps/local/{name}/setup` |  |  |
| GET | `apps/local/{name}/update` |  |  |
