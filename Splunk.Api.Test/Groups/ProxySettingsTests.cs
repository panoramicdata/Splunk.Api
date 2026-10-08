using Splunk.Api.Models.Server;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ProxySettingsTests
{
	private const string Path = "/services/server/httpsettings/proxysettings/proxyConfig";

	// Captured from Splunk 10.6.0.5 with the proxy values filled in as the reference documents them.
	private const string ProxyContent = """
		{
			"disabled": false,
			"eai:acl": null,
			"enable_tls_proxy": null,
			"http_proxy": "http://proxy.example.com:3128",
			"https_proxy": "http://proxy.example.com:3129",
			"no_proxy": "localhost, 127.0.0.1, ::1"
		}
		""";

	[Fact]
	public async Task CreateAsync_PostsTheNameAndProxies()
		=> await Calls.AssertAsync(
			c => c.ProxySettings.CreateAsync(new ProxySettingsCreateRequest { HttpProxy = "http://p:3128", NoProxy = "localhost" }, Calls.Token),
			HttpMethod.Post, "/services/server/httpsettings/proxysettings", Calls.JsonQuery,
			"name=proxyConfig&http_proxy=http%3A%2F%2Fp%3A3128&no_proxy=localhost");

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ProxySettings.GetAsync(Calls.Token), HttpMethod.Get, Path, Calls.JsonQuery, null);

	[Fact]
	public async Task UpdateAsync_PostsTheProxies()
		=> await Calls.AssertAsync(
			c => c.ProxySettings.UpdateAsync(new ProxySettingsUpdateRequest { HttpProxy = "a", HttpsProxy = "b", NoProxy = "c" }, Calls.Token),
			HttpMethod.Post, Path, Calls.JsonQuery, "http_proxy=a&https_proxy=b&no_proxy=c");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> await Calls.AssertAsync(c => c.ProxySettings.DeleteAsync(Calls.Token), HttpMethod.Delete, Path, Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.ProxySettings.GetAsync(Calls.Token), Feed.Of("proxyConfig", ProxyContent));

		var proxy = feed.Entries.Should().ContainSingle().Subject.Content!;
		proxy.HttpProxy.Should().Be("http://proxy.example.com:3128");
		proxy.HttpsProxy.Should().Be("http://proxy.example.com:3129");
		proxy.NoProxy.Should().Be("localhost, 127.0.0.1, ::1");
		proxy.EnableTlsProxy.Should().BeNull();
		proxy.Disabled.Should().BeFalse();
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.ProxySettings.GetAsync(Calls.Token), HttpStatusCode.Forbidden);
}
