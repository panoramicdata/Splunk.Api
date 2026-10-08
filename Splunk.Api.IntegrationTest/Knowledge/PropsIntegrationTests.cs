using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>Round trips the <c>data/props/*</c> endpoints with objects on a test-only sourcetype stanza in the search app.</summary>
[Collection(SplunkTestGroup.Name)]
public sealed class PropsIntegrationTests(SplunkFixture fixture) : SearchAppIntegrationTests(fixture)
{
	private readonly string _stanza = SplunkFixture.UniqueName("st");

	[Fact]
	public async Task ListAsync_ReturnsEachFamily()
	{
		var paging = new ListOptions { Count = 1 };
		using var all = Fixture.Client.InNamespace(SplunkNamespace.All);

		(await all.CalculatedFields.ListAsync(paging, Token)).Entries.Should().ContainSingle().Which.Content!.Attribute.Should().StartWith("EVAL-");
		(await all.FieldExtractions.ListAsync(paging, Token)).Entries.Should().ContainSingle().Which.Content!.Stanza.Should().NotBeNullOrEmpty();
		(await all.FieldAliases.ListAsync(paging, Token)).Entries.Should().ContainSingle().Which.Content!.Aliases.Should().NotBeEmpty();
		(await all.AutomaticLookups.ListAsync(paging, Token)).Entries.Should().ContainSingle().Which.Content!.Transform.Should().NotBeNullOrEmpty();
		(await all.SourcetypeRenames.ListAsync(paging, Token)).Paging!.Offset.Should().Be(0);
	}

	[Fact]
	public async Task CalculatedField_CreateGetUpdateDelete()
	{
		var name = $"{_stanza} : EVAL-raw_length";
		try
		{
			var created = await App.CalculatedFields.CreateAsync(new CalculatedFieldCreateRequest { Name = "raw_length", Stanza = _stanza, Value = "len(_raw)" }, Token);
			created.Entries.Should().ContainSingle().Which.Name.Should().Be(name);

			await App.CalculatedFields.UpdateAsync(name, new CalculatedFieldUpdateRequest { Value = "len(_raw)*2" }, Token);

			var field = (await App.CalculatedFields.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			field.FieldName.Should().Be("raw_length");
			field.Type.Should().Be("EVAL");
			field.Value.Should().Be("len(_raw)*2");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.CalculatedFields.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.CalculatedFields.GetAsync(name, Token));
	}

	[Fact]
	public async Task FieldExtraction_CreateGetUpdateDelete()
	{
		var name = $"{_stanza} : EXTRACT-port";
		try
		{
			await App.FieldExtractions.CreateAsync(
				new FieldExtractionCreateRequest { Name = "port", Stanza = _stanza, Type = FieldExtractionType.Extract, Value = "port (?<port>[0-9]+)" },
				Token);

			await App.FieldExtractions.UpdateAsync(name, new FieldExtractionUpdateRequest { Value = "port=(?<port>[0-9]+)" }, Token);

			var extraction = (await App.FieldExtractions.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			extraction.Type.Should().Be("Inline");
			extraction.Value.Should().Be("port=(?<port>[0-9]+)");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.FieldExtractions.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.FieldExtractions.GetAsync(name, Token));
	}

	[Fact]
	public async Task FieldAlias_CreateGetUpdateDelete_ReplacesTheAliases()
	{
		var name = $"{_stanza} : FIELDALIAS-addresses";
		try
		{
			await App.FieldAliases.CreateAsync(
				new FieldAliasCreateRequest { Name = "addresses", Stanza = _stanza, Aliases = new Dictionary<string, string> { ["src"] = "source_ip" } },
				Token);

			var updated = await App.FieldAliases.UpdateAsync(
				name,
				new FieldAliasUpdateRequest { Overwrite = false, Aliases = new Dictionary<string, string> { ["dst"] = "dest_ip" } },
				Token);
			updated.Entries.Should().ContainSingle().Which.Content!.Aliases.Should().ContainSingle().Which.Should().Be(new KeyValuePair<string, string>("dst", "dest_ip"));

			var alias = (await App.FieldAliases.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			alias.Overwrite.Should().BeFalse();
			alias.Value.Should().Be("dst ASNEW dest_ip");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.FieldAliases.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.FieldAliases.GetAsync(name, Token));
	}

	[Fact]
	public async Task AutomaticLookup_CreateGetUpdateDelete()
	{
		var definition = SplunkFixture.UniqueName("lk");
		var name = $"{_stanza} : LOOKUP-dns";
		try
		{
			await App.LookupDefinitions.CreateAsync(
				new LookupDefinitionCreateRequest { Name = definition, ExternalCommand = "external_lookup.py clienthost clientip", FieldsList = "clienthost,clientip" },
				Token);
			await App.AutomaticLookups.CreateAsync(
				new AutomaticLookupCreateRequest
				{
					Name = "dns",
					Stanza = _stanza,
					Transform = definition,
					Overwrite = false,
					InputFields = new Dictionary<string, string> { ["clientip"] = "" },
					OutputFields = new Dictionary<string, string> { ["clienthost"] = "client_name" }
				},
				Token);

			await App.AutomaticLookups.UpdateAsync(
				name,
				new AutomaticLookupUpdateRequest { Transform = definition, Overwrite = true, InputFields = new Dictionary<string, string> { ["clienthost"] = "" } },
				Token);

			var lookup = (await App.AutomaticLookups.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			lookup.Transform.Should().Be(definition);
			lookup.Overwrite.Should().BeTrue();
			lookup.InputFields.Should().ContainSingle().Which.Should().Be(new KeyValuePair<string, string>("clienthost", ""));
			lookup.OutputFields.Should().BeEmpty("an update replaces the lookup's fields");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.AutomaticLookups.DeleteAsync(name, CancellationToken.None));
			await Cleanup.IgnoreMissingAsync(() => App.LookupDefinitions.DeleteAsync(definition, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.AutomaticLookups.GetAsync(name, Token));
	}

	[Fact]
	public async Task SourcetypeRename_CreateGetUpdateDelete()
	{
		try
		{
			await App.SourcetypeRenames.CreateAsync(new SourcetypeRenameCreateRequest { Name = _stanza, Value = "splunk_api_it_renamed" }, Token);

			await App.SourcetypeRenames.UpdateAsync(_stanza, new SourcetypeRenameUpdateRequest { Value = "splunk_api_it_renamed_again" }, Token);

			var rename = (await App.SourcetypeRenames.GetAsync(_stanza, Token)).Entries.Should().ContainSingle().Subject.Content!;
			rename.Attribute.Should().Be("rename");
			rename.Value.Should().Be("splunk_api_it_renamed_again");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.SourcetypeRenames.DeleteAsync(_stanza, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.SourcetypeRenames.GetAsync(_stanza, Token));
	}
}
