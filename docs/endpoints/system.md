# system endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/system-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `messages` | IMessages.ListAsync | MessagesTests.ListAsync_SendsGet |
| POST | `messages` | IMessages.CreateAsync | MessagesTests.CreateAsync_PostsEveryField |
| GET | `messages/{name}` | IMessages.GetAsync | MessagesTests.GetAsync_SendsGet |
| DELETE | `messages/{name}` | IMessages.DeleteAsync | MessagesTests.DeleteAsync_SendsDelete |
| GET | `server/control` | IServerControl.ListAsync | ServerControlTests.ListAsync_SendsGet |
| POST | `server/control/restart` | IServerControl.RestartAsync | ServerControlTests.RestartAsync_PostsWithNoBody |
| POST | `server/control/restart_webui` | IServerControl.RestartWebUIAsync | ServerControlTests.RestartWebUIAsync_PostsWithNoBody |
| POST | `server/httpsettings/proxysettings` | IProxySettings.CreateAsync | ProxySettingsTests.CreateAsync_PostsTheNameAndProxies |
| GET | `server/httpsettings/proxysettings/proxyConfig` | IProxySettings.GetAsync | ProxySettingsTests.GetAsync_SendsGet |
| POST | `server/httpsettings/proxysettings/proxyConfig` | IProxySettings.UpdateAsync | ProxySettingsTests.UpdateAsync_PostsTheProxies |
| DELETE | `server/httpsettings/proxysettings/proxyConfig` | IProxySettings.DeleteAsync | ProxySettingsTests.DeleteAsync_SendsDelete |
| GET | `server/logger` | ILoggers.ListAsync | LoggersTests.ListAsync_SendsGet |
| GET | `server/logger/{name}` | ILoggers.GetAsync | LoggersTests.GetAsync_SendsGet |
| POST | `server/logger/{name}` | ILoggers.UpdateAsync | LoggersTests.UpdateAsync_PostsTheLevel |
| GET | `server/roles` | IServerRoles.GetAsync | ServerRolesTests.GetAsync_SendsGet |
| POST | `server/security/rotate-splunk-secret` | IServerSecurity.RotateSplunkSecretAsync | ServerSecurityTests.RotateSplunkSecretAsync_PostsWithNoBody |
| GET | `server/settings` | IServerSettings.GetAsync | ServerSettingsTests.GetAsync_SendsGet |
