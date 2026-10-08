using Splunk.Api.Models;
using Splunk.Api.Models.Configuration;

namespace Splunk.Api.IntegrationTest.Configuration;

[Collection(SplunkTestGroup.Name)]
public class ConfigurationIntegrationTests(SplunkFixture fixture)
{
	/// <summary>
	/// One fixed, test-only file: the REST API cannot delete a .conf file, so every run reuses it and removes its own stanzas.
	/// </summary>
	private const string TestFile = SplunkFixture.Prefix + "conf";

	private static CancellationToken Token => TestContext.Current.CancellationToken;

	private SplunkClient App => fixture.Client.InNamespace("nobody", "search");

	[Fact]
	public async Task Configs_ListsAndGetsServerConfStanzas()
	{
		var list = await fixture.Client.Configs.ListStanzasAsync("server", new ListOptions { Count = 5 }, Token);
		var general = await fixture.Client.Configs.GetStanzaAsync("server", "general", Token);

		list.Entries.Should().NotBeEmpty();
		general.Entries.Should().ContainSingle().Which.Content!.GetValue("serverName").Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Properties_ReadServerConf()
	{
		var files = await fixture.Client.ConfigProperties.ListFilesAsync(Token);
		var stanzas = await fixture.Client.ConfigProperties.ListStanzasAsync("server", Token);
		var keys = await fixture.Client.ConfigProperties.GetStanzaAsync("server", "general", Token);
		var serverName = await fixture.Client.ConfigProperties.GetValueAsync("server", "general", "serverName", Token);

		files.Entries.Select(e => e.Name).Should().Contain("server");
		stanzas.Entries.Select(e => e.Name).Should().Contain("general");
		keys.Entries.Should().Contain(e => e.Name == "serverName" && e.Content == serverName);
		serverName.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Stanza_RoundTrip_ThroughConfigsAndProperties()
	{
		var stanza = SplunkFixture.UniqueName("stanza") + "/x";
		await App.ConfigProperties.CreateFileAsync(new PropertiesFileCreateRequest { FileName = TestFile }, Token);
		try
		{
			var created = await App.Configs.CreateStanzaAsync(
				TestFile,
				new ConfStanzaCreateRequest { Name = stanza, AdditionalParameters = { ["colour"] = "red", ["size"] = "a=b" } },
				Token);
			created.Entries.Should().ContainSingle().Which.Content!.GetValue("size").Should().Be("a=b");

			await App.Configs.UpdateStanzaAsync(TestFile, stanza, new Dictionary<string, string?> { ["colour"] = "blue" }, Token);
			(await App.ConfigProperties.GetValueAsync(TestFile, stanza, "colour", Token)).Should().Be("blue");

			await App.ConfigProperties.SetValueAsync(TestFile, stanza, "colour", new PropertyValueRequest { Value = "green" }, Token);
			var updated = await App.ConfigProperties.UpdateStanzaAsync(TestFile, stanza, new Dictionary<string, string?> { ["shape"] = "round" }, Token);
			updated.Messages.Should().ContainSingle().Which.Text.Should().Contain("Successfully modified");

			var read = await App.Configs.GetStanzaAsync(TestFile, stanza, Token);
			read.Entries.Should().ContainSingle().Which.Content!.Values.Should().Contain("colour", "green").And.Contain("shape", "round");

			await App.ConfigProperties.DeleteValueAsync(TestFile, stanza, "shape", Token);
			var keys = await App.ConfigProperties.GetStanzaAsync(TestFile, stanza, Token);
			keys.Entries.Select(e => e.Name).Should().NotContain("shape");
			(await App.ConfigProperties.ListStanzasAsync(TestFile, Token)).Entries.Select(e => e.Name).Should().Contain(stanza);
		}
		finally
		{
			await App.Configs.DeleteStanzaAsync(TestFile, stanza, CancellationToken.None);
		}

		var act = () => App.Configs.GetStanzaAsync(TestFile, stanza, Token);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Properties_CreateAndDeleteStanza()
	{
		var stanza = SplunkFixture.UniqueName("pstanza");
		await App.ConfigProperties.CreateFileAsync(new PropertiesFileCreateRequest { FileName = TestFile }, Token);
		await App.ConfigProperties.CreateStanzaAsync(TestFile, new PropertiesStanzaCreateRequest { Stanza = stanza }, Token);
		try
		{
			(await App.ConfigProperties.ListStanzasAsync(TestFile, Token)).Entries.Select(e => e.Name).Should().Contain(stanza);
		}
		finally
		{
			await App.ConfigProperties.DeleteStanzaAsync(TestFile, stanza, CancellationToken.None);
		}

		(await App.ConfigProperties.ListStanzasAsync(TestFile, Token)).Entries.Select(e => e.Name).Should().NotContain(stanza);
	}

	[Fact]
	public async Task DeleteValue_WithoutNamespace_IsRejected()
	{
		var act = () => fixture.Client.ConfigProperties.DeleteValueAsync(TestFile, "nothing", "nothing", Token);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().NotBeNullOrWhiteSpace();
	}
}
