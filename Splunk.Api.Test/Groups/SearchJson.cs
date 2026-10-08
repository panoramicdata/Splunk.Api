namespace Splunk.Api.Test.Groups;

/// <summary>Search responses captured from Splunk Enterprise 10.6.0.5 (docker splunk/splunk), trimmed; host names replaced.</summary>
internal static class SearchJson
{
	public const string JobContent = """
		{
			"canSummarize": false,
			"cursorTime": "1970-01-01T00:00:00.000+00:00",
			"defaultSaveTTL": "604800",
			"defaultTTL": "600",
			"delegate": "",
			"diskUsage": 65536,
			"dispatchState": "DONE",
			"doneProgress": 1,
			"dropCount": 0,
			"earliestTime": "2026-10-08T12:47:15.000+00:00",
			"eventAvailableCount": 5,
			"eventCount": 5,
			"eventFieldCount": 15,
			"eventIsStreaming": true,
			"eventIsTruncated": false,
			"eventSearch": "search index=_internal ",
			"eventSorting": "desc",
			"isDone": true,
			"isFailed": false,
			"isFinalized": false,
			"isPaused": false,
			"isPreviewEnabled": true,
			"isRealTimeSearch": false,
			"isSaved": false,
			"isSavedSearch": false,
			"isZombie": false,
			"keywords": "index::_internal",
			"label": "",
			"latestTime": "2026-10-08T13:47:15.000+00:00",
			"numPreviews": 2,
			"optimizedSearch": "| search index=_internal | head 5",
			"pid": "7395",
			"priority": 5,
			"provenance": "rest:jobs",
			"remoteSearch": "litsearch index=_internal | fields keepcolorder=t \"_raw\"",
			"reportSearch": "head 5",
			"resultCount": 5,
			"resultIsStreaming": false,
			"resultPreviewCount": 5,
			"runDuration": 0.014,
			"scanCount": 12,
			"search": "search index=_internal | head 5",
			"searchEarliestTime": 1791463635,
			"searchLatestTime": "1791467235.5",
			"sid": "splunk_api_probe_2",
			"statusBuckets": 300,
			"ttl": 600,
			"workload_pool": "",
			"performance": { "command.head": { "invocations": 1, "input_count": 5, "output_count": 5 } },
			"messages": [ { "type": "INFO", "text": "Your timerange was substituted." } ],
			"request": { "exec_mode": "blocking", "id": "splunk_api_probe_2", "search": "search index=_internal | head 5", "status_buckets": "300" },
			"runtime": { "auto_cancel": "0", "auto_pause": "0" },
			"searchProviders": ["splunk01"]
		}
		""";

	public const string FailedJobContent = """
		{
			"dispatchState": "FAILED", "isDone": true, "isFailed": true, "sid": "failed_sid",
			"messages": [
				{ "type": "FATAL", "text": "Error in 'EvalCommand': The 'nosuchfunc' function is unsupported or undefined." },
				{ "type": "ERROR", "text": "Error in 'EvalCommand': The 'nosuchfunc' function is unsupported or undefined." }
			]
		}
		""";

	public const string Results = """
		{"preview":false,"init_offset":0,"post_process_count":3,"messages":[{"type":"WARN","text":"Some warning."}],"fields":[{"name":"_time"},{"name":"mv"},{"name":"x","type":"str"},{"name":"host","groupby_rank":"0","summary.count":"5"}],"results":[{"_time":"2026-10-08T13:47:15.000+00:00","mv":["a","b"],"x":"2123633747","host":"splunk01"},{"_time":"2026-10-08T13:47:16.000+00:00","mv":["c"],"x":"250048592","n":null}], "highlighted":{"0":{"x":[]}}}
		""";

	public const string EmptyResults = """
		{"preview":false,"init_offset":0,"post_process_count":0,"messages":[],"results":[]}
		""";

	public const string Summary = """
		{"earliest_time":"2026-10-08T12:47:00.000+00:00","latest_time":"2026-10-08T13:47:00.000+00:00","duration":3600,"event_count":5,"fields":{"linecount":{"count":5,"numeric_count":5,"distinct_count":1,"is_exact":true,"relevant":false,"min":"1","max":"1","mean":1,"stdev":0,"modes":[{"value":"1","count":5,"is_exact":true}]},"host":{"count":5,"numeric_count":0,"distinct_count":1,"is_exact":true,"relevant":true,"modes":[{"value":"splunk01","count":5,"is_exact":true}]}},"histogram":[]}
		""";

	public const string Timeline = """
		{"cursor_time":0,"is_time_cursored":true,"buckets":[{"total_count":0,"available_count":0,"is_finalized":true,"duration":60,"earliest_strftime":"2026-10-08T13:46:00.000+00:00","earliest_time":1791467160,"earliest_time_offset":0,"latest_time_offset":0},{"total_count":5,"available_count":5,"is_finalized":false,"duration":60,"earliest_strftime":"2026-10-08T13:47:00.000+00:00","earliest_time":1791467220,"earliest_time_offset":3600,"latest_time_offset":3600}],"event_count":5}
		""";

	public const string Export = """
		{"preview":true,"offset":0,"result":{"sourcetype":"splunkd","count":"1"}}

		{"preview":false,"offset":0,"result":{"_time":"2026-10-08 13:48:34.000 GMT","x":"1"}}
		{"preview":false,"offset":1,"lastrow":true,"result":{"_time":"2026-10-08 13:48:35.000 GMT","x":["1","2"]}}
		{"preview":false,"lastrow":true}
		""";
}
