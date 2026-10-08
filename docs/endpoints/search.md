# search endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/search-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `alerts/alert_actions` |  |  |
| GET | `alerts/fired_alerts` |  |  |
| GET | `alerts/fired_alerts/{name}` |  |  |
| DELETE | `alerts/fired_alerts/{name}` |  |  |
| GET | `alerts/metric_alerts` |  |  |
| POST | `alerts/metric_alerts` |  |  |
| GET | `alerts/metric_alerts/{alert_name}` |  |  |
| POST | `alerts/metric_alerts/{alert_name}` |  |  |
| DELETE | `alerts/metric_alerts/{alert_name}` |  |  |
| GET | `data/commands` |  |  |
| GET | `data/commands/{name}` |  |  |
| GET | `saved/searches` |  |  |
| POST | `saved/searches` |  |  |
| GET | `saved/searches/{name}` |  |  |
| POST | `saved/searches/{name}` |  |  |
| DELETE | `saved/searches/{name}` |  |  |
| POST | `saved/searches/{name}/acknowledge` |  |  |
| POST | `saved/searches/{name}/dispatch` |  |  |
| GET | `saved/searches/{name}/history` |  |  |
| POST | `saved/searches/{name}/reschedule` |  |  |
| GET | `saved/searches/{name}/scheduled_times` |  |  |
| GET | `saved/searches/{name}/suppress` |  |  |
| GET | `scheduled/views` |  |  |
| GET | `scheduled/views/{name}` |  |  |
| POST | `scheduled/views/{name}` |  |  |
| DELETE | `scheduled/views/{name}` |  |  |
| POST | `scheduled/views/{name}/dispatch` |  |  |
| GET | `scheduled/views/{name}/history` |  |  |
| POST | `scheduled/views/{name}/reschedule` |  |  |
| GET | `scheduled/views/{name}/scheduled_times` |  |  |
| GET | `search/concurrency-settings` |  |  |
| POST | `search/concurrency-settings/scheduler` |  |  |
| POST | `search/concurrency-settings/search` |  |  |
| GET | `search/jobs` |  |  |
| POST | `search/jobs` |  |  |
| POST | `search/v2/jobs/export` |  |  |
| GET | `search/jobs/export (deprecated)` |  |  |
| POST | `search/jobs/export (deprecated)` |  |  |
| GET | `search/jobs/{search_id}` |  |  |
| POST | `search/jobs/{search_id}` |  |  |
| DELETE | `search/jobs/{search_id}` |  |  |
| POST | `search/jobs/{search_id}/control` |  |  |
| GET | `search/v2/jobs/{search_id}/events` |  |  |
| POST | `search/v2/jobs/{search_id}/events` |  |  |
| GET | `search/jobs/{search_id}/events (deprecated)` |  |  |
| GET | `search/v2/jobs/{search_id}/results` |  |  |
| POST | `search/v2/jobs/{search_id}/results` |  |  |
| GET | `search/jobs/{search_id}/results (deprecated)` |  |  |
| GET | `search/v2/jobs/{search_id}/results_preview` |  |  |
| POST | `search/v2/jobs/{search_id}/results_preview` |  |  |
| GET | `search/jobs/{search_id}/results_preview (deprecated)` |  |  |
| GET | `search/jobs/{search_id}/search.log` |  |  |
| GET | `search/jobs/{search_id}/summary` |  |  |
| GET | `search/jobs/{search_id}/timeline` |  |  |
| POST | `search/v2/parser` |  |  |
| GET | `search/parser (deprecated)` |  |  |
| GET | `search/scheduler` |  |  |
| POST | `search/scheduler/status` |  |  |
| GET | `search/timeparser` |  |  |
| GET | `search/typeahead` |  |  |
