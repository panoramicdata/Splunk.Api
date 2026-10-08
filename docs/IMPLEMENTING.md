# Implementing endpoints

This is the contract for adding Splunk REST operations to Splunk.Api. `server/info` (`IServerInfo`,
`Models/Introspection/ServerInfo`, `ServerInfoTests`) is the worked example; copy its shape.

## Source of truth

- `docs/endpoints/<category>.md` lists every operation of the Splunk Enterprise **10.6** REST API reference, one row per
  path and method (656 in all). The reference is at
  `https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/<category>-endpoints`; read the endpoint's own
  section for its parameters and response fields before writing it.
- When an operation is implemented, fill its row: `Client method` = `IInterface.Method` (several, comma-separated, when
  more than one method serves the operation, for example a typed and a raw variant), `Test` = `TestClass.TestMethod` of
  the unit test that pins its request. `InventoryTests` checks that each named method exists and that its verb and path
  match the row (placeholder names are ignored), that each test exists, and that every Refit method appears in a row.
- Rows marked `(deprecated)` are **not** implemented. The v1 search endpoints are disabled by default since Splunk 9.0.1;
  the `search/v2/...` ones replace them.
- A category leaves `docs/pending-categories.txt` once every non-deprecated row is filled; from then on `InventoryTests`
  enforces it.
- Where the reference is wrong (it has a few path and method errors), follow what a live Splunk 10.6 does, and say so in
  the method's XML `<remarks>`.

## Layout and naming

- **Interfaces:** `Splunk.Api/Interfaces/I<Family>.cs`, namespace `Splunk.Api.Interfaces`, one interface per endpoint
  family (a path prefix such as `saved/searches`, `data/indexes`, `authentication/users`). Name it for the resource in
  the plural: `ISavedSearches`, `IIndexes`, `IUsers`. Interface names must be unique across the library.
- **Client properties:** one partial file per category, `Splunk.Api/SplunkClient.<Category>.cs`, with one line per
  interface: `public ISavedSearches SavedSearches => field ??= For<ISavedSearches>();` plus an XML summary naming the
  endpoint family. Property names must be unique.
- **Models:** `Splunk.Api/Models/<Category>/`, namespace `Splunk.Api.Models.<Category>`, one public type per file.
  Category folder names: `Access`, `Applications`, `Cluster`, `Configuration`, `Deployment`, `FederatedSearch`,
  `Inputs`, `Introspection`, `Knowledge`, `KvStore`, `Licensing`, `MetricsCatalog`, `Outputs`, `Search`, `Spl2`,
  `Server` (the "system" category: never name a namespace `System`), `Topology`, `WorkloadManagement`.
- Shared types already exist in `Splunk.Api.Models`: `SplunkFeed<T>`, `SplunkEntry<T>`, `SplunkAcl`, `SplunkPaging`,
  `SplunkMessage`, `SplunkContent`, `SplunkDynamicContent`, `ListOptions`, `SortDirection`, `SortMode`,
  `SplunkFormRequest`, `JsonBody<T>`, `AclUpdateRequest` (for `.../{name}/acl`) and `MoveRequest` (for
  `.../{name}/move`). Reuse them; do not duplicate them. Do not edit files outside your category without saying so in
  your report.

## Interface methods

- Refit attribute paths are relative with the `services/` prefix and no leading slash: `[Get("services/saved/searches")]`.
  The client sends them to `servicesNS/{owner}/{app}/...` when a namespace is set (`client.InNamespace(...)`), so do not
  write `servicesNS` paths and do not add owner/app parameters.
- Every method is `async`-shaped (`Task`/`Task<T>`), takes a **required** `CancellationToken cancellationToken` last, and
  has **no optional parameters** (Sonar S2360). Optional query parameters travel in one nullable options object
  (`[Query] XListOptions? options`; callers pass `null`), optional body fields in the request model.
- Path parameters are escaped as one segment by Refit (`a/b` becomes `a%2Fb`). Name C# parameters in camelCase and use
  the C# name in the attribute's braces (`[Get("services/search/v2/jobs/{searchId}/results")]`).
- Standard shapes for an entity collection `X` (adapt to what the endpoint really supports):
  - list: `Task<SplunkFeed<X>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken)`; when the
    endpoint has its own GET parameters, derive `XListOptions : ListOptions` and add them with `[AliasAs("wire_name")]`.
  - get: `Task<SplunkFeed<X>> GetAsync(string name, CancellationToken cancellationToken)` (Splunk returns a feed of one).
  - create: `Task<SplunkFeed<X>> CreateAsync([Body] XCreateRequest request, CancellationToken cancellationToken)`.
  - update: `Task<SplunkFeed<X>> UpdateAsync(string name, [Body] XUpdateRequest request, CancellationToken cancellationToken)`.
  - delete: `Task DeleteAsync(string name, CancellationToken cancellationToken)`.
  - sub-actions such as `enable`, `disable`, `_reload`, `acl`, `move`, `dispatch` get their own method when the
    reference documents the path.
- Responses that are not the JSON feed envelope: return a specific model (search results, KV store documents, SPL2 and
  HEC replies), `Task<string>` for plain text (for example `search.log`), or `Task<HttpContent>` / `Task<Stream>` for a
  stream the caller reads and disposes (exports). Choose an explicit non-JSON output mode in the path
  (`[Get("services/...?output_mode=raw")]`); the client only adds `output_mode=json` when none is present.
- XML documentation on every public member: what the operation does, the HTTP verb and path in `<c>...</c>`, required
  capabilities, and anything surprising (eventual consistency, restart needed, Splunk Cloud differences).

## Request models

- Derive from `SplunkFormRequest`. Each property is one form field; put `[JsonPropertyName("wire.name")]` on **every**
  property (wire names are often dotted or camelCase). Splunk's required parameters use the C# `required` modifier;
  everything else is nullable and omitted when `null`. Booleans go as `true`/`false`, enums by
  `[JsonStringEnumMemberName]`, lists as repeated fields.
- Open-ended parameter families (`action.<name>.*`, `args.*`, `dispatch.*` beyond the common ones, conf keys) go in
  `AdditionalParameters`, which every request already has. Model the documented, commonly used parameters as typed
  properties.
- Endpoints whose fields are entirely free-form (configuration file stanzas) may take
  `[Body] IDictionary<string, string?> fields` instead of a model.
- JSON request bodies (KV store data, SPL2) use `[Body] JsonBody<T>`.

## Response models

- Entry content derives from `SplunkContent`, which keeps every unmodelled property in `AdditionalProperties`, so model
  the documented and commonly used fields, not necessarily all of them.
- `[JsonPropertyName("wire")]` on every property. Numbers are `int`/`long`/`double` (nullable when Splunk may omit them);
  booleans `bool` or `bool?`; epoch seconds `DateTimeOffset?` with `[JsonConverter(typeof(EpochSecondsConverter))]`
  (namespace `Splunk.Api.Serialization`); ISO timestamps `DateTimeOffset?`; lists `IReadOnlyList<T>` defaulting to `[]`;
  strings `string?`. `SplunkJson.Options` already reads Splunk's loose types (`"1"`, `"true"`, `""`, quoted numbers),
  so do not add converters for those.
- Enums only for small closed sets; first member `Unknown = 0`, the rest with `[JsonStringEnumMemberName("wire")]`.
  Unrecognised values read as `Unknown`. Prefer `string` where Splunk's set is open or version-dependent.

## Tests (Splunk.Api.Test, xUnit v3 + AwesomeAssertions)

- One test class per interface in `Splunk.Api.Test/Groups/<Interface without I>Tests.cs` (split into partial files when it
  grows; Codacy grades a file down past about 40 total complexity or 300 lines), using `TestClient.Create(stub)` and
  `TestClient.Stub(json)` from `Support/`.
- **Every operation** has a test asserting the exact method, path (`Uri.AbsolutePath`), query and form body
  (`RecordedCall.Body`), and one that maps a realistic response, asserting every modelled field. Capture realistic
  JSON from the live test Splunk (see below), then replace host names, GUIDs and keys with neutral values.
- Each interface has at least one error-path test (non-success status raises `SplunkApiException` with the message).
- No skipped tests (`failSkips` is on), no `Thread.Sleep`, no network. Avoid copy-pasted test bodies: shared JSON goes in
  constants or a fixture class, repeated assertions in a helper, so Codacy finds no duplication.
- Line and branch coverage of what you add should be 100%.

## Live verification (Splunk.Api.IntegrationTest)

- A disposable Splunk Enterprise 10.6 runs in Docker (container `splunk-api-test`). Its URL, `admin` credentials and
  certificate thumbprint are in this project's user secrets (`Splunk:BaseUrl`, `Splunk:Username`, `Splunk:Password`,
  `Splunk:TrustedServerCertificateThumbprint`). Use them through `SplunkFixture`; **never print or commit them**. To
  capture a raw response, write a throwaway test or script under `temp/` (git-ignored) that reads the secrets itself.
- Add integration tests under `Splunk.Api.IntegrationTest/<Category>/` using `[Collection(SplunkTestGroup.Name)]` and
  `SplunkFixture`: every read operation the standalone instance supports, and create, update and delete round trips for
  objects you can safely create. Name every object you create with `SplunkFixture.UniqueName(...)` (prefix
  `splunk_api_it_`) and delete it in the same test (try/finally). Nothing is skipped: where a feature needs a cluster,
  a forwarder or Splunk Cloud, assert the error a standalone instance returns instead.
- The instance is shared by several people working at once. **Never**: restart or shut it down, enable clustering or
  deployment server roles, change licensing, change or delete anything not created by your own test, change the `admin`
  password, or run SPL that deletes data. Writes are limited to objects named with the test prefix.

## Code quality

- Zero warnings (`TreatWarningsAsErrors`), XML docs on all public API, file-scoped namespaces, tabs, CRLF. Files written
  by tools are LF: run `dotnet format whitespace <project>` before building.
- Keep each file small and simple (Codacy grade A): split large interfaces or models into partial files or separate
  families rather than growing one file.
- Build only the projects you touched: `dotnet build Splunk.Api.Test/Splunk.Api.Test.csproj`, then
  `dotnet test --project Splunk.Api.Test/Splunk.Api.Test.csproj --no-build` (exit code 0 = all passed).
