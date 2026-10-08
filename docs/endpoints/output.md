# output endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/output-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `data/outputs/tcp/default` | ITcpOutputDefaults.ListAsync | TcpOutputDefaultsTests.ListAsync_SendsExactRequest |
| POST | `data/outputs/tcp/default` | ITcpOutputDefaults.CreateAsync | TcpOutputDefaultsTests.CreateAsync_SendsExactRequest |
| GET | `data/outputs/tcp/default/{name}` | ITcpOutputDefaults.GetAsync | TcpOutputDefaultsTests.GetAsync_SendsExactRequest |
| POST | `data/outputs/tcp/default/{name}` | ITcpOutputDefaults.UpdateAsync | TcpOutputDefaultsTests.UpdateAsync_SendsExactRequest |
| DELETE | `data/outputs/tcp/default/{name}` | ITcpOutputDefaults.DeleteAsync | TcpOutputDefaultsTests.DeleteAsync_SendsExactRequest |
| GET | `data/outputs/tcp/group` | ITcpOutputGroups.ListAsync | TcpOutputGroupsTests.ListAsync_SendsExactRequest |
| POST | `data/outputs/tcp/group` | ITcpOutputGroups.CreateAsync | TcpOutputGroupsTests.CreateAsync_SendsExactRequest |
| GET | `data/outputs/tcp/group/{name}` | ITcpOutputGroups.GetAsync | TcpOutputGroupsTests.GetAsync_SendsExactRequest |
| POST | `data/outputs/tcp/group/{name}` | ITcpOutputGroups.UpdateAsync | TcpOutputGroupsTests.UpdateAsync_SendsExactRequest |
| DELETE | `data/outputs/tcp/group/{name}` | ITcpOutputGroups.DeleteAsync | TcpOutputGroupsTests.DeleteAsync_SendsExactRequest |
| GET | `data/outputs/tcp/server` | ITcpOutputServers.ListAsync | TcpOutputServersTests.ListAsync_SendsExactRequest |
| POST | `data/outputs/tcp/server` | ITcpOutputServers.CreateAsync | TcpOutputServersTests.CreateAsync_SendsExactRequest |
| GET | `data/outputs/tcp/server/{name}` | ITcpOutputServers.GetAsync | TcpOutputServersTests.GetAsync_SendsExactRequest |
| POST | `data/outputs/tcp/server/{name}` | ITcpOutputServers.UpdateAsync | TcpOutputServersTests.UpdateAsync_SendsExactRequest |
| DELETE | `data/outputs/tcp/server/{name}` | ITcpOutputServers.DeleteAsync | TcpOutputServersTests.DeleteAsync_SendsExactRequest |
| GET | `data/outputs/tcp/server/{name}/allconnections` | ITcpOutputServers.ListConnectionsAsync | TcpOutputServersTests.ListConnectionsAsync_SendsExactRequest |
| GET | `data/outputs/tcp/syslog` | ISyslogOutputs.ListAsync | SyslogOutputsTests.ListAsync_SendsExactRequest |
| POST | `data/outputs/tcp/syslog` | ISyslogOutputs.CreateAsync | SyslogOutputsTests.CreateAsync_SendsExactRequest |
| GET | `data/outputs/tcp/syslog/{name}` | ISyslogOutputs.GetAsync | SyslogOutputsTests.GetAsync_SendsExactRequest |
| POST | `data/outputs/tcp/syslog/{name}` | ISyslogOutputs.UpdateAsync | SyslogOutputsTests.UpdateAsync_SendsExactRequest |
| DELETE | `data/outputs/tcp/syslog/{name}` | ISyslogOutputs.DeleteAsync | SyslogOutputsTests.DeleteAsync_SendsExactRequest |
