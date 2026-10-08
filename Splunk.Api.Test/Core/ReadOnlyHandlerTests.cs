using Splunk.Api.Handlers;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Core;

public class ReadOnlyHandlerTests
{
	private const string Base = "https://splunk.test:8089/splunkd/";

	private static async Task<HttpResponseMessage> SendAsync(HttpMethod method, string path)
	{
		using var harness = new HandlerHarness(new ReadOnlyHandler(new Uri(Base)));
		harness.Stub.Enqueue(HttpStatusCode.OK);
		return await harness.SendAsync(method, Base + path);
	}

	[Theory]
	[InlineData("GET", "services/saved/searches")]
	[InlineData("HEAD", "services/server/info")]
	[InlineData("POST", "services/auth/login")]
	[InlineData("POST", "services/auth/login/")]
	[InlineData("POST", "servicesNS/admin/search/auth/login")]
	[InlineData("POST", "services/search/jobs")]
	[InlineData("POST", "services/search/v2/jobs")]
	[InlineData("POST", "services/search/jobs/export")]
	[InlineData("POST", "services/search/v2/jobs/export")]
	[InlineData("POST", "servicesNS/nobody/search/search/v2/jobs/1700000000.42/control")]
	[InlineData("POST", "services/search/jobs/scheduler__admin_c2VhcmNo__RMD5%2Fx/events")]
	[InlineData("POST", "services/search/v2/jobs/sid/results")]
	[InlineData("POST", "services/search/v2/jobs/sid/results_preview")]
	[InlineData("POST", "services/search/parser")]
	[InlineData("POST", "services/search/v2/parser?q=search")]
	public async Task Allows_ReadOnlyOperations(string method, string path)
	{
		using var response = await SendAsync(new HttpMethod(method), path);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Theory]
	[InlineData("DELETE", "services/search/jobs/sid")]
	[InlineData("PUT", "services/search/jobs")]
	[InlineData("PATCH", "services/search/jobs")]
	[InlineData("POST", "services/saved/searches")]
	[InlineData("POST", "services/search/jobs/sid")]
	[InlineData("POST", "services/search/jobs/sid/acl")]
	[InlineData("POST", "services/search/jobs/a/b/control")]
	[InlineData("POST", "services/search/v3/jobs")]
	[InlineData("POST", "services/search/jobs/export/extra")]
	[InlineData("POST", "services/Search/Jobs")]
	[InlineData("POST", "services/auth/login/x")]
	[InlineData("POST", "servicesNS/admin/search")]
	[InlineData("POST", "other/auth/login")]
	[InlineData("POST", "services/search/jobs/../../apps/local")]
	public async Task Refuses_EverythingElse(string method, string path)
	{
		var act = () => SendAsync(new HttpMethod(method), path);

		var thrown = await act.Should().ThrowAsync<SplunkReadOnlyException>();
		thrown.Which.Method.Should().Be(method);
		thrown.Which.Path.Should().NotContain("?");
	}

	[Fact]
	public async Task Refuses_RequestsOutsideTheBaseAddress()
	{
		using var harness = new HandlerHarness(new ReadOnlyHandler(new Uri(Base)));

		var act = () => harness.SendAsync(HttpMethod.Post, "https://splunk.test:8089/services/search/jobs?search=x");

		var thrown = await act.Should().ThrowAsync<SplunkReadOnlyException>();
		thrown.Which.Path.Should().Be("https://splunk.test:8089/services/search/jobs");
		thrown.Which.Message.Should().Be("The Splunk client is read-only and refused POST https://splunk.test:8089/services/search/jobs.");
		harness.Stub.Calls.Should().BeEmpty();
	}

	[Fact]
	public async Task Client_ReadOnly_RefusesBeforeSending()
	{
		var stub = new StubHandler();
		using var client = TestClient.Create(stub, o => o.ReadOnly = true);
		var probe = client.For<IProbe>();

		var act = () => probe.DeleteAsync("services/saved/searches/x", TestContext.Current.CancellationToken);

		var thrown = await act.Should().ThrowAsync<SplunkReadOnlyException>();
		thrown.Which.Path.Should().Be("https://splunk.test:8089/services/saved/searches/x");
		stub.Calls.Should().BeEmpty();
	}

	[Fact]
	public async Task Client_ReadOnly_SeesTheNamespacedPath()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.Created);
		using var client = TestClient.Create(stub, o => (o.ReadOnly, o.Namespace) = (true, SplunkNamespace.Shared("search")));
		var probe = client.For<IProbe>();

		await probe.PostAsync("services/search/v2/jobs", new Dictionary<string, string?> { ["search"] = "search *" }, TestContext.Current.CancellationToken);

		stub.Calls[0].Uri.AbsolutePath.Should().Be("/servicesNS/nobody/search/search/v2/jobs");
	}

	[Fact]
	public async Task Client_NotReadOnly_SendsWrites()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.OK);
		using var client = TestClient.Create(stub);

		await client.For<IProbe>().DeleteAsync("services/saved/searches/x", TestContext.Current.CancellationToken);

		stub.Calls[0].Method.Should().Be(HttpMethod.Delete);
	}
}
