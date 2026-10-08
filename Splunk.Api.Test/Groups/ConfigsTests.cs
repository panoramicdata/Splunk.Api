using Splunk.Api.Models;
using Splunk.Api.Models.Configuration;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ConfigsTests
{
	// Captured from Splunk 10.6.0.5: GET servicesNS/nobody/search/configs/conf-splunk_api_it_probeconf/stanza%2Fone (content).
	private const string StanzaContent = """
		{
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "search",
			"eai:userName": "nobody",
			"key1": "v1",
			"key2": "v=2",
			"count": 3,
			"enabled": true,
			"list": ["a", "b"],
			"nothing": null
		}
		""";

	[Fact]
	public async Task ListStanzasAsync_SendsGetWithTheFileInThePath()
		=> await Calls.AssertAsync(
			c => c.Configs.ListStanzasAsync("server", new ListOptions { Count = 2 }, Calls.Token),
			HttpMethod.Get, "/services/configs/conf-server", "?count=2&output_mode=json", null);

	[Fact]
	public async Task CreateStanzaAsync_PostsTheNameThenTheKeys()
		=> await Calls.AssertAsync(
			c => c.Configs.CreateStanzaAsync(
				"my_app",
				new ConfStanzaCreateRequest { Name = "stanza/one", AdditionalParameters = { ["key1"] = "v1", ["key2"] = "v=2" } },
				Calls.Token),
			HttpMethod.Post, "/services/configs/conf-my_app", Calls.JsonQuery, "name=stanza%2Fone&key1=v1&key2=v%3D2");

	[Fact]
	public async Task GetStanzaAsync_EscapesTheStanzaAsOneSegment()
	{
		var call = await Calls.AssertAsync(
			c => c.Configs.GetStanzaAsync("my_app", "stanza/one:x", Calls.Token),
			HttpMethod.Get, "/services/configs/conf-my_app/stanza%2Fone%3Ax", Calls.JsonQuery, null);

		call.Uri.OriginalString.Should().Contain("conf-my_app/stanza%2Fone%3Ax?");
	}

	[Fact]
	public async Task UpdateStanzaAsync_PostsTheKeys()
		=> await Calls.AssertAsync(
			c => c.Configs.UpdateStanzaAsync("my_app", "general", new Dictionary<string, string?> { ["key1"] = "changed", ["cleared"] = null }, Calls.Token),
			HttpMethod.Post, "/services/configs/conf-my_app/general", Calls.JsonQuery, "key1=changed&cleared=");

	[Fact]
	public async Task DeleteStanzaAsync_SendsDelete()
		=> await Calls.AssertAsync(
			c => c.Configs.DeleteStanzaAsync("my_app", "general", Calls.Token),
			HttpMethod.Delete, "/services/configs/conf-my_app/general", Calls.JsonQuery, null);

	[Fact]
	public async Task GetStanzaAsync_MapsFreeFormKeysAsText()
	{
		var feed = await Calls.MapAsync(c => c.Configs.GetStanzaAsync("f", "stanza/one", Calls.Token), Feed.Of("stanza/one", StanzaContent));

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("stanza/one");
		var stanza = entry.Content!;
		stanza.Disabled.Should().BeFalse();
		stanza.EaiAppName.Should().Be("search");
		stanza.EaiUserName.Should().Be("nobody");
		stanza.GetValue("key1").Should().Be("v1");
		stanza.GetValue("key2").Should().Be("v=2");
		stanza.GetValue("count").Should().Be("3");
		stanza.GetValue("enabled").Should().Be("true");
		stanza.GetValue("list").Should().Be("""["a", "b"]""");
		stanza.GetValue("nothing").Should().BeNull();
		stanza.GetValue("missing").Should().BeNull();
		stanza.Values.Should().HaveCount(6).And.ContainKey("key1").And.NotContainKey("disabled");
	}

	[Fact]
	public async Task ListStanzasAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.Configs.ListStanzasAsync("nope", null, Calls.Token), HttpStatusCode.NotFound);
}
