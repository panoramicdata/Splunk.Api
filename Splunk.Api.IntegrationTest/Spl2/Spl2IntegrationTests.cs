using Splunk.Api.Models;
using Splunk.Api.Models.Spl2;
using System.Net;

namespace Splunk.Api.IntegrationTest.Spl2;

[Collection(SplunkTestGroup.Name)]
public class Spl2IntegrationTests(SplunkFixture fixture) : IAsyncLifetime
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	/// <summary>
	/// The orchestrator runs as a splunkd sidecar that the shared instance restarts now and then; while it is down its
	/// routes answer 404 <c>Not Found</c>. Wait (up to a minute) until it answers before each test.
	/// </summary>
	public async ValueTask InitializeAsync()
	{
		for (var attempt = 0; ; attempt++)
		{
			try
			{
				await Client.Spl2Modules.ListAsync(new Spl2ModuleListOptions { Count = 1 }, Ct);
				return;
			}
			catch (SplunkApiException exception) when (exception.StatusCode == HttpStatusCode.NotFound && attempt < 30)
			{
				await Task.Delay(TimeSpan.FromSeconds(2), Ct);
			}
		}
	}

	public ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);
		return ValueTask.CompletedTask;
	}

	[Fact]
	public async Task Datasets_PageThroughAndGetOne()
	{
		var first = await Client.Spl2Datasets.ListAsync(new Spl2DatasetListOptions { Kind = "index", PageSize = 2 }, Ct);
		first.Results.Should().HaveCount(2).And.OnlyContain(d => d.Kind == "index");
		first.TotalCount.Should().BeGreaterThan(2);

		var second = await Client.Spl2Datasets.ListAsync(new Spl2DatasetListOptions { Kind = "index", PageSize = 2, PaginationToken = first.NextPaginationToken }, Ct);
		second.Results.Select(d => d.Id).Should().NotIntersectWith(first.Results.Select(d => d.Id));

		var main = await Client.Spl2Datasets.GetAsync("~indexes.main", null, Ct);
		main.Id.Should().Be("indexes.main");
		main.DataType.Should().Be("event");
	}

	[Fact]
	public async Task Datasets_RejectedArguments_RaiseBadRequest()
	{
		var withConnection = () => Client.Spl2Datasets.GetAsync("indexes.main", true, Ct);
		var thrown = await withConnection.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		thrown.Which.Message.Should().Be("The with_connection parameter is supported only for unified datasets.");

		var metricKind = () => Client.Spl2Datasets.ListAsync(new Spl2DatasetListOptions { Kind = "metric" }, Ct);
		(await metricKind.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Be("Validation Failed: kind=metric is not allowed.");
	}

	[Fact]
	public async Task Modules_ListAndUnknownModuleErrors()
	{
		var list = await Client.Spl2Modules.ListAsync(new Spl2ModuleListOptions { Templates = Spl2TemplateFilter.Include, Count = 10 }, Ct);
		list.Results.Should().OnlyContain(m => m.Name != null);

		var unknown = "apps.search." + SplunkFixture.UniqueName("none");
		await ShouldFailAsync(() => Client.Spl2Modules.GetAsync(unknown, null, Ct), HttpStatusCode.NotFound, "Module not found.");
		await ShouldFailAsync(() => Client.Spl2Modules.DeleteAsync(unknown, Ct), HttpStatusCode.NotFound, "Module not found.");
		await ShouldFailAsync(() => Client.Spl2Modules.GetPermissionsAsync(unknown, Ct), HttpStatusCode.NotFound, "Module not found.");
		await ShouldFailAsync(
			() => Client.Spl2Modules.UpdatePermissionsAsync(unknown, Permissions(unknown), Ct),
			HttpStatusCode.BadRequest,
			"failed to find module " + unknown);
	}

	[Fact]
	public async Task Modules_UnderServicesNS_AreNotRouted()
	{
		using var namespaced = Client.InNamespace("admin", "search");

		await ShouldFailAsync(() => namespaced.Spl2Modules.ListAsync(null, Ct), HttpStatusCode.NotFound, "route not found.");
	}

	[Fact]
	public async Task Conversion_ConvertsOrReportsTheLanguageServerDown()
	{
		var request = new JsonBody<Spl2ConversionRequest>(new Spl2ConversionRequest { Spl = "search index=_internal | stats count by host" });

		await LanguageServerAsync(async () => (await Client.Spl2Conversion.ConvertAsync(request, Ct)).Spl2.Should().Contain("_internal"));
		await LanguageServerAsync(async () => (await Client.Spl2Conversion.ConvertV2Async(request, Ct)).Spl2.Should().Contain("_internal"));
	}

	[Fact]
	public async Task ModuleLifecycle_RunsOrReportsTheLanguageServerDown()
	{
		var name = SplunkFixture.UniqueName("mod");
		var resource = "apps.search." + name;
		const string Definition = "$s1 = from _internal | head 3;\nexport {$s1}\n";
		await LanguageServerAsync(async () =>
		{
			var module = await Client.Spl2Modules.PutAsync(resource, null, null, new JsonBody<Spl2ModuleRequest>(new Spl2ModuleRequest { Name = name, Namespace = "apps.search", Definition = Definition }), Ct);
			try
			{
				module.Name.Should().Be(name);
				(await Client.Spl2Modules.GetAsync(resource, null, Ct)).Definition.Should().Be(Definition);
				(await Client.Spl2Modules.UpdatePermissionsAsync(resource, Permissions(resource), Ct)).Code.Should().Be(201);
				(await Client.Spl2Modules.GetPermissionsAsync(resource, Ct)).Should().Contain(p => p.Role == "admin");
			}
			finally
			{
				await Client.Spl2Modules.DeleteAsync(resource, CancellationToken.None);
			}
		});
		await LanguageServerAsync(async () =>
		{
			var dispatched = await Client.Spl2Modules.DispatchAsync(
				new JsonBody<Spl2DispatchRequest>(new Spl2DispatchRequest
				{
					Module = Definition,
					Namespace = "apps.search",
					QueryParameters = new Dictionary<string, Spl2DispatchQuery> { ["s1"] = new() { Earliest = "-1h", Latest = "now" } }
				}),
				Ct);
			var sid = dispatched.QueryParameters["s1"].Sid!;
			await Client.Search.WaitForCompletionAsync(sid, new SearchWaitOptions { Timeout = TimeSpan.FromMinutes(2) }, Ct);
			await Client.SearchJobs.DeleteAsync(sid, CancellationToken.None);
		});
	}

	private static JsonBody<Spl2ModulePermissionsRequest> Permissions(string resource)
		=> new(new Spl2ModulePermissionsRequest { ResourceName = resource, Permissions = [new("read", ["admin"]), new("write", ["admin"])] });

	private static async Task ShouldFailAsync(Func<Task> act, HttpStatusCode status, string message)
	{
		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(status);
		thrown.Which.Message.Should().Be(message);
	}

	/// <summary>
	/// Runs an operation that needs Splunk's SPL2 language server. The splunk/splunk 10.6 Docker image does not run it, and
	/// Splunk answers 500 naming the missing socket; anywhere it runs, the operation must succeed.
	/// </summary>
	private static async Task LanguageServerAsync(Func<Task> operation)
	{
		try
		{
			await operation();
		}
		catch (SplunkApiException exception) when (exception.StatusCode == HttpStatusCode.InternalServerError)
		{
			exception.Message.Should().StartWith("Failed to open resource handle: uds://").And.Contain("lsp-");
		}
	}
}
