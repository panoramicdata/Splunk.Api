# Splunk.Api

[![NuGet](https://img.shields.io/nuget/v/Splunk.Api.svg)](https://www.nuget.org/packages/Splunk.Api)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![CI](https://github.com/panoramicdata/Splunk.Api/actions/workflows/ci.yml/badge.svg)](https://github.com/panoramicdata/Splunk.Api/actions/workflows/ci.yml)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/1c76d7154abe41a4a370da2e8107f8cb)](https://app.codacy.com/gh/panoramicdata/Splunk.Api/dashboard)
[![Codacy Coverage](https://app.codacy.com/project/badge/Coverage/1c76d7154abe41a4a370da2e8107f8cb)](https://app.codacy.com/gh/panoramicdata/Splunk.Api/dashboard)

A typed, modern .NET client for the [Splunk Enterprise REST API](https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/introduction/using-the-rest-api-reference)
(version 10.6), built on Refit and System.Text.Json, plus a client for the HTTP Event Collector.

Splunk.Api is an independent open-source project by Panoramic Data Limited. It is not made, endorsed or supported by
Splunk Inc. Splunk is a trademark of Splunk Inc.

## Installation

```shell
dotnet add package Splunk.Api
```

Splunk.Api targets .NET 10. The package version follows the REST API reference it implements: `10.6.x` targets
Splunk Enterprise 10.6. Most endpoints also work on earlier releases that have them.

## Quick start

```csharp
using Splunk.Api;
using Splunk.Api.Models.Introspection;

using var client = new SplunkClient(new SplunkClientOptions
{
	BaseUrl = "https://splunk.example.com:8089",
	Token = "<a Splunk authentication token>"
});

var info = await client.ServerInfo.GetAsync(cancellationToken);
Console.WriteLine($"Splunk {info.Entries[0].Content!.Version}");

var indexes = await client.Indexes.ListAsync(new IndexListOptions { Count = 0 }, cancellationToken);
foreach (var index in indexes.Entries)
{
	Console.WriteLine($"{index.Name}: {index.Content!.TotalEventCount} events");
}
```

Each group of endpoints is a property of `SplunkClient` (`client.Users`, `client.Indexes`, `client.EventTypes`,
`client.KvStoreData`...). Methods take a required `CancellationToken`; optional parameters travel in an options or
request object, so pass `null` when you need none.

## Authentication

| Option | How it authenticates |
|---|---|
| `Token` | A Splunk authentication token, sent as `Authorization: Bearer`. Recommended. |
| `Username` + `Password` | Logs in through `services/auth/login` and sends the session key; logs in again once if the session expires. |
| `Username` + `Password` + `UseBasicAuthentication = true` | HTTP basic credentials on every request. |

Splunk's management port often uses a self-signed certificate. Rather than turning validation off, pin it:

```csharp
var options = new SplunkClientOptions
{
	BaseUrl = "https://localhost:8089",
	Username = "admin",
	Password = password,
	TrustedServerCertificateThumbprint = "<SHA-256 thumbprint>" // other certificates are still validated normally
};
```

`ServerCertificateValidationCallback` gives full control, and the `SplunkClient(options, innerHandler)` constructor
accepts your own `HttpMessageHandler` (a proxy, or instrumentation).

## Namespaces

Splunk scopes knowledge objects to a user and an app (`servicesNS/{owner}/{app}/...`). Set a default with
`SplunkClientOptions.Namespace`, or get a view of a client in another namespace; views share the client's connections
and session:

```csharp
var search = client.InNamespace("nobody", "search");          // objects shared in the search app
var everything = client.InNamespace(SplunkNamespace.All);       // -/-: every user and app

var eventTypes = await everything.EventTypes.ListAsync(null, cancellationToken);
```

## Creating and changing objects

```csharp
using Splunk.Api.Models.Access;

await client.Users.CreateAsync(new UserCreateRequest
{
	Name = "jsmith",
	Password = initialPassword,
	Roles = ["user"],
	Email = "jsmith@example.com"
}, cancellationToken);
```

Request objects carry Splunk's documented parameters as typed properties. Splunk's open-ended parameter families (for
example `action.<name>.*`) go in `AdditionalParameters`, which every request object has.

## KV store

```csharp
using Splunk.Api.Models.KvStore;

var app = client.InNamespace("nobody", "search");
var key = await app.KvStoreData.InsertAsync("assets", new JsonBody<Asset>(new Asset("web01", 4)), cancellationToken);
var busy = await app.KvStoreData.QueryAsync<Asset>(
	"assets",
	new KvStoreQuery { Query = """{"cpus":{"$gt":2}}""", Sort = "cpus:-1", Limit = 10 },
	cancellationToken);

public sealed record Asset(string Name, int Cpus);
```

## HTTP Event Collector

The HTTP Event Collector is a separate service (port 8088) with its own tokens, so it has its own client:

```csharp
using Splunk.Api.Models.Inputs;

using var hec = new SplunkHecClient(new SplunkHecClientOptions
{
	BaseUrl = "https://splunk.example.com:8088",
	Token = hecToken
});

await hec.SendAsync(
	[new HecEvent { Event = new { message = "deployment finished", version = "2.4.1" }, Sourcetype = "deploy", Index = "main" }],
	cancellationToken);
```

Batches go in one request; indexer acknowledgement is supported through `Channel` and `QueryAcksAsync`.

## Read-only mode

`ReadOnly = true` makes the client refuse, before anything is sent, every request that could change Splunk: any DELETE,
and any POST other than logging in and running, controlling or parsing searches. It raises `SplunkReadOnlyException`.
It cannot police the SPL inside a search (`| delete`, `| outputlookup`), so use a Splunk role without those capabilities
for a truly read-only identity.

## Errors, retries and timeouts

- A non-success response raises `SplunkApiException` with the status code and Splunk's messages.
- 429 and 503 are retried for any verb, other 5xx for idempotent verbs, honouring `Retry-After`, with exponential
  back-off (`MaxRetries`, `RetryBaseDelay`, `MaxRetryDelay`). A connection that could not be established is retried
  too, since nothing was sent.
- `Timeout` applies per attempt and raises `TimeoutException`; cancelling your token raises `OperationCanceledException`.
- Credentials, session keys and query strings (which can carry search text) are never logged; pass an `ILogger` as
  `Logger` to see method, path and retry decisions.

## Coverage

`docs/endpoints/` lists every operation in the Splunk Enterprise 10.6 REST API reference (656), with the client
method that implements it and the test that pins its request. `InventoryTests` keeps that list and the code in step.
Deprecated operations, including the v1 search endpoints Splunk disables by default since 9.0.1, are not implemented.

| Area | Status |
|---|---|
| Access control, users, roles, tokens, SAML/LDAP/MFA/ProxySSO, stored passwords | Complete |
| Apps and licensing | Complete |
| Knowledge objects: event types, tags, field extractions, aliases, lookups, data models, views, macros | Complete |
| Indexes, inputs (monitor, TCP/UDP, scripted, modular, Windows, HEC tokens), outputs, receivers | Complete |
| Configuration files and properties, server settings, messages, introspection, health | Complete |
| KV store collections and data, workload management | Complete |
| Indexer and search head clustering, deployment server, federated search, topology | Complete |
| Search jobs, saved searches, alerts, SPL2, metrics catalog | In progress |

Splunk's reference is wrong in places; where a live Splunk 10.6 behaves differently, the method follows Splunk and its
documentation says so.

## Quality

- Every operation has a unit test pinning its exact HTTP method, path, query and body, and a test mapping a real
  (anonymised) Splunk 10.6 response. The library has 100% line and branch coverage.
- Integration tests run every safe operation against a disposable Splunk Enterprise 10.6 in Docker; see
  [docs/TESTING.md](docs/TESTING.md).
- Zero compiler warnings, nullable reference types, XML documentation on every public member, Codacy grade A.

## Links

- NuGet: https://www.nuget.org/packages/Splunk.Api
- Source: https://github.com/panoramicdata/Splunk.Api
- Issues: https://github.com/panoramicdata/Splunk.Api/issues
- Contributing: [CONTRIBUTING.md](CONTRIBUTING.md), adding endpoints: [docs/IMPLEMENTING.md](docs/IMPLEMENTING.md)

## License

MIT. See [LICENSE](LICENSE).
