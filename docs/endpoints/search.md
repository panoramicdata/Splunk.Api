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
| GET | `search/jobs` | ISearchJobs.ListAsync | SearchJobsTests.ListAsync_SendsGetWithPagingAndFilter |
| POST | `search/jobs` | ISearchJobs.CreateAsync, ISearchJobs.RunOneshotAsync | SearchJobsTests.CreateAsync_SendsEveryTypedParameterAsForm, SearchJobsTests.RunOneshotAsync_SendsOneshotForm |
| POST | `search/v2/jobs/export` | ISearchExport.ExportAsync, ISearchExport.ExportCsvAsync, ISearchExport.ExportRawAsync | SearchExportTests.ExportAsync_SendsPostAndStreamsJsonLines, SearchExportTests.ExportCsvAsync_SendsPostForCsv, SearchExportTests.ExportRawAsync_SendsPostForRawText |
| GET | `search/jobs/export (deprecated)` |  |  |
| POST | `search/jobs/export (deprecated)` |  |  |
| GET | `search/jobs/{search_id}` | ISearchJobs.GetAsync | SearchJobsTests.GetAsync_SendsGetToTheEscapedSid |
| POST | `search/jobs/{search_id}` | ISearchJobs.UpdateAsync | SearchJobsTests.UpdateAsync_SendsCustomProperties |
| DELETE | `search/jobs/{search_id}` | ISearchJobs.DeleteAsync | SearchJobsTests.DeleteAsync_SendsDelete |
| POST | `search/jobs/{search_id}/control` | ISearchJobs.ControlAsync | SearchJobsTests.ControlAsync_SendsArgumentsAndMapsTheMessages |
| GET | `search/v2/jobs/{search_id}/events` | ISearchJobResults.GetEventsAsync, ISearchJobResults.GetEventsRawAsync | SearchJobResultsTests.GetEventsAsync_SendsGetWithEveryOption, SearchJobResultsTests.GetEventsRawAsync_SendsGetForRawText |
| POST | `search/v2/jobs/{search_id}/events` | ISearchJobResults.PostProcessEventsAsync | SearchJobResultsTests.PostProcessEventsAsync_SendsEveryParameterAsForm |
| GET | `search/jobs/{search_id}/events (deprecated)` |  |  |
| GET | `search/v2/jobs/{search_id}/results` | ISearchJobResults.GetResultsAsync, ISearchJobResults.GetResultsCsvAsync | SearchJobResultsTests.GetResultsAsync_SendsGetWithPaging, SearchJobResultsTests.GetResultsCsvAsync_SendsGetForCsvAndStreamsIt |
| POST | `search/v2/jobs/{search_id}/results` | ISearchJobResults.PostProcessResultsAsync | SearchJobResultsTests.PostProcessResultsAsync_SendsTheSearchAsForm |
| GET | `search/jobs/{search_id}/results (deprecated)` |  |  |
| GET | `search/v2/jobs/{search_id}/results_preview` | ISearchJobResults.GetPreviewAsync | SearchJobResultsTests.GetPreviewAsync_SendsGetWithPaging |
| POST | `search/v2/jobs/{search_id}/results_preview` | ISearchJobResults.PostProcessPreviewAsync | SearchJobResultsTests.PostProcessPreviewAsync_SendsTheSearchAsForm |
| GET | `search/jobs/{search_id}/results_preview (deprecated)` |  |  |
| GET | `search/jobs/{search_id}/search.log` | ISearchJobs.GetSearchLogAsync | SearchJobsTests.GetSearchLogAsync_SendsGetAndReturnsTheText |
| GET | `search/jobs/{search_id}/summary` | ISearchJobs.GetSummaryAsync | SearchJobsTests.GetSummaryAsync_SendsGetWithEveryOption |
| GET | `search/jobs/{search_id}/timeline` | ISearchJobs.GetTimelineAsync | SearchJobsTests.GetTimelineAsync_SendsGetWithTimeFormats |
| POST | `search/v2/parser` |  |  |
| GET | `search/parser (deprecated)` |  |  |
| GET | `search/scheduler` |  |  |
| POST | `search/scheduler/status` |  |  |
| GET | `search/timeparser` |  |  |
| GET | `search/typeahead` |  |  |
