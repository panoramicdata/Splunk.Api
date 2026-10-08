# knowledge endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/knowledge-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `admin/summarization` |  |  |
| GET | `admin/summarization/tstats:DM_{app}_{data_model_ID}` |  |  |
| GET | `data/lookup-table-files` |  |  |
| POST | `data/lookup-table-files` |  |  |
| GET | `data/lookup-table-files/{name}` |  |  |
| POST | `data/lookup-table-files/{name}` |  |  |
| DELETE | `data/lookup-table-files/{name}` |  |  |
| GET | `data/props/calcfields` |  |  |
| POST | `data/props/calcfields` |  |  |
| GET | `data/props/calcfields/{name}` |  |  |
| POST | `data/props/calcfields/{name}` |  |  |
| DELETE | `data/props/calcfields/{name}` |  |  |
| GET | `data/props/extractions` |  |  |
| POST | `data/props/extractions` |  |  |
| GET | `data/props/extractions/{name}` |  |  |
| POST | `data/props/extractions/{name}` |  |  |
| DELETE | `data/props/extractions/{name}` |  |  |
| GET | `data/props/fieldaliases` |  |  |
| POST | `data/props/fieldaliases` |  |  |
| GET | `data/props/fieldaliases/{name}` |  |  |
| POST | `data/props/fieldaliases/{name}` |  |  |
| DELETE | `data/props/fieldaliases/{name}` |  |  |
| GET | `data/props/lookups` |  |  |
| POST | `data/props/lookups` |  |  |
| GET | `data/props/lookups/{name}` |  |  |
| POST | `data/props/lookups/{name}` |  |  |
| DELETE | `data/props/lookups/{name}` |  |  |
| GET | `data/props/sourcetype-rename` |  |  |
| POST | `data/props/sourcetype-rename` |  |  |
| GET | `data/props/sourcetype-rename/{name}` |  |  |
| POST | `data/props/sourcetype-rename/{name}` |  |  |
| DELETE | `data/props/sourcetype-rename/{name}` |  |  |
| GET | `data/transforms/extractions` |  |  |
| POST | `data/transforms/extractions` |  |  |
| GET | `data/transforms/extractions/{name}` |  |  |
| POST | `data/transforms/extractions/{name}` |  |  |
| DELETE | `data/transforms/extractions/{name}` |  |  |
| GET | `data/transforms/lookups` |  |  |
| POST | `data/transforms/lookups` |  |  |
| GET | `data/transforms/lookups/{name}` |  |  |
| POST | `data/transforms/lookups/{name}` |  |  |
| DELETE | `data/transforms/lookups/{name}` |  |  |
| GET | `data/transforms/metric-schema` |  |  |
| POST | `data/transforms/metric-schema` |  |  |
| DELETE | `data/transforms/metric-schema` |  |  |
| POST | `data/transforms/statsdextractions` |  |  |
| GET | `data/ui/global-banner` |  |  |
| POST | `data/ui/global-banner` |  |  |
| GET | `data/ui/panels` |  |  |
| POST | `data/ui/panels` |  |  |
| GET | `data/ui/views` |  |  |
| POST | `data/ui/views` |  |  |
| POST | `data/ui/views/{dashboard_id}/disable` |  |  |
| POST | `data/ui/views/{dashboard_id}/enable` |  |  |
| GET | `data/ui/views/{name}` |  |  |
| POST | `data/ui/views/{name}` |  |  |
| DELETE | `data/ui/views/{name}` |  |  |
| GET | `data/ui/views/{name}/history` |  |  |
| GET | `data/ui/views/{name}/revision` |  |  |
| GET | `datamodel/acceleration (deprecated)` |  |  |
| GET | `datamodel/acceleration/{name} (deprecated)` |  |  |
| GET | `datamodel/model` |  |  |
| POST | `datamodel/model` |  |  |
| GET | `datamodel/model/{name}` |  |  |
| POST | `datamodel/model/{name}` |  |  |
| DELETE | `datamodel/model/{name}` |  |  |
| GET | `datamodel/pivot/{name}` |  |  |
| GET | `directory` |  |  |
| GET | `directory/{name}` |  |  |
| GET | `saved/bookmarks/monitoring_console` |  |  |
| POST | `saved/bookmarks/monitoring_console` |  |  |
| DELETE | `saved/bookmarks/monitoring_console` |  |  |
| GET | `saved/eventtypes` |  |  |
| POST | `saved/eventtypes` |  |  |
| GET | `saved/eventtypes/{name}` |  |  |
| POST | `saved/eventtypes/{name}` |  |  |
| DELETE | `saved/eventtypes/{name}` |  |  |
| GET | `search/fields` |  |  |
| GET | `search/fields/{field_name}` |  |  |
| GET | `search/fields/{field_name}/tags` |  |  |
| POST | `search/fields/{field_name}/tags` |  |  |
| GET | `search/tags` |  |  |
| GET | `search/tags/{tag_name}` |  |  |
| POST | `search/tags/{tag_name}` |  |  |
| DELETE | `search/tags/{tag_name}` |  |  |
