using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Core;

public class ClientTests
{
	private const string Empty = """{"entry":[]}""";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private static StubHandler Stub(int responses)
	{
		var stub = new StubHandler();
		for (var i = 0; i < responses; i++)
		{
			stub.Enqueue(HttpStatusCode.OK, Empty);
		}

		return stub;
	}

	private sealed class TrackingHandler : HttpMessageHandler
	{
		public bool Disposed { get; private set; }

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(Empty), RequestMessage = request });
		}

		protected override void Dispose(bool disposing)
		{
			Disposed = true;
			base.Dispose(disposing);
		}
	}

	[Fact]
	public void Constructors_RequireArguments()
	{
		var options = new SplunkClientOptions { BaseUrl = TestClient.BaseUrl, Token = "t" };

		((Action)(() => _ = new SplunkClient(null!))).Should().Throw<ArgumentNullException>();
		((Action)(() => _ = new SplunkClient(null!, new StubHandler()))).Should().Throw<ArgumentNullException>();
		((Action)(() => _ = new SplunkClient(options, null!))).Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void Constructor_ValidatesOptions()
	{
		var act = () => new SplunkClient(new SplunkClientOptions { BaseUrl = "nope", Token = "t" });

		act.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("BaseUrl");
	}

	[Fact]
	public void Constructor_WithOwnTransport_CreatesAUsableClient()
	{
		using var client = new SplunkClient(new SplunkClientOptions { BaseUrl = "https://splunk.test:8089", Token = "t" });

		client.BaseAddress.Should().Be(new Uri("https://splunk.test:8089/"));
		client.Namespace.Should().BeNull();
		client.ServerInfo.Should().NotBeNull().And.BeSameAs(client.ServerInfo);
	}

	[Theory]
	[InlineData("https://splunk.test:8089", "https://splunk.test:8089/")]
	[InlineData("https://splunk.test:8089/", "https://splunk.test:8089/")]
	[InlineData("http://proxy.test/splunkd/__raw", "http://proxy.test/splunkd/__raw/")]
	public void BaseAddress_AlwaysEndsWithSlash(string baseUrl, string expected)
	{
		using var client = TestClient.Create(new StubHandler(), o => o.BaseUrl = baseUrl);

		client.BaseAddress.AbsoluteUri.Should().Be(expected);
	}

	[Theory]
	[InlineData("https://proxy.test/splunkd/__raw", null, "/splunkd/__raw/services/server/info")]
	[InlineData("https://proxy.test/splunkd/__raw/", "nobody/search", "/splunkd/__raw/servicesNS/nobody/search/server/info")]
	[InlineData("https://splunk.test:8089", "-/-", "/servicesNS/-/-/server/info")]
	public async Task Requests_KeepThePathPrefixAndNamespace(string baseUrl, string? ns, string expectedPath)
	{
		var stub = Stub(1);
		var parts = ns?.Split('/');
		using var client = TestClient.Create(stub, o =>
		{
			o.BaseUrl = baseUrl;
			o.Namespace = parts is null ? null : new SplunkNamespace(parts[0], parts[1]);
		});

		await client.ServerInfo.GetAsync(Ct);

		stub.Calls[0].Uri.AbsolutePath.Should().Be(expectedPath);
		stub.Calls[0].Uri.Query.Should().Be("?output_mode=json");
	}

	[Fact]
	public async Task InNamespace_EscapesEachPartAsOneSegment()
	{
		var stub = Stub(1);
		using var client = TestClient.Create(stub);
		using var view = client.InNamespace("first last", "app/é");

		await view.ServerInfo.GetAsync(Ct);

		view.Namespace.Should().Be(new SplunkNamespace("first last", "app/é"));
		view.BaseAddress.Should().BeSameAs(client.BaseAddress);
		stub.Calls[0].Uri.AbsolutePath.Should().Be("/servicesNS/first%20last/app%2F%C3%A9/server/info");
	}

	[Fact]
	public async Task InNamespace_ReplacesTheClientsNamespace()
	{
		var stub = Stub(2);
		using var client = TestClient.Create(stub, o => o.Namespace = SplunkNamespace.Shared("search"));
		using var view = client.InNamespace(SplunkNamespace.All);

		await client.ServerInfo.GetAsync(Ct);
		await view.ServerInfo.GetAsync(Ct);

		stub.Calls.Select(c => c.Uri.AbsolutePath).Should().Equal("/servicesNS/nobody/search/server/info", "/servicesNS/-/-/server/info");
	}

	[Fact]
	public void InNamespace_RejectsInvalidNamespaces()
	{
		using var client = TestClient.Create(new StubHandler());

		((Action)(() => client.InNamespace(null!))).Should().Throw<ArgumentNullException>();
		((Action)(() => client.InNamespace("", "search"))).Should().Throw<ArgumentException>().Which.ParamName.Should().Be("Owner");
		((Action)(() => client.InNamespace("admin", "."))).Should().Throw<ArgumentException>().Which.ParamName.Should().Be("App");
	}

	[Fact]
	public async Task InNamespace_SharesTheSession()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.OK, TestClient.LoginJson("key-1"));
		stub.Enqueue(HttpStatusCode.OK, Empty);
		stub.Enqueue(HttpStatusCode.OK, Empty);
		using var client = TestClient.Create(stub, TestClient.UseSession);
		using var view = client.InNamespace("admin", "search");

		await client.ServerInfo.GetAsync(Ct);
		await view.ServerInfo.GetAsync(Ct);

		stub.Calls.Should().HaveCount(3);
		stub.Calls[0].Uri.AbsolutePath.Should().Be("/services/auth/login");
		stub.Calls.Skip(1).Select(c => c.Headers.Authorization!.ToString()).Should().AllBe("Splunk key-1");
	}

	[Fact]
	public async Task DisposingAView_LeavesTheClientUsable()
	{
		var handler = new TrackingHandler();
		using var owner = new SplunkClient(new SplunkClientOptions { BaseUrl = TestClient.BaseUrl, Token = "t" }, handler);

		owner.InNamespace(SplunkNamespace.All).Dispose();
		var feed = await owner.ServerInfo.GetAsync(Ct);

		feed.Entries.Should().BeEmpty();
		handler.Disposed.Should().BeFalse();
	}

	[Fact]
	public async Task DisposingTheClient_DisposesTheTransportAndStopsViews()
	{
		var handler = new TrackingHandler();
		var owner = new SplunkClient(new SplunkClientOptions { BaseUrl = TestClient.BaseUrl, Token = "t" }, handler);
		var view = owner.InNamespace(SplunkNamespace.All);

		owner.Dispose();
		owner.Dispose();
		var act = () => view.ServerInfo.GetAsync(Ct);

		handler.Disposed.Should().BeTrue();
		await act.Should().ThrowAsync<ObjectDisposedException>();
	}
}
