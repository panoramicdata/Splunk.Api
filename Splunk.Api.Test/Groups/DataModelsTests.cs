using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class DataModelsTests
{
	private const string Definition = """{"modelName":"web","objects":[]}""";
	private const string EncodedDefinition = "%7B%22modelName%22%3A%22web%22%2C%22objects%22%3A%5B%5D%7D";

	// Captured from Splunk Enterprise 10.6.0.5 (GET datamodel/model), trimmed.
	private static readonly string ModelJson = KnowledgeTestKit.Feed("datamodel/model", "ATv2_Data_Model", """
		{
			"acceleration": "{\"enabled\":false,\"earliest_time\":\"-1y\",\"cron_schedule\":\"3-58/5 * * * *\",\"max_time\":3600}",
			"acceleration.allowed": true,
			"acceleration.hunk.compression_codec": "",
			"dataset.type": "datamodel",
			"description": "{\"modelName\":\"ATv2_Data_Model\",\"displayName\":\"ATv2 Data Model\",\"description\":\"Audit Trail v2 Data Model\",\"objectSummary\":{\"Event-Based\":33,\"Transaction-Based\":0,\"Search-Based\":0}}",
			"disabled": false,
			"displayName": "ATv2 Data Model",
			"eai:acl": null,
			"eai:appName": "audit_trail",
			"eai:digest": "b8cc1d568a2e122a1ef172e0a23ed591",
			"eai:type": "datamodel",
			"eai:userName": "nobody",
			"strict_fields": "true",
			"tags_whitelist": "cleartext,cloud,default"
		}
		""");

	// Captured from Splunk Enterprise 10.6.0.5 (GET datamodel/pivot/{name}), trimmed.
	private static readonly string PivotJson = KnowledgeTestKit.Feed("datamodel/pivot", "web", """
		{
			"acceleration": false,
			"drilldown_search": "| search (index=_internal) | stats count AS c",
			"eai:acl": null,
			"open_in_search": "| search (index=_internal) | stats count AS c | fields c",
			"pivot_json": "{\"baseClass\":\"Internal\",\"rows\":[],\"columns\":[]}",
			"pivot_search": "| pivot web Internal count(Internal) AS c ROWSUMMARY 0 COLSUMMARY 0 SHOWOTHER 1",
			"search": "| search (index=_internal) | stats count AS c | fields c",
			"tstats_search": ""
		}
		""");

	[Fact]
	public async Task ListAsync_SendsConcise()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModels.ListAsync(new DataModelListOptions { Concise = true }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/datamodel/model", "?concise=true&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsNameDefinitionAndAcceleration()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModels.CreateAsync(
				new DataModelCreateRequest { Name = "web", Description = Definition, Acceleration = """{"enabled":false}""" },
				TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/datamodel/model", body: "name=web&description=" + EncodedDefinition + "&acceleration=%7B%22enabled%22%3Afalse%7D");

	[Fact]
	public async Task GetAsync_SendsGetWithConcise()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModels.GetAsync("web", new DataModelGetOptions { Concise = false }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/datamodel/model/web", "?concise=false&output_mode=json");

	[Fact]
	public async Task UpdateAsync_PostsProvisional()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModels.UpdateAsync("web", new DataModelUpdateRequest { Description = Definition, Provisional = true }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/datamodel/model/web", body: "description=" + EncodedDefinition + "&provisional=true");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModels.DeleteAsync("web", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/datamodel/model/web");

	[Fact]
	public async Task GetPivotAsync_SendsThePivotSearch()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModels.GetPivotAsync("web", new PivotOptions { PivotSearch = "| pivot web Internal count(Internal) AS c" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/datamodel/pivot/web", "?pivot_search=%7C%20pivot%20web%20Internal%20count%28Internal%29%20AS%20c&output_mode=json");

	[Fact]
	public async Task GetPivotAsync_SendsThePivotJson()
		=> (await KnowledgeTestKit.SendAsync(c => c.DataModels.GetPivotAsync("web", new PivotOptions { PivotJson = "{}" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/datamodel/pivot/web", "?pivot_json=%7B%7D&output_mode=json");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.DataModels.GetAsync("ATv2_Data_Model", null, TestContext.Current.CancellationToken), ModelJson);

		entry.ShouldBeTheCapturedEntry("ATv2_Data_Model");
		var model = entry.Content!;
		model.DisplayName.Should().Be("ATv2 Data Model");
		model.Description.Should().Contain("\"objectSummary\"");
		model.Acceleration.Should().StartWith("{\"enabled\":false");
		model.AccelerationAllowed.Should().BeTrue();
		model.Digest.Should().Be("b8cc1d568a2e122a1ef172e0a23ed591");
		model.Type.Should().Be("datamodel");
		model.DatasetType.Should().Be("datamodel");
		model.StrictFields.Should().BeTrue();
		model.TagsWhitelist.Should().Be("cleartext,cloud,default");
		model.EaiAppName.Should().Be("audit_trail");
	}

	[Fact]
	public async Task GetPivotAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.DataModels.GetPivotAsync("web", new PivotOptions { PivotJson = "{}" }, TestContext.Current.CancellationToken), PivotJson);

		var pivot = entry.Content!;
		pivot.PivotSearch.Should().StartWith("| pivot web Internal");
		pivot.PivotJson.Should().Be("{\"baseClass\":\"Internal\",\"rows\":[],\"columns\":[]}");
		pivot.Search.Should().Be("| search (index=_internal) | stats count AS c | fields c");
		pivot.OpenInSearch.Should().Be(pivot.Search);
		pivot.DrilldownSearch.Should().Be("| search (index=_internal) | stats count AS c");
		pivot.TstatsSearch.Should().BeEmpty();
		pivot.AdditionalProperties["acceleration"].GetBoolean().Should().BeFalse();
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.DataModels.GetAsync("missing", null, TestContext.Current.CancellationToken));
}
