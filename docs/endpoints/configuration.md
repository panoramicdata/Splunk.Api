# configuration endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/configuration-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `configs/conf-{file}` |  |  |
| POST | `configs/conf-{file}` |  |  |
| GET | `configs/conf-{file}/{stanza}` |  |  |
| POST | `configs/conf-{file}/{stanza}` |  |  |
| DELETE | `configs/conf-{file}/{stanza}` |  |  |
| GET | `properties` |  |  |
| POST | `properties` |  |  |
| GET | `properties/{file}` |  |  |
| POST | `properties/{file}` |  |  |
| GET | `properties/{file}/{stanza}` |  |  |
| POST | `properties/{file}/{stanza}` |  |  |
| DELETE | `properties/{file}/{stanza}` |  |  |
| GET | `properties/{file}/{stanza}/{key}` |  |  |
| POST | `properties/{file}/{stanza}/{key}` |  |  |
| DELETE | `properties/{file}/{stanza}/{key}` |  |  |
