# Changelog

## 10.6.66

- `HecSettingsUpdateRequest.SettingsName` and `GlobalBannerCreateRequest.SingletonName` are static read-only properties
  rather than constants. Source that uses them where a constant is required (a `switch` case, an attribute argument)
  needs a small change; compiled callers are unaffected.
- The form encoder sends only properties with a public getter, as `SplunkFormRequest` always documented.
- `SplunkHecClientOptions` rejects a `Timeout` or `MaxRetryDelay` above `int.MaxValue` milliseconds, as
  `SplunkClientOptions` already did. Both now derive from `SplunkConnectionOptions`.
- Related requests and responses share settings through new base classes (for example `DeploymentServerClassSettings`,
  `FederatedProviderSettings`, `ScheduledContent`). No property, wire name or behaviour changed; a few request bodies and
  query strings send the same fields in a different order.

## 10.6.32

- First release: a typed client for every non-deprecated operation in the Splunk Enterprise 10.6 REST API reference,
  a high-level search runner, KV store document access and an HTTP Event Collector client.
