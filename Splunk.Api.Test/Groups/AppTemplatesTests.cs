using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class AppTemplatesTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET apps/apptemplates), trimmed; host replaced. The content really is this.
	private const string TemplatesJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/apps/apptemplates",
			"entry": [
				{ "name": "barebones", "author": "system", "content": { "eai:acl": null, "lol": "wut" } },
				{ "name": "sample_app", "author": "system", "content": { "eai:acl": null, "lol": "wut" } }
			],
			"paging": { "total": 2, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.AppTemplates.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/apps/apptemplates");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.AppTemplates.GetAsync("barebones", ct)))
			.ShouldBe(HttpMethod.Get, "/services/apps/apptemplates/barebones");

	[Fact]
	public async Task Content_MapsTheTemplates()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.AppTemplates.ListAsync(null, ct), TemplatesJson);

		feed.Entries.Select(e => e.Name).Should().Equal("barebones", "sample_app");
		feed.Entries[0].Content!.AdditionalProperties["lol"].GetString().Should().Be("wut");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.AppTemplates.GetAsync("missing", ct));
}
