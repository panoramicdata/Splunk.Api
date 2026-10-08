using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>Round trips a test data model in the search app, with its pivot and acceleration summary.</summary>
[Collection(SplunkTestGroup.Name)]
public sealed class DataModelIntegrationTests(SplunkFixture fixture) : IDisposable
{
	private readonly SplunkClient _app = fixture.Client.InNamespace("nobody", "search");

	private static CancellationToken Token => TestContext.Current.CancellationToken;

	public void Dispose() => _app.Dispose();

	[Fact]
	public async Task DataModel_CreateGetUpdateValidateDelete()
	{
		var name = SplunkFixture.UniqueName("dm");
		try
		{
			var created = await _app.DataModels.CreateAsync(new DataModelCreateRequest { Name = name, Description = Definition(name) }, Token);
			created.Entries.Should().ContainSingle().Which.Content!.DisplayName.Should().Be("IT model");

			await _app.DataModels.UpdateAsync(name, new DataModelUpdateRequest { Acceleration = """{"enabled":false,"earliest_time":"-1d"}""" }, Token);

			var model = (await _app.DataModels.GetAsync(name, new DataModelGetOptions { Concise = true }, Token)).Entries.Should().ContainSingle().Subject.Content!;
			model.Acceleration.Should().Contain("\"earliest_time\":\"-1d\"");
			model.Type.Should().Be("datamodel");
			model.Description.Should().Contain("\"modelName\":\"" + name + "\"");

			var validated = await _app.DataModels.UpdateAsync(name, new DataModelUpdateRequest { Description = Definition(name), Provisional = true }, Token);
			validated.Entries.Should().ContainSingle().Which.Content!.Description.Should().NotBeNullOrEmpty();
			(await _app.DataModels.ListAsync(new DataModelListOptions { Search = name, Concise = true }, Token)).Entries.Should().ContainSingle();
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => _app.DataModels.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => _app.DataModels.GetAsync(name, null, Token));
	}

	[Fact]
	public async Task Pivot_TranslatesAPivotSearch()
	{
		var name = SplunkFixture.UniqueName("dm");
		try
		{
			await _app.DataModels.CreateAsync(new DataModelCreateRequest { Name = name, Description = Definition(name) }, Token);

			var pivot = (await _app.DataModels.GetPivotAsync(name, new PivotOptions { PivotSearch = $"| pivot {name} Internal count(Internal) AS c" }, Token))
				.Entries.Should().ContainSingle().Subject.Content!;

			pivot.PivotSearch.Should().StartWith($"| pivot {name} Internal count(Internal) AS c");
			pivot.PivotJson.Should().Contain("\"baseClass\":\"Internal\"");
			pivot.Search.Should().Contain("index=_internal");
			pivot.OpenInSearch.Should().NotBeNullOrEmpty();
			pivot.DrilldownSearch.Should().NotBeNullOrEmpty();

			var fromJson = await _app.DataModels.GetPivotAsync(name, new PivotOptions { PivotJson = pivot.PivotJson }, Token);
			fromJson.Entries.Should().ContainSingle().Which.Content!.Search.Should().Be(pivot.Search);
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => _app.DataModels.DeleteAsync(name, CancellationToken.None));
		}
	}

	[Fact]
	public async Task Summaries_ListAndGet_AnAcceleratedModel()
	{
		var name = SplunkFixture.UniqueName("dm");
		try
		{
			await _app.DataModels.CreateAsync(
				new DataModelCreateRequest { Name = name, Description = Definition(name), Acceleration = """{"enabled":true,"earliest_time":"-1h","cron_schedule":"*/30 * * * *"}""" },
				Token);

			var listed = await fixture.Client.DataModelSummaries.ListAsync(new DataModelSummaryListOptions { ByTstats = true, Search = name }, Token);
			listed.Entries.Should().ContainSingle().Which.Name.Should().Be($"tstats:DM_search_{name}");

			var summary = (await _app.DataModelSummaries.GetAsync("search", name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			summary.SummaryId.Should().Be($"DM_search_{name}");
			summary.Search.Should().Contain("summarize tstats=t");
			summary.TimeRangeSeconds.Should().Be(3600);
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => _app.DataModels.DeleteAsync(name, CancellationToken.None));
		}
	}

	[Fact]
	public async Task Summaries_GetANonAcceleratedModel_RaisesNotFound()
		=> await Cleanup.AssertGoneAsync(() => _app.DataModelSummaries.GetAsync("search", SplunkFixture.UniqueName("dm"), Token));

	[Fact]
	public async Task DataModels_ListEveryApp()
	{
		using var all = fixture.Client.InNamespace(SplunkNamespace.All);

		var feed = await all.DataModels.ListAsync(new DataModelListOptions { Count = 1 }, Token);

		feed.Entries.Should().ContainSingle().Which.Content!.AccelerationAllowed.Should().NotBeNull();
	}

	private static string Definition(string name)
		=> $$"""
			{"modelName":"{{name}}","displayName":"IT model","description":"Integration test","objects":[{"objectName":"Internal","displayName":"Internal","parentName":"BaseEvent","fields":[{"fieldName":"host","owner":"Internal","type":"string","fieldSearch":"","required":false,"multivalue":false,"hidden":false,"editable":true,"displayName":"host","comment":""}],"calculations":[],"constraints":[{"search":"index=_internal","owner":"Internal"}],"lineage":"Internal"}],"objectNameList":["Internal"]}
			""";
}
