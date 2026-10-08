using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class FieldAliasesTests
{
	private const string EntryName = "audittrailv2 : FIELDALIAS-cim";
	private const string EntryPath = "/services/data/props/fieldaliases/audittrailv2%20%3A%20FIELDALIAS-cim";

	// Captured from Splunk Enterprise 10.6.0.5 (GET data/props/fieldaliases), trimmed.
	private static readonly string AliasJson = KnowledgeTestKit.Feed("data/props/fieldaliases", EntryName, """
		{
			"alias.0.actor.name": "user",
			"alias.1.host": "dest",
			"alias.2.host": "dvc",
			"attribute": "FIELDALIAS-cim",
			"eai:acl": null,
			"overwrite": false,
			"stanza": "audittrailv2",
			"type": "FIELDALIAS",
			"value": "\"actor.name\" ASNEW user host ASNEW dest host ASNEW dvc"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToFieldaliases()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldAliases.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/props/fieldaliases");

	[Fact]
	public async Task CreateAsync_PostsEachAliasAsAnAliasField()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldAliases.CreateAsync(
				new FieldAliasCreateRequest { Name = "cim", Stanza = "audittrailv2", Overwrite = false, Aliases = new Dictionary<string, string> { ["src"] = "source_ip", ["actor.name"] = "user" } },
				TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/data/props/fieldaliases", body: "name=cim&stanza=audittrailv2&overwrite=false&alias.src=source_ip&alias.actor.name=user");

	[Fact]
	public async Task GetAsync_SendsGetToTheEntry()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldAliases.GetAsync(EntryName, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, EntryPath);

	[Fact]
	public async Task UpdateAsync_PostsTheAliases()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldAliases.UpdateAsync(
				EntryName,
				new FieldAliasUpdateRequest { Overwrite = true, Aliases = new Dictionary<string, string> { ["dst"] = "dest_ip" } },
				TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, EntryPath, body: "overwrite=true&alias.dst=dest_ip");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldAliases.DeleteAsync(EntryName, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, EntryPath);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField_AndCollectsTheAliases()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.FieldAliases.GetAsync(EntryName, TestContext.Current.CancellationToken), AliasJson);

		entry.ShouldBeTheCapturedEntry(EntryName);
		var alias = entry.Content!;
		alias.Attribute.Should().Be("FIELDALIAS-cim");
		alias.Stanza.Should().Be("audittrailv2");
		alias.Type.Should().Be("FIELDALIAS");
		alias.Value.Should().StartWith("\"actor.name\" ASNEW user");
		alias.Overwrite.Should().BeFalse();
		alias.Aliases.Should().Equal(
			new KeyValuePair<string, string>("actor.name", "user"),
			new KeyValuePair<string, string>("host", "dest"),
			new KeyValuePair<string, string>("host", "dvc"));
		alias.AdditionalProperties.Should().ContainKey("alias.1.host");
	}

	[Fact]
	public void Requests_ReadTheirAliasesBackFromTheAdditionalParameters()
	{
		var create = new FieldAliasCreateRequest { Name = "n", Stanza = "s", Aliases = new Dictionary<string, string> { ["a"] = "b" } };
		var update = new FieldAliasUpdateRequest { Aliases = new Dictionary<string, string> { ["c"] = "d" } };

		create.Aliases.Should().Equal(new Dictionary<string, string> { ["a"] = "b" });
		update.Aliases.Should().Equal(new Dictionary<string, string> { ["c"] = "d" });
		update.AdditionalParameters.Should().Equal(new Dictionary<string, string?> { ["alias.c"] = "d" });
	}

	[Fact]
	public void Requests_KeepAdditionalParametersSetFirst_AndReadNullAsEmpty()
	{
		var request = new FieldAliasCreateRequest
		{
			Name = "n",
			Stanza = "s",
			AdditionalParameters = new Dictionary<string, string?> { ["alias.x"] = null, ["other"] = "1" },
			Aliases = new Dictionary<string, string> { ["y"] = "z" }
		};

		request.Aliases.Should().Equal(new Dictionary<string, string> { ["x"] = "", ["y"] = "z" });
	}

	[Fact]
	public void Requests_RejectNullAliases()
	{
		var act = () => new FieldAliasUpdateRequest { Aliases = null! };

		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.FieldAliases.GetAsync("missing", TestContext.Current.CancellationToken));
}
