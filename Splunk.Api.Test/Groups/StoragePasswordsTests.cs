using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class StoragePasswordsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET storage/passwords/{name} for a test credential), trimmed; host and secrets replaced.
	private const string PasswordJson = """
		{
			"links": { "create": "/services/storage/passwords/_new", "_reload": "/services/storage/passwords/_reload" },
			"origin": "https://splunk.test:8089/services/storage/passwords",
			"entry": [
				{
					"name": "splunk_api_it_realm:splunk_api_it_user:",
					"id": "https://splunk.test:8089/servicesNS/nobody/search/storage/passwords/splunk_api_it_realm%3Asplunk_api_it_user%3A",
					"author": "admin",
					"acl": { "app": "search", "owner": "nobody", "sharing": "app", "can_write": true, "perms": { "read": ["*"], "write": ["admin"] } },
					"content": {
						"clear_password": "dummy-secret",
						"eai:acl": null,
						"encr_password": "$7$ZmFrZQ==",
						"password": "********",
						"realm": "splunk_api_it_realm",
						"username": "splunk_api_it_user"
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.StoragePasswords.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/storage/passwords");

	[Fact]
	public async Task CreateAsync_PostsTheCredential()
		=> (await EndpointRequests.SendAsync((c, ct) => c.StoragePasswords.CreateAsync(new StoredPasswordCreateRequest { Name = "svc", Password = "p@ss word", Realm = "api" }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/storage/passwords", "name=svc&password=p%40ss+word&realm=api");

	[Fact]
	public async Task GetAsync_EscapesTheColonsOfTheName()
		=> (await EndpointRequests.SendAsync((c, ct) => c.StoragePasswords.GetAsync("api:svc:", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/storage/passwords/api%3Asvc%3A");

	[Fact]
	public async Task GetAsync_InAnAppNamespace_UsesServicesNS()
	{
		var stub = TestClient.Stub(PasswordJson);
		using var client = TestClient.Create(stub);

		await client.InNamespace("nobody", "search").StoragePasswords.GetAsync("api:svc:", TestContext.Current.CancellationToken);

		stub.Calls.Should().ContainSingle().Which.Uri.AbsolutePath.Should().Be("/servicesNS/nobody/search/storage/passwords/api%3Asvc%3A");
	}

	[Fact]
	public async Task UpdateAsync_PostsTheNewPassword()
		=> (await EndpointRequests.SendAsync((c, ct) => c.StoragePasswords.UpdateAsync("api:svc:", new StoredPasswordUpdateRequest { Password = "new" }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/storage/passwords/api%3Asvc%3A", "password=new");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await EndpointRequests.SendAsync((c, ct) => c.StoragePasswords.DeleteAsync("api:svc:", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/storage/passwords/api%3Asvc%3A");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.StoragePasswords.GetAsync("splunk_api_it_realm:splunk_api_it_user:", ct), PasswordJson);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("splunk_api_it_realm:splunk_api_it_user:");
		entry.Content!.Username.Should().Be("splunk_api_it_user");
		entry.Content.Realm.Should().Be("splunk_api_it_realm");
		entry.Content.ClearPassword.Should().Be("dummy-secret");
		entry.Content.EncryptedPassword.Should().Be("$7$ZmFrZQ==");
		entry.Content.Password.Should().Be("********");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.StoragePasswords.GetAsync("missing", ct));
}
