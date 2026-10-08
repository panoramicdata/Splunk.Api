using Splunk.Api.Models;
using Splunk.Api.Models.Spl2;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public partial class Spl2ModulesTests
{
	private const string ModulesPath = "/services/orchestrator/v1/spl2/modules";
	private const string ItemPath = ModulesPath + "/apps.search.my_module";

	// From the Splunk 10.6 OpenAPI specification's example: the test instance cannot create modules.
	private const string ModuleJson = """
		{"namespace":"apps.search","name":"my_module","definition":"$s = from main | head 10;\nexport {$s}\n","description":"d","createdAt":"2026-01-14 09:23:11.104000Z","createdBy":"admin","updatedAt":"2026-03-18 16:47:52.331000Z","updatedBy":"analyst","sourcePath":"apps/search/my_module","version":7,"canWrite":true,"@template":{"name":"t","runtime":["splunkd"]},"annotations":[]}
		""";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetWithEveryOption()
	{
		var stub = TestClient.Stub($$"""{"results":[{{ModuleJson}}],"totalCount":1}""");
		using var client = TestClient.Create(stub);

		var list = await client.Spl2Modules.ListAsync(
			new Spl2ModuleListOptions
			{
				Namespace = "apps.search",
				Search = ["name=my*", "createdBy=admin"],
				Filter = "my",
				Count = 5,
				Offset = 1,
				OrderBy = "name",
				IncludeDefinitions = true,
				IncludeAnnotations = false,
				ExcludeReservedModules = true,
				Templates = Spl2TemplateFilter.Include
			},
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Get,
			ModulesPath,
			"?namespace=apps.search&search=name%3Dmy%2A&search=createdBy%3Dadmin&filter=my&count=5&offset=1&orderBy=name&includeDefinitions=true"
				+ "&include_annotations=false&excludeReservedModules=true&templates=include&output_mode=json");
		list.TotalCount.Should().Be(1);
		list.Results.Should().ContainSingle().Which.Name.Should().Be("my_module");
	}

	[Theory]
	[InlineData(Spl2TemplateFilter.Exclude, "exclude")]
	[InlineData(Spl2TemplateFilter.Only, "only")]
	public async Task ListAsync_SendsTheTemplateFilter(Spl2TemplateFilter filter, string wire)
	{
		var stub = TestClient.Stub("""{"results":[]}""");
		using var client = TestClient.Create(stub);

		var list = await client.Spl2Modules.ListAsync(new Spl2ModuleListOptions { Templates = filter }, Ct);

		stub.Calls[0].Uri.Query.Should().Be($"?templates={wire}&output_mode=json");
		list.Results.Should().BeEmpty();
		list.TotalCount.Should().BeNull();
	}

	[Fact]
	public async Task GetAsync_SendsGetAndMapsTheModule()
	{
		var stub = TestClient.Stub(ModuleJson);
		using var client = TestClient.Create(stub);

		var module = await client.Spl2Modules.GetAsync("apps.search.my_module", true, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ItemPath, "?include_annotations=true&output_mode=json");
		module.Name.Should().Be("my_module");
		module.Namespace.Should().Be("apps.search");
		module.Definition.Should().StartWith("$s = from main");
		module.Description.Should().Be("d");
		module.SourcePath.Should().Be("apps/search/my_module");
		module.Version.Should().Be(7);
		module.CanWrite.Should().BeTrue();
		module.CreatedBy.Should().Be("admin");
		module.CreatedAt.Should().Be("2026-01-14 09:23:11.104000Z");
		module.UpdatedBy.Should().Be("analyst");
		module.UpdatedAt.Should().Be("2026-03-18 16:47:52.331000Z");
		module.Template!.Value.GetProperty("name").GetString().Should().Be("t");
		module.AdditionalProperties.Should().ContainKey("annotations");
	}

	[Fact]
	public async Task PutAsync_SendsTheModuleAsJson()
	{
		var stub = TestClient.Stub(ModuleJson);
		using var client = TestClient.Create(stub);

		await client.Spl2Modules.PutAsync(
			"apps.search.my_module",
			true,
			null,
			new JsonBody<Spl2ModuleRequest>(new Spl2ModuleRequest
			{
				Name = "my_module",
				Namespace = "apps.search",
				Definition = "$s = from main | head 10; export {$s}",
				Description = "d",
				DisplayName = "My module",
				EventSampling = "none",
				EarliestTime = "-1h",
				LatestTime = "now"
			}),
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Put,
			ItemPath,
			"?isUpdate=true&output_mode=json",
			"""{"name":"my_module","namespace":"apps.search","definition":"$s = from main | head 10; export {$s}","description":"d","displayName":"My module","eventSampling":"none","earliestTime":"-1h","latestTime":"now"}""");
	}

	[Fact]
	public async Task DeleteAsync_SendsDelete()
	{
		var stub = SearchRequestAssert.NoContentStub();
		using var client = TestClient.Create(stub);

		await client.Spl2Modules.DeleteAsync("apps.search.my_module", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Delete, ItemPath, "?output_mode=json");
	}

	[Fact]
	public async Task GetAsync_Unknown_RaisesNotFound()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"code":"not_found","message":"Module not found.","details":[{"name":"apps.search.nosuch"}]}""", HttpStatusCode.NotFound));

		await SearchRequestAssert.FailsWith(() => client.Spl2Modules.GetAsync("apps.search.nosuch", null, Ct), HttpStatusCode.NotFound, "Module not found.");
	}
}
