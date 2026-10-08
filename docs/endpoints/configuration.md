# configuration endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/configuration-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `configs/conf-{file}` | IConfigs.ListStanzasAsync | ConfigsTests.ListStanzasAsync_SendsGetWithTheFileInThePath |
| POST | `configs/conf-{file}` | IConfigs.CreateStanzaAsync | ConfigsTests.CreateStanzaAsync_PostsTheNameThenTheKeys |
| GET | `configs/conf-{file}/{stanza}` | IConfigs.GetStanzaAsync | ConfigsTests.GetStanzaAsync_EscapesTheStanzaAsOneSegment |
| POST | `configs/conf-{file}/{stanza}` | IConfigs.UpdateStanzaAsync | ConfigsTests.UpdateStanzaAsync_PostsTheKeys |
| DELETE | `configs/conf-{file}/{stanza}` | IConfigs.DeleteStanzaAsync | ConfigsTests.DeleteStanzaAsync_SendsDelete |
| GET | `properties` | IConfigProperties.ListFilesAsync | ConfigPropertiesTests.ListFilesAsync_SendsGet |
| POST | `properties` | IConfigProperties.CreateFileAsync | ConfigPropertiesTests.CreateFileAsync_PostsConf |
| GET | `properties/{file}` | IConfigProperties.ListStanzasAsync | ConfigPropertiesTests.ListStanzasAsync_SendsGet |
| POST | `properties/{file}` | IConfigProperties.CreateStanzaAsync | ConfigPropertiesTests.CreateStanzaAsync_PostsStanza |
| GET | `properties/{file}/{stanza}` | IConfigProperties.GetStanzaAsync | ConfigPropertiesTests.GetStanzaAsync_SendsGet |
| POST | `properties/{file}/{stanza}` | IConfigProperties.UpdateStanzaAsync | ConfigPropertiesTests.UpdateStanzaAsync_PostsTheKeys |
| DELETE | `properties/{file}/{stanza}` | IConfigProperties.DeleteStanzaAsync | ConfigPropertiesTests.DeleteStanzaAsync_SendsDeleteLocalOnly |
| GET | `properties/{file}/{stanza}/{key}` | IConfigProperties.GetValueAsync | ConfigPropertiesTests.GetValueAsync_SendsGet |
| POST | `properties/{file}/{stanza}/{key}` | IConfigProperties.SetValueAsync | ConfigPropertiesTests.SetValueAsync_PostsValue |
| DELETE | `properties/{file}/{stanza}/{key}` | IConfigProperties.DeleteValueAsync | ConfigPropertiesTests.DeleteValueAsync_SendsDeleteLocalOnly |
