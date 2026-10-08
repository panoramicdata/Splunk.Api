# search endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/search-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `alerts/alert_actions` | IAlertActions.ListAsync | AlertActionsTests.ListAsync_SendsGetAndMapsTheAction |
| GET | `alerts/fired_alerts` | IFiredAlerts.ListAsync | FiredAlertsTests.ListAsync_SendsGetAndMapsTheSummary |
| GET | `alerts/fired_alerts/{name}` | IFiredAlerts.GetAsync | FiredAlertsTests.GetAsync_SendsGet |
| DELETE | `alerts/fired_alerts/{name}` | IFiredAlerts.DeleteAsync | FiredAlertsTests.DeleteAsync_SendsDeleteToTheInstance |
| GET | `alerts/metric_alerts` | IMetricAlerts.ListAsync | MetricAlertsTests.ListAsync_SendsGetWithPaging |
| POST | `alerts/metric_alerts` | IMetricAlerts.CreateAsync | MetricAlertsTests.CreateAsync_SendsEveryTypedSettingAsForm |
| GET | `alerts/metric_alerts/{alert_name}` | IMetricAlerts.GetAsync | MetricAlertsTests.GetAsync_SendsGetAndMapsTheAlert |
| POST | `alerts/metric_alerts/{alert_name}` | IMetricAlerts.UpdateAsync | MetricAlertsTests.UpdateAsync_SendsOnlyWhatIsSet |
| DELETE | `alerts/metric_alerts/{alert_name}` | IMetricAlerts.DeleteAsync | MetricAlertsTests.DeleteAsync_SendsDelete |
| GET | `data/commands` | ISearchCommands.ListAsync | SearchCommandsTests.ListAsync_SendsGetWithPaging |
| GET | `data/commands/{name}` | ISearchCommands.GetAsync | SearchCommandsTests.GetAsync_SendsGetToTheCommand |
| GET | `saved/searches` | ISavedSearches.ListAsync | SavedSearchesTests.ListAsync_SendsGetWithEveryOption |
| POST | `saved/searches` | ISavedSearches.CreateAsync | SavedSearchesTests.CreateAsync_SendsEveryTypedSettingAsForm |
| GET | `saved/searches/{name}` | ISavedSearches.GetAsync | SavedSearchesTests.GetAsync_SendsGetToTheEscapedName |
| POST | `saved/searches/{name}` | ISavedSearches.UpdateAsync | SavedSearchesTests.UpdateAsync_SendsOnlyWhatIsSet |
| DELETE | `saved/searches/{name}` | ISavedSearches.DeleteAsync | SavedSearchesTests.DeleteAsync_SendsDelete |
| POST | `saved/searches/{name}/acknowledge` | ISavedSearches.AcknowledgeAsync | SavedSearchesTests.AcknowledgeAsync_SendsTheKey |
| POST | `saved/searches/{name}/dispatch` | ISavedSearches.DispatchAsync | SavedSearchesTests.DispatchAsync_SendsEveryOverrideAndReturnsTheSid |
| GET | `saved/searches/{name}/history` | ISavedSearches.GetHistoryAsync | SavedSearchesTests.GetHistoryAsync_SendsGetWithTheTriplet |
| POST | `saved/searches/{name}/reschedule` | ISavedSearches.RescheduleAsync | SavedSearchesTests.RescheduleAsync_SendsTheTime |
| GET | `saved/searches/{name}/scheduled_times` | ISavedSearches.GetScheduledTimesAsync | SavedSearchesTests.GetScheduledTimesAsync_SendsTheRange |
| GET | `saved/searches/{name}/suppress` | ISavedSearches.GetSuppressionAsync | SavedSearchesTests.GetSuppressionAsync_SendsGetWithTheKey |
| GET | `scheduled/views` | IScheduledViews.ListAsync | ScheduledViewsTests.ListAsync_SendsGetWithPaging |
| GET | `scheduled/views/{name}` | IScheduledViews.GetAsync | ScheduledViewsTests.GetAsync_SendsGet |
| POST | `scheduled/views/{name}` | IScheduledViews.UpdateAsync | ScheduledViewsTests.UpdateAsync_SendsTheSchedule |
| DELETE | `scheduled/views/{name}` | IScheduledViews.DeleteAsync | ScheduledViewsTests.DeleteAsync_SendsDelete |
| POST | `scheduled/views/{name}/dispatch` | IScheduledViews.DispatchAsync | ScheduledViewsTests.DispatchAsync_SendsTheOverrides |
| GET | `scheduled/views/{name}/history` | IScheduledViews.GetHistoryAsync | ScheduledViewsTests.GetHistoryAsync_SendsGet |
| POST | `scheduled/views/{name}/reschedule` | IScheduledViews.RescheduleAsync | ScheduledViewsTests.RescheduleAsync_SendsTheTime |
| GET | `scheduled/views/{name}/scheduled_times` | IScheduledViews.GetScheduledTimesAsync | ScheduledViewsTests.GetScheduledTimesAsync_SendsTheRange |
| GET | `search/concurrency-settings` | ISearchConcurrencySettings.ListAsync | SearchConcurrencySettingsTests.ListAsync_SendsGet |
| POST | `search/concurrency-settings/scheduler` | ISearchConcurrencySettings.UpdateSchedulerAsync | SearchConcurrencySettingsTests.UpdateSchedulerAsync_SendsThePercentages |
| POST | `search/concurrency-settings/search` | ISearchConcurrencySettings.UpdateSearchAsync | SearchConcurrencySettingsTests.UpdateSearchAsync_SendsTheLimits |
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
| POST | `search/v2/parser` | ISearchParser.ParseAsync | SearchParserTests.ParseAsync_SendsPostWithEveryOption |
| GET | `search/parser (deprecated)` |  |  |
| GET | `search/scheduler` | ISearchScheduler.GetStatusAsync | SearchSchedulerTests.GetStatusAsync_SendsGet |
| POST | `search/scheduler/status` | ISearchScheduler.SetStatusAsync | SearchSchedulerTests.SetStatusAsync_SendsDisabled |
| GET | `search/timeparser` | ISearchTimeParser.ParseAsync | SearchTimeParserTests.ParseAsync_SendsEachTimeAndTheOptions |
| GET | `search/typeahead` | ISearchTypeahead.GetAsync | SearchTypeaheadTests.GetAsync_SendsThePrefixAndCount |
