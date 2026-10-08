namespace Splunk.Api.Test.Groups;

/// <summary>Saved search and alert responses captured from Splunk Enterprise 10.6.0.5, trimmed.</summary>
internal static class SavedSearchJson
{
	public const string SavedSearchContent = """
		{
			"action.email": false,
			"action.email.to": "ops@example.com",
			"action.email.sendpdf": false,
			"actions": "email",
			"alert.digest_mode": true,
			"alert.expires": "24h",
			"alert.severity": 4,
			"alert.suppress": true,
			"alert.suppress.fields": "host",
			"alert.suppress.period": "1h",
			"alert.track": true,
			"alert_comparator": "greater than",
			"alert_condition": "",
			"alert_threshold": "0",
			"alert_type": "number of events",
			"auto_summarize": false,
			"cron_schedule": "*/5 * * * *",
			"description": "probe",
			"disabled": false,
			"dispatch.buckets": 0,
			"dispatch.earliest_time": "-15m",
			"dispatch.latest_time": "now",
			"dispatch.lookups": true,
			"dispatch.max_count": 500000,
			"dispatch.max_time": 0,
			"dispatch.ttl": "2p",
			"dispatchAs": "owner",
			"display.general.type": "statistics",
			"eai:acl": null,
			"eai:appName": "search",
			"eai:userName": "admin",
			"is_scheduled": true,
			"is_visible": true,
			"max_concurrent": 1,
			"next_scheduled_time": "2026-10-08 13:50:00 UTC",
			"qualifiedSearch": " makeresults count=2",
			"realtime_schedule": true,
			"request.ui_dispatch_app": "search",
			"request.ui_dispatch_view": "search",
			"run_n_times": 0,
			"run_on_startup": false,
			"schedule_priority": "default",
			"schedule_window": "0",
			"scheduled_times": [1791467700, "1791468000"],
			"search": "| makeresults count=2",
			"workload_pool": ""
		}
		""";

	public const string ScheduledViewContent = """
		{
			"action.email": true,
			"action.email.pdfview": "my_view",
			"action.email.subject.view": "Splunk Dashboard: my_view",
			"action.email.to": "nobody@example.com",
			"action.email.useNSSubject": "1",
			"cron_schedule": "0 3 * * *",
			"description": "probe",
			"disabled": false,
			"eai:acl": null,
			"is_scheduled": true,
			"next_scheduled_time": "2026-10-09 03:00:00 UTC",
			"schedule_priority": "default",
			"schedule_window": "0",
			"scheduled_times": [1791514800, 1791601200]
		}
		""";

	public const string HistoryContent = """
		{"eai:acl":null,"isDone":true,"isFinalized":false,"isRealTimeSearch":false,"isSaved":false,"isScheduled":true,"isZombie":false,"start":1791467430,"ttl":600}
		""";

	public const string SuppressionContent = """{"eai:acl":null,"suppressed":true,"suppressionKey":"admin;search;my_alert;;","expiration":"2026-10-08 14:50:00 UTC"}""";

	public const string AlertActionContent = """
		{"command":"sendalert $action_name$ results_file=\"$results.file$\"","description":"Generic HTTP POST to a specified URL","disabled":false,"eai:acl":null,"icon_path":"webhook.png","is_custom":"1","label":"Webhook","maxresults":"10000","maxtime":"5m","payload_format":"json","track_alert":"0","ttl":"10p","param.user_agent":"Splunk/$server.guid$"}
		""";

	public const string FiredAlertContent = """
		{"actions":null,"alert_type":"historical","digest_mode":true,"eai:acl":null,"expiration_time_rendered":"2026-10-09 13:53:26 UTC","savedsearch_name":"my_alert","severity":3,"sid":"scheduler__admin__search__RMD5_at_1791467606_3","trigger_time":1791467606,"trigger_time_rendered":"2026-10-08 13:53:26 UTC","triggered_alerts":1}
		""";

	public const string MetricAlertContent = """
		{"_group_key":"streamalert_556fd83e401db47b","action.email":false,"action.logevent":"0","condition":"'avg(cpu.usage)' > 99","description":"probe","disabled":false,"eai:acl":null,"filter":"host=*","groupby":"host","metric_indexes":"_metrics","trigger.expires":"24h","trigger.max_tracked":5,"trigger.suppress":""}
		""";
}
