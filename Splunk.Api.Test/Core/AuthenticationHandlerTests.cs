using Splunk.Api.Handlers;
using Splunk.Api.Test.Support;
using System.Net;
using System.Reflection;

namespace Splunk.Api.Test.Core;

public partial class AuthenticationHandlerTests
{
	private const string Base = "https://splunk.test:8089/";
	private const string Info = Base + "services/server/info";
	private const string LoginUrl = Base + "services/auth/login?output_mode=json";

	private static AuthenticationHandler Handler(Action<SplunkClientOptions>? tweak = null)
	{
		var options = new SplunkClientOptions();
		TestClient.UseSession(options);
		tweak?.Invoke(options);
		return new AuthenticationHandler(options, new Uri(Base));
	}

	private static HandlerHarness Session(params string[] responses)
	{
		var harness = new HandlerHarness(Handler());
		foreach (var response in responses)
		{
			harness.Stub.Enqueue(response.StartsWith('4') ? (HttpStatusCode)int.Parse(response[..3], System.Globalization.CultureInfo.InvariantCulture) : HttpStatusCode.OK, response.StartsWith('4') ? response[4..] : response);
		}

		return harness;
	}

	private static string Login(string key) => TestClient.LoginJson(key);

	/// <summary>Sends one GET through a handler configured by <paramref name="configure"/>; returns the Authorization header sent.</summary>
	private static async Task<string> AuthorizationSentAsync(Action<SplunkClientOptions> configure)
	{
		using var harness = new HandlerHarness(Handler(configure));
		harness.Stub.Enqueue(HttpStatusCode.OK);

		using var response = await harness.SendAsync(HttpMethod.Get, Info);

		return harness.Stub.Calls.Single().Headers.Authorization!.ToString();
	}

	[Fact]
	public async Task Token_IsSentAsBearer()
		=> (await AuthorizationSentAsync(o => o.Token = "the-token")).Should().Be("Bearer the-token");

	[Fact]
	public async Task Basic_IsSentOnEveryRequest()
		=> (await AuthorizationSentAsync(o => (o.UseBasicAuthentication, o.Password) = (true, "pä:ss")))
			.Should().Be("Basic " + Convert.ToBase64String("admin:pä:ss"u8.ToArray()));

	[Fact]
	public async Task Session_LogsInOnce_ThenSendsTheKey()
	{
		using var harness = Session(Login("key-1"), "{}", "{}");

		using var first = await harness.SendAsync(HttpMethod.Get, Info);
		using var second = await harness.SendAsync(HttpMethod.Get, Info);

		var calls = harness.Stub.Calls;
		calls.Should().HaveCount(3);
		calls[0].Method.Should().Be(HttpMethod.Post);
		calls[0].Uri.AbsoluteUri.Should().Be(LoginUrl);
		calls[0].Body.Should().Be("username=admin&password=secret");
		calls[0].Headers.Authorization.Should().BeNull();
		calls.Skip(1).Select(c => c.Headers.Authorization!.ToString()).Should().AllBe("Splunk key-1");
	}

	[Fact]
	public async Task Session_LogsInAgainOnce_On401_AndReplaysTheBody()
	{
		using var harness = Session(Login("key-1"), "401 {}", Login("key-2"), "{}");

		using var response = await harness.SendAsync(HttpMethod.Post, Base + "services/saved/searches", new FormUrlEncodedContent([new("name", "x")]));

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var calls = harness.Stub.Calls;
		calls.Select(c => c.Uri.AbsolutePath).Should().Equal("/services/auth/login", "/services/saved/searches", "/services/auth/login", "/services/saved/searches");
		calls[1].Authorization.Should().Be("Splunk key-1");
		calls[3].Authorization.Should().Be("Splunk key-2");
		calls[3].Body.Should().Be("name=x");
	}

	[Fact]
	public async Task Session_A401AfterLoggingInAgain_IsReturned()
	{
		using var harness = Session(Login("key-1"), "401 {}", Login("key-2"), "401 {}");

		using var response = await harness.SendAsync(HttpMethod.Get, Info);

		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
		harness.Stub.Calls.Should().HaveCount(4);
	}

	[Fact]
	public async Task Session_A401ForAStreamBody_IsReturnedWithoutLoggingIn()
	{
		using var harness = Session(Login("key-1"), "401 {}");

		using var response = await harness.SendAsync(HttpMethod.Post, Info, new StreamContent(new MemoryStream([1])));

		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
		harness.Stub.Calls.Should().HaveCount(2);
	}

	[Fact]
	public async Task Session_FailedLogin_IsReturnedForTheCallersRequest()
	{
		using var harness = Session("401 {\"messages\":[{\"type\":\"WARN\",\"text\":\"Login failed\"}]}");

		using var response = await UnauthorizedForTheCallersRequestAsync(harness);

		(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken)).Should().Contain("Login failed");
	}

	[Fact]
	public async Task Session_FailedLoginAfter401_IsReturnedForTheCallersRequest()
	{
		using var harness = Session(Login("key-1"), "401 {}", "401 {}");

		using var response = await UnauthorizedForTheCallersRequestAsync(harness);

		harness.Stub.Calls.Should().HaveCount(3);
	}

	/// <summary>Sends a GET and checks that the 401 that comes back is answered for the caller's own request.</summary>
	private static async Task<HttpResponseMessage> UnauthorizedForTheCallersRequestAsync(HandlerHarness harness)
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, Info);
		var response = await harness.SendAsync(request);
		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
		response.RequestMessage.Should().BeSameAs(request);
		return response;
	}

	[Fact]
	public async Task Session_LoginWithoutAKey_Throws()
	{
		using var harness = Session("{}");

		var act = () => harness.SendAsync(HttpMethod.Get, Info);

		await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*did not contain a session key*");
	}

	[Theory]
	[InlineData("services/auth/login")]
	[InlineData("services/auth/login/")]
	[InlineData("servicesNS/admin/search/auth/login")]
	public async Task Session_ExplicitLoginCalls_PassThrough(string path)
	{
		using var harness = Session("{}");

		using var response = await harness.SendAsync(HttpMethod.Post, Base + path, new FormUrlEncodedContent([new("username", "u")]));

		harness.Stub.Calls.Should().ContainSingle().Which.Headers.Authorization.Should().BeNull();
	}

	[Theory]
	[InlineData(Base + "services/auth/login/extra")]
	[InlineData(Base + "other/auth/login")]
	[InlineData("https://elsewhere.test/services/server/info")]
	[InlineData("https://splunk.test:8089")]
	public async Task Session_OtherRequests_AreAuthenticated(string url)
	{
		using var harness = Session(Login("key-1"), "{}");

		using var response = await harness.SendAsync(HttpMethod.Get, url);

		harness.Stub.Calls[1].Headers.Authorization!.ToString().Should().Be("Splunk key-1");
	}

	[Theory]
	[InlineData("""{"sessionKey":"abc"}""", "abc")]
	[InlineData("""{"other":1,"sessionKey":"k"}""", "k")]
	public void ReadSessionKey_ReadsTheKey(string body, string expected)
		=> AuthenticationHandler.ReadSessionKey(body).Should().Be(expected);

	[Theory]
	[InlineData("")]
	[InlineData("<response/>")]
	[InlineData("[]")]
	[InlineData("null")]
	[InlineData("{}")]
	[InlineData("""{"sessionKey":1}""")]
	[InlineData("""{"sessionKey":""}""")]
	[InlineData("""{"sessionKey":null}""")]
	public void ReadSessionKey_RejectsOtherBodies(string body)
	{
		var act = () => AuthenticationHandler.ReadSessionKey(body);

		act.Should().Throw<InvalidOperationException>().WithMessage("Splunk's login response did not contain a session key.");
	}

	[Fact]
	public void Dispose_CanBeRepeated_AndToleratesTheFinalizerPath()
	{
		var handler = Handler();
		var dispose = typeof(AuthenticationHandler).GetMethod("Dispose", BindingFlags.Instance | BindingFlags.NonPublic, [typeof(bool)])!;

		dispose.Invoke(handler, [false]);
		handler.Dispose();
		var act = handler.Dispose;

		act.Should().NotThrow();
	}
}
