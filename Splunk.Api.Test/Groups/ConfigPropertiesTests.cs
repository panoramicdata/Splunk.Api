using Splunk.Api.Models.Configuration;
using Splunk.Api.Test.Support;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ConfigPropertiesTests
{
	// Captured from Splunk 10.6.0.5: GET properties/{file}/{stanza} returns one entry per key, content = the value.
	private const string StanzaJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/servicesNS/nobody/search/properties/my_app/stanza%2Fone",
			"updated": "2026-10-08T13:54:50+00:00",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [
				{ "name": "key1", "id": "https://splunk.test:8089/servicesNS/nobody/search/properties/my_app/stanza%2Fone/key1", "updated": "1970-01-01T00:00:00+00:00", "links": { "alternate": "/servicesNS/nobody/search/properties/my_app/stanza%2Fone/key1" }, "content": "changed" },
				{ "name": "key2", "id": "https://splunk.test:8089/servicesNS/nobody/search/properties/my_app/stanza%2Fone/key2", "updated": "1970-01-01T00:00:00+00:00", "links": { "alternate": "/servicesNS/nobody/search/properties/my_app/stanza%2Fone/key2" }, "content": "v=2" }
			]
		}
		""";

	// GET properties and GET properties/{file}: entries with a name and no content.
	private const string FilesJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/properties",
			"updated": "2026-10-08T13:50:06+00:00",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [
				{ "name": "alert_actions", "id": "https://splunk.test:8089/services/properties/alert_actions", "updated": "1970-01-01T00:00:00+00:00", "links": { "alternate": "/services/properties/alert_actions" } },
				{ "name": "server", "id": "https://splunk.test:8089/services/properties/server", "updated": "1970-01-01T00:00:00+00:00", "links": { "alternate": "/services/properties/server" } }
			]
		}
		""";

	private const string ModifiedJson = """{"messages":[{"type":"INFO","text":"Successfully modified 2 key(s)"}]}""";

	[Fact]
	public async Task ListFilesAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ConfigProperties.ListFilesAsync(Calls.Token), HttpMethod.Get, "/services/properties", Calls.JsonQuery, null);

	[Fact]
	public async Task CreateFileAsync_PostsConf()
		=> await Calls.AssertAsync(
			c => c.ConfigProperties.CreateFileAsync(new PropertiesFileCreateRequest { FileName = "my_app" }, Calls.Token),
			HttpMethod.Post, "/services/properties", Calls.JsonQuery, "__conf=my_app");

	[Fact]
	public async Task ListStanzasAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ConfigProperties.ListStanzasAsync("server", Calls.Token), HttpMethod.Get, "/services/properties/server", Calls.JsonQuery, null);

	[Fact]
	public async Task CreateStanzaAsync_PostsStanza()
		=> await Calls.AssertAsync(
			c => c.ConfigProperties.CreateStanzaAsync("my_app", new PropertiesStanzaCreateRequest { Stanza = "second" }, Calls.Token),
			HttpMethod.Post, "/services/properties/my_app", Calls.JsonQuery, "__stanza=second");

	[Fact]
	public async Task GetStanzaAsync_SendsGet()
		=> await Calls.AssertAsync(
			c => c.ConfigProperties.GetStanzaAsync("my_app", "stanza/one", Calls.Token),
			HttpMethod.Get, "/services/properties/my_app/stanza%2Fone", Calls.JsonQuery, null);

	[Fact]
	public async Task UpdateStanzaAsync_PostsTheKeys()
		=> await Calls.AssertAsync(
			c => c.ConfigProperties.UpdateStanzaAsync("my_app", "general", new Dictionary<string, string?> { ["key3"] = "three", ["key4"] = "four" }, Calls.Token),
			HttpMethod.Post, "/services/properties/my_app/general", Calls.JsonQuery, "key3=three&key4=four");

	[Fact]
	public async Task DeleteStanzaAsync_SendsDeleteLocalOnly()
		=> await Calls.AssertAsync(
			c => c.ConfigProperties.DeleteStanzaAsync("my_app", "general", Calls.Token),
			HttpMethod.Delete, "/services/properties/my_app/general", "?local_only=true&output_mode=json", null);

	[Fact]
	public async Task GetValueAsync_SendsGet()
		=> await Calls.AssertAsync(
			c => c.ConfigProperties.GetValueAsync("server", "general", "serverName", Calls.Token),
			HttpMethod.Get, "/services/properties/server/general/serverName", Calls.JsonQuery, null);

	[Fact]
	public async Task SetValueAsync_PostsValue()
		=> await Calls.AssertAsync(
			c => c.ConfigProperties.SetValueAsync("my_app", "general", "key1", new PropertyValueRequest { Value = "a b&c" }, Calls.Token),
			HttpMethod.Post, "/services/properties/my_app/general/key1", Calls.JsonQuery, "value=a+b%26c");

	[Fact]
	public async Task DeleteValueAsync_SendsDeleteLocalOnly()
		=> await Calls.AssertAsync(
			c => c.ConfigProperties.DeleteValueAsync("my_app", "general", "key1", Calls.Token),
			HttpMethod.Delete, "/services/properties/my_app/general/key1", "?local_only=true&output_mode=json", null);

	[Fact]
	public async Task GetValueAsync_ReturnsThePlainTextBody()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.OK, "65cd44b6d7f9", r => r.Content.Headers.ContentType = new("text/plain"));
		using var client = TestClient.Create(stub);

		var value = await client.ConfigProperties.GetValueAsync("server", "general", "serverName", Calls.Token);

		value.Should().Be("65cd44b6d7f9");
	}

	[Fact]
	public async Task GetStanzaAsync_MapsOneEntryPerKey()
	{
		var feed = await Calls.MapAsync(c => c.ConfigProperties.GetStanzaAsync("my_app", "stanza/one", Calls.Token), StanzaJson);

		feed.Entries.Select(e => (e.Name, e.Content)).Should().Equal(("key1", "changed"), ("key2", "v=2"));
	}

	[Fact]
	public async Task ListFilesAsync_MapsNamesWithoutContent()
	{
		var feed = await Calls.MapAsync(c => c.ConfigProperties.ListFilesAsync(Calls.Token), FilesJson);

		feed.Entries.Select(e => e.Name).Should().Equal("alert_actions", "server");
		feed.Entries[0].Content.Should().BeNull();
	}

	[Fact]
	public async Task UpdateStanzaAsync_MapsTheConfirmationMessage()
	{
		var feed = await Calls.MapAsync(
			c => c.ConfigProperties.UpdateStanzaAsync("my_app", "general", new Dictionary<string, string?> { ["k"] = "v" }, Calls.Token),
			ModifiedJson);

		feed.Entries.Should().BeEmpty();
		feed.Messages.Should().ContainSingle().Which.Text.Should().Be("Successfully modified 2 key(s)");
	}

	[Fact]
	public async Task DeleteValueAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.ConfigProperties.DeleteValueAsync("f", "s", "k", Calls.Token), HttpStatusCode.BadRequest);
}
