using Splunk.Api.Handlers;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Core;

/// <summary>
/// The handlers that rewrite request URIs. Both build the new URI as a string, so already-escaped segments
/// (<c>%2F</c>, <c>%20</c>, UTF-8) and query values must come through unchanged.
/// </summary>
public class PathHandlerTests
{
	private static async Task<Uri> SendAsync(DelegatingHandler handler, string url)
	{
		using var harness = new HandlerHarness(handler);
		harness.Stub.Enqueue(HttpStatusCode.OK);
		using var response = await harness.SendAsync(HttpMethod.Get, url);
		return harness.Stub.Calls.Single().Uri;
	}

	[Theory]
	[InlineData("https://h/", "https://h/services/saved/searches/a%2Fb%20c", "/servicesNS/first%20last/app%2F%C3%A9/saved/searches/a%2Fb%20c", "")]
	[InlineData("https://h/", "https://h/services/x?search=a%26b%3Dc&f=%2A", "/servicesNS/first%20last/app%2F%C3%A9/x", "?search=a%26b%3Dc&f=%2A")]
	[InlineData("https://h/", "https://h/services/é", "/servicesNS/first%20last/app%2F%C3%A9/%C3%A9", "")]
	[InlineData("https://h/splunkd/__raw/", "https://h/splunkd/__raw/services/x", "/splunkd/__raw/servicesNS/first%20last/app%2F%C3%A9/x", "")]
	[InlineData("https://h/my%20proxy/", "https://h/my%20proxy/services/x", "/my%20proxy/servicesNS/first%20last/app%2F%C3%A9/x", "")]
	public async Task Namespace_RewritesGlobalServicesRequests(string baseUrl, string url, string path, string query)
	{
		var uri = await SendAsync(new NamespaceHandler(new Uri(baseUrl), new SplunkNamespace("first last", "app/é")), url);

		uri.AbsolutePath.Should().Be(path);
		uri.Query.Should().Be(query);
		uri.Authority.Should().Be("h");
	}

	[Theory]
	[InlineData("https://h:8089/", "https://h:8089/servicesNS/admin/search/x")]
	[InlineData("https://h:8089/", "https://h:8089/other/services/x")]
	[InlineData("https://h:8089/", "https://h:8089/servicesx/y")]
	[InlineData("https://h:8089/", "https://h:8089/services")]
	[InlineData("https://h:8089/p/", "https://h:8089/services/x")]
	public async Task Namespace_LeavesOtherRequestsAlone(string baseUrl, string url)
	{
		var uri = await SendAsync(new NamespaceHandler(new Uri(baseUrl), SplunkNamespace.All), url);

		uri.AbsoluteUri.Should().Be(url);
	}

	[Theory]
	[InlineData("https://h/services/x", "https://h/services/x?output_mode=json")]
	[InlineData("https://h/services/x?", "https://h/services/x?output_mode=json")]
	[InlineData("https://h/services/a%2Fb?search=a%26b%20c", "https://h/services/a%2Fb?search=a%26b%20c&output_mode=json")]
	[InlineData("https://h/services/x?output_mode=csv", "https://h/services/x?output_mode=csv")]
	[InlineData("https://h/services/x?a=1&output_mode=raw", "https://h/services/x?a=1&output_mode=raw")]
	[InlineData("https://h/services/x?my_output_mode=csv", "https://h/services/x?my_output_mode=csv&output_mode=json")]
	public async Task OutputMode_IsAddedUnlessChosen(string url, string expected)
	{
		var uri = await SendAsync(new OutputModeHandler(), url);

		uri.AbsoluteUri.Should().Be(expected);
	}

	[Theory]
	[InlineData("https://h/", "https://h/services/x", "services/x")]
	[InlineData("https://h/p", "https://h/p/services/x", "services/x")]
	[InlineData("https://h/p/", "https://h/p/", "")]
	[InlineData("https://h/p/", "https://h/q/services/x", null)]
	[InlineData("https://h/p/", "https://h/p", null)]
	public void Relative_IsThePathBelowTheBase(string baseUrl, string url, string? expected)
		=> ServicePath.Relative(new Uri(baseUrl), new Uri(url)).Should().Be(expected);

	[Theory]
	[InlineData("services/search/jobs", "search/jobs")]
	[InlineData("services/", "")]
	[InlineData("servicesNS/a/b/search/jobs/x", "search/jobs/x")]
	[InlineData("servicesNS/a/b/", "")]
	[InlineData("servicesNS/a/b", "")]
	[InlineData("servicesNS/a", "")]
	[InlineData("other/x", null)]
	[InlineData("", null)]
	public void Endpoint_StripsTheServicesPrefix(string relative, string? expected)
		=> ServicePath.Endpoint(relative).Should().Be(expected);

	[Theory]
	[InlineData("services/x", true)]
	[InlineData("servicesNS/a/b/x", false)]
	[InlineData("services", false)]
	public void IsGlobalServices_OnlyForServices(string relative, bool expected)
		=> ServicePath.IsGlobalServices(relative).Should().Be(expected);

	[Fact]
	public void ToNamespace_ReplacesThePrefix()
		=> ServicePath.ToNamespace("services/x/y", "servicesNS/a/b/").Should().Be("servicesNS/a/b/x/y");
}
