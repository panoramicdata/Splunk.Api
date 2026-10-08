using Splunk.Api.Models.Spl2;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class Spl2DatasetsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (orchestrator/v1/datasets?pageSize=1), pagination token shortened.
	private const string DatasetJson = """
		{"owner":"nobody","modified":"1970-01-01 00:00:00","updatedAt":"1970-01-01T00:00:00.000Z","id":"indexes.main","namespace":"~indexes","module":"","name":"main","kind":"index","createdby":"nobody","createdBy":"nobody","modifiedby":"nobody","updatedBy":"nobody","resourcename":"~indexes.main","resourceName":"~indexes.main","internalname":"indexes_____main","internalName":"indexes_____main","resourceGroup":"tenant","datatype":"event","title":"Main","summary":"s","description":"d","createdAt":"2026-01-14 09:23:11Z"}
		""";

	private static readonly string ListJson = $$"""{"results":[{{DatasetJson}}],"nextLink":"/services/orchestrator/v1/datasets?pageSize=1&paginationToken=f30f%2Bea18","totalCount":254}""";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetWithEveryOption()
	{
		var stub = TestClient.Stub(ListJson);
		using var client = TestClient.Create(stub);

		await client.Spl2Datasets.ListAsync(
			new Spl2DatasetListOptions
			{
				Kind = "lookup",
				Filter = "name=x",
				OrderBy = "name desc",
				Offset = 2,
				PageSize = 10,
				PaginationToken = "tok",
				ConnectionId = "c1",
				DataSource = "ds",
				SupportedCapabilities = "search",
				Visibility = "public"
			},
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Get,
			"/services/orchestrator/v1/datasets",
			"?kind=lookup&filter=name%3Dx&orderBy=name%20desc&offset=2&pageSize=10&paginationToken=tok&connection_id=c1&dataSource=ds"
				+ "&supported_capabilities=search&visibility=public&output_mode=json");
	}

	[Fact]
	public async Task ListAsync_MapsThePage()
	{
		using var client = TestClient.Create(TestClient.Stub(ListJson));

		var page = await client.Spl2Datasets.ListAsync(null, Ct);

		page.TotalCount.Should().Be(254);
		page.NextLink.Should().StartWith("/services/orchestrator/v1/datasets?");
		page.NextPaginationToken.Should().Be("f30f+ea18");
		var dataset = page.Results.Should().ContainSingle().Subject;
		dataset.Id.Should().Be("indexes.main");
		dataset.Name.Should().Be("main");
		dataset.Namespace.Should().Be("~indexes");
		dataset.ResourceName.Should().Be("~indexes.main");
		dataset.Kind.Should().Be("index");
		dataset.Module.Should().BeEmpty();
		dataset.Owner.Should().Be("nobody");
		dataset.Title.Should().Be("Main");
		dataset.Summary.Should().Be("s");
		dataset.Description.Should().Be("d");
		dataset.InternalName.Should().Be("indexes_____main");
		dataset.DataType.Should().Be("event");
		dataset.CreatedBy.Should().Be("nobody");
		dataset.CreatedAt.Should().Be("2026-01-14 09:23:11Z");
		dataset.UpdatedBy.Should().Be("nobody");
		dataset.UpdatedAt.Should().Be("1970-01-01T00:00:00.000Z");
		dataset.AdditionalProperties.Should().ContainKeys("resourceGroup", "modified");
	}

	[Theory]
	[InlineData(null, null)]
	[InlineData("/services/orchestrator/v1/datasets", null)]
	[InlineData("/services/orchestrator/v1/datasets?pageSize=1", null)]
	[InlineData("/x?paginationToken=abc&pageSize=1", "abc")]
	public void NextPaginationToken_ReadsTheTokenFromTheLink(string? nextLink, string? expected)
		=> new Spl2DatasetList { NextLink = nextLink }.NextPaginationToken.Should().Be(expected);

	[Fact]
	public async Task GetAsync_SendsGetWithConnection()
	{
		var stub = TestClient.Stub(DatasetJson);
		using var client = TestClient.Create(stub);

		var dataset = await client.Spl2Datasets.GetAsync("~indexes.main", false, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/orchestrator/v1/datasets/~indexes.main", "?with_connection=false&output_mode=json");
		dataset.Id.Should().Be("indexes.main");
	}

	[Fact]
	public async Task GetAsync_Error_RaisesTheOrchestratorMessage()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"code":"bad_request","message":"The with_connection parameter is supported only for unified datasets."}""", HttpStatusCode.BadRequest));

		await SearchRequestAssert.FailsWith(
			() => client.Spl2Datasets.GetAsync("indexes.main", true, Ct),
			HttpStatusCode.BadRequest,
			"The with_connection parameter is supported only for unified datasets.");
	}
}
