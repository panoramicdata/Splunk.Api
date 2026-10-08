using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;
using System.Net;

namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>Round trips the <c>data/transforms/*</c> and <c>data/lookup-table-files</c> endpoints in the search app.</summary>
[Collection(SplunkTestGroup.Name)]
public sealed class TransformsIntegrationTests(SplunkFixture fixture) : SearchAppIntegrationTests(fixture)
{
	[Fact]
	public async Task FieldTransform_CreateGetUpdateDelete_UpdateResetsOmittedSettings()
	{
		var name = SplunkFixture.UniqueName("tx");
		try
		{
			var created = await App.FieldTransforms.CreateAsync(
				new FieldTransformCreateRequest { Name = name, Regex = "(?<k>[a-z]+)=(?<v>[a-z]+)", MultivalueAdd = true, CleanKeys = false },
				Token);
			created.Entries.Should().ContainSingle().Which.Content!.SourceKey.Should().Be("_raw", "Splunk defaults SOURCE_KEY although the reference marks it required");

			await App.FieldTransforms.UpdateAsync(name, new FieldTransformUpdateRequest { Regex = "(?<k>[a-z]+):(?<v>[a-z]+)", Format = "$1::$2" }, Token);

			var transform = (await App.FieldTransforms.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			transform.Regex.Should().Be("(?<k>[a-z]+):(?<v>[a-z]+)");
			transform.Format.Should().Be("$1::$2");
			transform.MultivalueAdd.Should().BeFalse("an update rewrites the stanza");
			transform.CleanKeys.Should().BeTrue();
			(await App.FieldTransforms.ListAsync(new ListOptions { Search = name }, Token)).Entries.Should().ContainSingle();
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.FieldTransforms.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.FieldTransforms.GetAsync(name, Token));
	}

	[Fact]
	public async Task LookupDefinition_CreateGetUpdateDelete()
	{
		var name = SplunkFixture.UniqueName("lk");
		const string Command = "external_lookup.py clienthost clientip";
		try
		{
			var created = await App.LookupDefinitions.CreateAsync(
				new LookupDefinitionCreateRequest { Name = name, ExternalCommand = Command, FieldsList = "clienthost,clientip", MaxMatches = 5 },
				Token);
			created.Entries.Should().ContainSingle().Which.Content!.Type.Should().Be("external");

			await App.LookupDefinitions.UpdateAsync(
				name,
				new LookupDefinitionUpdateRequest { ExternalCommand = Command, FieldsList = "clienthost,clientip", DefaultMatch = "none", MinMatches = 1 },
				Token);

			var lookup = (await App.LookupDefinitions.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			lookup.ExternalCommand.Should().Be(Command);
			lookup.Fields.Should().Equal("clienthost", "clientip");
			lookup.DefaultMatch.Should().Be("none");
			lookup.MinMatches.Should().Be(1);
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.LookupDefinitions.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.LookupDefinitions.GetAsync(name, Token));
	}

	[Fact]
	public async Task LookupDefinitions_ListWithGetSize_ReportsFileSizes()
	{
		using var all = Fixture.Client.InNamespace(SplunkNamespace.All);

		var feed = await all.LookupDefinitions.ListAsync(new LookupDefinitionListOptions { GetSize = true, Search = "type=file" }, Token);

		feed.Entries.Should().NotBeEmpty().And.OnlyContain(e => e.Content!.Type == "file" && e.Content.FileName != null);
		feed.Entries.Should().Contain(e => e.Content!.Size > 0, "getsize reports the size of each file that exists");
		feed.Entries.Should().OnlyContain(e => e.Content!.Fields != null, "a missing file's null fields_array reads as empty");
	}

	[Fact]
	public async Task MetricSchema_CreateListDelete()
	{
		var name = SplunkFixture.UniqueName("ms");
		try
		{
			var created = await App.MetricSchemas.CreateAsync(
				new MetricSchemaCreateRequest { Name = name, FieldNames = "size,count", BlacklistDimensions = "location" },
				Token);
			created.Entries.Should().ContainSingle().Which.Name.Should().Be($"metric-schema:{name}");

			var schema = (await App.MetricSchemas.ListAsync(new ListOptions { Search = name }, Token)).Entries.Should().ContainSingle().Subject.Content!;
			schema.Measures.Should().Be("size,count");
			schema.BlacklistDimensions.Should().Be("location");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.MetricSchemas.DeleteAsync(name, CancellationToken.None));
		}

		(await App.MetricSchemas.ListAsync(new ListOptions { Search = name }, Token)).Entries.Should().BeEmpty();
	}

	[Fact]
	public async Task StatsdExtraction_Create()
	{
		var name = SplunkFixture.UniqueName("sd");
		try
		{
			var created = await App.StatsdExtractions.CreateAsync(
				new StatsdExtractionCreateRequest { Name = name, Regex = "[.](?<hostname>[^.]+)[.]", RemoveDimensionsFromMetricName = true },
				Token);

			var entry = created.Entries.Should().ContainSingle().Subject;
			entry.Name.Should().Be($"statsd-dims:{name}");
			entry.Content!.Regex.Should().Be("[.](?<hostname>[^.]+)[.]");
			entry.Content.RemoveDimensionsFromMetricName.Should().BeTrue();
		}
		finally
		{
			await Cleanup.DeleteUndocumentedAsync(Fixture, "data/transforms/statsdextractions", name);
		}
	}

	[Fact]
	public async Task LookupTableFiles_ListAndGet()
	{
		using var all = Fixture.Client.InNamespace(SplunkNamespace.All);
		var listed = (await all.LookupTableFiles.ListAsync(new ListOptions { Count = 1 }, Token)).Entries.Should().ContainSingle().Subject;
		using var owner = Fixture.Client.InNamespace("nobody", listed.Acl!.App!);

		var file = (await owner.LookupTableFiles.GetAsync(listed.Name, Token)).Entries.Should().ContainSingle().Subject.Content!;

		file.Path.Should().EndWith("/lookups/" + listed.Name);
		file.Fields.Should().NotBeEmpty();
		file.Size.Should().BePositive();
		file.LastModifiedTime.Should().NotBeNull();
	}

	[Fact]
	public async Task LookupTableFiles_CreateAndUpdate_RefuseAPathOutsideTheStagingArea()
	{
		var name = SplunkFixture.UniqueName("ltf") + ".csv";
		const string Outside = "/tmp/splunk_api_it.csv";

		var create = () => App.LookupTableFiles.CreateAsync(new LookupTableFileCreateRequest { Name = name, StagedPath = Outside }, Token);
		var update = () => App.LookupTableFiles.UpdateAsync(name, new LookupTableFileUpdateRequest { StagedPath = Outside }, Token);

		(await create.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Contain("outside of staging area");
		(await update.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task LookupTableFiles_DeleteMissing_RaisesNotFound()
		=> await Cleanup.AssertGoneAsync(() => App.LookupTableFiles.DeleteAsync(SplunkFixture.UniqueName("ltf") + ".csv", Token));
}
