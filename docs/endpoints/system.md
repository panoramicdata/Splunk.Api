# system endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/system-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `messages` |  |  |
| POST | `messages` |  |  |
| GET | `messages/{name}` |  |  |
| DELETE | `messages/{name}` |  |  |
| GET | `server/control` |  |  |
| POST | `server/control/restart` |  |  |
| POST | `server/control/restart_webui` |  |  |
| POST | `server/httpsettings/proxysettings` |  |  |
| GET | `server/httpsettings/proxysettings/proxyConfig` |  |  |
| POST | `server/httpsettings/proxysettings/proxyConfig` |  |  |
| DELETE | `server/httpsettings/proxysettings/proxyConfig` |  |  |
| GET | `server/logger` |  |  |
| GET | `server/logger/{name}` |  |  |
| POST | `server/logger/{name}` |  |  |
| GET | `server/roles` |  |  |
| POST | `server/security/rotate-splunk-secret` |  |  |
| GET | `server/settings` |  |  |
