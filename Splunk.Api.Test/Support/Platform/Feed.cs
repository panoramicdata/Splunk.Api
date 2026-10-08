namespace Splunk.Api.Test.Support.Platform;

/// <summary>
/// Wraps captured entry content in Splunk's JSON feed envelope, so each test file holds only the content it maps.
/// </summary>
internal static class Feed
{
	/// <summary>A feed with one entry named <paramref name="name"/> whose content is <paramref name="content"/> (raw JSON).</summary>
	public static string Of(string name, string content) => Of((name, content));

	/// <summary>A feed with one entry per (name, content) pair.</summary>
	public static string Of(params (string Name, string Content)[] entries)
	{
		var items = entries.Select(e => $$"""
			{
				"name": "{{e.Name}}",
				"id": "https://splunk.test:8089/services/x/{{e.Name}}",
				"updated": "1970-01-01T00:00:00+00:00",
				"links": { "alternate": "/services/x/{{e.Name}}", "list": "/services/x/{{e.Name}}" },
				"author": "system",
				"acl": { "app": "", "can_list": true, "can_write": true, "modifiable": false, "owner": "system", "perms": { "read": ["*"], "write": [] }, "removable": false, "sharing": "system" },
				"content": {{e.Content}}
			}
			""");
		return $$"""
			{
				"links": {},
				"origin": "https://splunk.test:8089/services/x",
				"updated": "2026-10-08T13:49:16+00:00",
				"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
				"entry": [{{string.Join(",", items)}}],
				"paging": { "total": {{entries.Length}}, "perPage": 30, "offset": 0 },
				"messages": []
			}
			""";
	}
}
