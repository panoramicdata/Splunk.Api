using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class AutomaticLookupsTests
{
	private const string EntryName = "audittrail : LOOKUP-dmc_add_instance_info";
	private const string EntryPath = "/services/data/props/lookups/audittrail%20%3A%20LOOKUP-dmc_add_instance_info";

	// Captured from Splunk Enterprise 10.6.0.5 (GET data/props/lookups); "src.ip" added to show a dotted column.
	private static readonly string LookupJson = KnowledgeTestKit.Feed("data/props/lookups", EntryName, """
		{
			"attribute": "LOOKUP-dmc_add_instance_info",
			"eai:acl": null,
			"lookup.field.input.host": "",
			"lookup.field.input.src.ip": "clientip",
			"lookup.field.output.0.machine": "",
			"lookup.field.output.1.search_group": "group",
			"overwrite": false,
			"stanza": "audittrail",
			"transform": "dmc_assets",
			"type": "LOOKUP",
			"value": "dmc_assets host OUTPUTNEW machine search_group AS group"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToLookups()
		=> (await KnowledgeTestKit.SendAsync(c => c.AutomaticLookups.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/props/lookups");

	[Fact]
	public async Task CreateAsync_PostsTheSettingsAndFieldFamilies()
		=> (await KnowledgeTestKit.SendAsync(c => c.AutomaticLookups.CreateAsync(
				new AutomaticLookupCreateRequest
				{
					Name = "assets",
					Stanza = "access_combined",
					Transform = "asset_lookup",
					Overwrite = false,
					InputFields = new Dictionary<string, string> { ["host"] = "" },
					OutputFields = new Dictionary<string, string> { ["owner"] = "asset_owner" }
				},
				TestContext.Current.CancellationToken)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/data/props/lookups",
				body: "name=assets&stanza=access_combined&transform=asset_lookup&overwrite=false&lookup.field.input.host=&lookup.field.output.owner=asset_owner");

	[Fact]
	public async Task GetAsync_SendsGetToTheEntry()
		=> (await KnowledgeTestKit.SendAsync(c => c.AutomaticLookups.GetAsync(EntryName, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, EntryPath);

	[Fact]
	public async Task UpdateAsync_PostsTheSettings()
		=> (await KnowledgeTestKit.SendAsync(c => c.AutomaticLookups.UpdateAsync(
				EntryName,
				new AutomaticLookupUpdateRequest { Transform = "asset_lookup", Overwrite = true, InputFields = new Dictionary<string, string> { ["ip"] = "" } },
				TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, EntryPath, body: "transform=asset_lookup&overwrite=true&lookup.field.input.ip=");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.AutomaticLookups.DeleteAsync(EntryName, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, EntryPath);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField_AndCollectsTheFields()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.AutomaticLookups.GetAsync(EntryName, TestContext.Current.CancellationToken), LookupJson);

		entry.ShouldBeTheCapturedEntry(EntryName);
		var lookup = entry.Content!;
		lookup.Attribute.Should().Be("LOOKUP-dmc_add_instance_info");
		lookup.Stanza.Should().Be("audittrail");
		lookup.Type.Should().Be("LOOKUP");
		lookup.Value.Should().Be("dmc_assets host OUTPUTNEW machine search_group AS group");
		lookup.Transform.Should().Be("dmc_assets");
		lookup.Overwrite.Should().BeFalse();
		lookup.InputFields.Should().Equal(new KeyValuePair<string, string>("host", ""), new KeyValuePair<string, string>("src.ip", "clientip"));
		lookup.OutputFields.Should().Equal(new KeyValuePair<string, string>("machine", ""), new KeyValuePair<string, string>("search_group", "group"));
	}

	[Fact]
	public async Task FieldFamilies_ReadNonStringValuesAsTheirJson()
	{
		var json = KnowledgeTestKit.Feed("data/props/lookups", EntryName, """{ "lookup.field.output.0.count": 5 }""");

		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.AutomaticLookups.GetAsync(EntryName, TestContext.Current.CancellationToken), json);

		entry.Content!.OutputFields.Should().ContainSingle().Which.Should().Be(new KeyValuePair<string, string>("count", "5"));
		entry.Content.InputFields.Should().BeEmpty();
	}

	[Fact]
	public void Requests_ReadTheirFieldsBackFromTheAdditionalParameters()
	{
		var request = new AutomaticLookupUpdateRequest
		{
			Transform = "t",
			Overwrite = true,
			InputFields = new Dictionary<string, string> { ["a"] = "" },
			OutputFields = new Dictionary<string, string> { ["b"] = "c" }
		};

		request.InputFields.Should().Equal(new Dictionary<string, string> { ["a"] = "" });
		request.OutputFields.Should().Equal(new Dictionary<string, string> { ["b"] = "c" });
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.AutomaticLookups.GetAsync("missing", TestContext.Current.CancellationToken));
}
