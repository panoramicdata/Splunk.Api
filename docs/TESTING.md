# Testing

## Unit tests

`Splunk.Api.Test` needs no network and runs in CI on every push and pull request:

```shell
dotnet test --project Splunk.Api.Test/Splunk.Api.Test.csproj
```

Every operation has a test that pins its exact HTTP method, path, query string and form body, and one that maps a
realistic response (captured from a live Splunk 10.6 and anonymised). Skipped tests fail the run (`failSkips`).
`InventoryTests` keeps `docs/endpoints/*.md` and the Refit interfaces in step.

Coverage, as CI collects it:

```shell
dotnet build Splunk.Api.Test/Splunk.Api.Test.csproj
./Splunk.Api.Test/bin/Debug/net10.0/Splunk.Api.Test --coverage --coverage-settings "$PWD/coverage.config" --coverage-output-format cobertura --coverage-output coverage.cobertura.xml
```

(`--coverage-settings` must be an absolute path.)

## Integration tests

`Splunk.Api.IntegrationTest` runs against a real Splunk Enterprise. The easiest is a disposable one in Docker:

```powershell
./docker/Start-SplunkTestInstance.ps1
dotnet test --project Splunk.Api.IntegrationTest/Splunk.Api.IntegrationTest.csproj
```

The script starts `splunk/splunk` as container `splunk-api-test`, waits for it, and stores the URL, a generated `admin`
password and the pinned certificate thumbprint in the project's user secrets. To use another instance, set
`Splunk:BaseUrl` and either `Splunk:Token` or `Splunk:Username` + `Splunk:Password` (plus
`Splunk:TrustedServerCertificateThumbprint` for a self-signed certificate) with `dotnet user-secrets`, or as
`Splunk__BaseUrl` etc. environment variables. A missing setting fails the tests with a message naming it.

The tests create only objects named `splunk_api_it_...` and delete them as they go. They never restart Splunk, change
licensing, clustering or server settings. Even so, point them only at a disposable or test instance.

CI does not run the integration tests: that would need Splunk running inside every build.
