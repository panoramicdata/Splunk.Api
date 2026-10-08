using Splunk.Api.Models;
using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class DuoMfaTests
{
	// From the reference's example (a live standalone instance has no Duo configuration); keys replaced.
	private const string DuoJson = """
		{
			"links": { "create": "/services/admin/Duo-MFA/_new" },
			"entry": [
				{
					"name": "duo-mfa",
					"author": "nobody",
					"acl": { "app": "system", "owner": "nobody", "sharing": "system", "perms": { "read": ["*"], "write": ["*"] } },
					"content": {
						"apiHostname": "api-00000000.duosecurity.com",
						"appSecretKey": "$1$app",
						"integrationKey": "$1$integration",
						"secretKey": "$1$secret",
						"failOpen": "0",
						"timeout": "5",
						"sslVersions": "tls1.2",
						"cipherSuite": "ECDHE-RSA-AES256-GCM-SHA384",
						"ecdhCurves": "prime256v1",
						"sslVerifyServerCert": "true",
						"sslRootCAPath": "/opt/splunk/etc/auth/ca.pem",
						"sslCommonNameToCheck": "*.duosecurity.com",
						"sslAltNameToCheck": "duo.example.com",
						"useClientSSLCompression": "true",
						"eai:acl": null
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.DuoMfa.ListAsync(new ListOptions { Count = 5 }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/Duo-MFA", query: "?count=5&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsTheConfiguration()
		=> (await EndpointRequests.SendAsync((c, ct) => c.DuoMfa.CreateAsync(
			new DuoMfaCreateRequest
			{
				Name = "duo",
				IntegrationKey = "DIXXXXXXXXXXXXXXXXXX",
				SecretKey = "secret",
				ApiHostname = "api-0.duosecurity.com",
				AppSecretKey = "abc",
				FailOpen = false,
				Timeout = 10,
				SslVersions = "tls1.2",
				CipherSuite = "c",
				EcdhCurves = "e",
				SslVerifyServerCert = true,
				SslRootCAPath = "/ca.pem",
				SslCommonNameToCheck = "cn",
				SslAltNameToCheck = "alt",
				UseClientSslCompression = true,
				EnableMfaAuthRest = false
			},
			ct)))
			.ShouldBeEndpointRequest(
				HttpMethod.Post,
				"/services/admin/Duo-MFA",
				"name=duo&integrationKey=DIXXXXXXXXXXXXXXXXXX&secretKey=secret&apiHostname=api-0.duosecurity.com&appSecretKey=abc&failOpen=false&timeout=10&sslVersions=tls1.2&cipherSuite=c&ecdhCurves=e&sslVerifyServerCert=true&sslRootCAPath=%2Fca.pem&sslCommonNameToCheck=cn&sslAltNameToCheck=alt&useClientSSLCompression=true&enableMfaAuthRest=false");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await EndpointRequests.SendAsync((c, ct) => c.DuoMfa.GetAsync("duo-mfa", ct), DuoJson))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/Duo-MFA/duo-mfa");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await EndpointRequests.SendAsync((c, ct) => c.DuoMfa.UpdateAsync(
			"duo-mfa",
			new DuoMfaUpdateRequest { IntegrationKey = "k", SecretKey = "s", ApiHostname = "h", Timeout = 20 },
			ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/admin/Duo-MFA/duo-mfa", "timeout=20&integrationKey=k&secretKey=s&apiHostname=h");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await EndpointRequests.SendAsync((c, ct) => c.DuoMfa.DeleteAsync("duo-mfa", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/admin/Duo-MFA/duo-mfa");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.DuoMfa.ListAsync(null, ct), DuoJson);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("duo-mfa");
		var duo = entry.Content!;
		duo.ApiHostname.Should().Be("api-00000000.duosecurity.com");
		duo.AppSecretKey.Should().Be("$1$app");
		duo.IntegrationKey.Should().Be("$1$integration");
		duo.SecretKey.Should().Be("$1$secret");
		duo.FailOpen.Should().BeFalse();
		duo.Timeout.Should().Be(5);
		duo.SslVersions.Should().Be("tls1.2");
		duo.CipherSuite.Should().Be("ECDHE-RSA-AES256-GCM-SHA384");
		duo.EcdhCurves.Should().Be("prime256v1");
		duo.SslVerifyServerCert.Should().BeTrue();
		duo.SslRootCAPath.Should().Be("/opt/splunk/etc/auth/ca.pem");
		duo.SslCommonNameToCheck.Should().Be("*.duosecurity.com");
		duo.SslAltNameToCheck.Should().Be("duo.example.com");
		duo.UseClientSslCompression.Should().BeTrue();
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.DuoMfa.GetAsync("missing", ct));
}
