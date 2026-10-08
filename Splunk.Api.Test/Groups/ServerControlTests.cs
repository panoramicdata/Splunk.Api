using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ServerControlTests
{
	// Captured from Splunk 10.6.0.5: GET server/control has no entries, only links.
	private const string ControlJson = """
		{
			"links": {
				"restart": "/services/server/control/restart",
				"restart_webui": "/services/server/control/restart_webui",
				"reload_ssl_config": "/services/server/control/reload_ssl_config"
			},
			"origin": "https://splunk.test:8089/services/server/control",
			"updated": "2026-10-08T13:49:16+00:00",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [],
			"paging": { "total": 0, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ServerControl.ListAsync(Calls.Token), HttpMethod.Get, "/services/server/control", Calls.JsonQuery, null);

	[Fact]
	public async Task RestartAsync_PostsWithNoBody()
	{
		var call = await Calls.AssertAsync(c => c.ServerControl.RestartAsync(Calls.Token), HttpMethod.Post, "/services/server/control/restart", Calls.JsonQuery, null);

		call.ContentType.Should().BeNull();
	}

	[Fact]
	public async Task RestartWebUIAsync_PostsWithNoBody()
		=> await Calls.AssertAsync(c => c.ServerControl.RestartWebUIAsync(Calls.Token), HttpMethod.Post, "/services/server/control/restart_webui", Calls.JsonQuery, null);

	[Fact]
	public async Task ListAsync_MapsTheActionLinks()
	{
		var feed = await Calls.MapAsync(c => c.ServerControl.ListAsync(Calls.Token), ControlJson);

		feed.Entries.Should().BeEmpty();
		feed.Links.Should().ContainKeys("restart", "restart_webui", "reload_ssl_config");
	}

	[Fact]
	public async Task RestartAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.ServerControl.RestartAsync(Calls.Token), HttpStatusCode.Forbidden);
}
