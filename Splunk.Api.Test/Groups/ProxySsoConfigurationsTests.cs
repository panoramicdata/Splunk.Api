using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class ProxySsoConfigurationsTests
{
	// Shaped after the reference (enabling ProxySSO on the shared test instance would change every login).
	private const string ConfigurationJson = """
		{
			"entry": [
				{
					"name": "proxy1",
					"content": {
						"title": "proxy1",
						"defaultRoleIfMissing": "user",
						"excludedUsers": "admin",
						"excludedAutoMappedRoles": "can_delete",
						"blacklistedUsers": "admin",
						"blacklistedAutoMappedRoles": "can_delete",
						"disabled": "0"
					}
				}
			]
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoConfigurations.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/ProxySSO-auth");

	[Fact]
	public async Task CreateAsync_PostsTheConfiguration()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoConfigurations.CreateAsync(
			new ProxySsoConfigurationCreateRequest
			{
				Name = "proxy1",
				DefaultRoleIfMissing = "user",
				ExcludedUsers = "admin",
				ExcludedAutoMappedRoles = "can_delete",
				BlacklistedUsers = "root",
				BlacklistedAutoMappedRoles = "admin"
			},
			ct)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/admin/ProxySSO-auth",
				"name=proxy1&defaultRoleIfMissing=user&excludedUsers=admin&excludedAutoMappedRoles=can_delete&blacklistedUsers=root&blacklistedAutoMappedRoles=admin");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoConfigurations.GetAsync("proxy1", ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/ProxySSO-auth/proxy1");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoConfigurations.UpdateAsync("proxy1", new ProxySsoConfigurationUpdateRequest { DefaultRoleIfMissing = "power" }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/admin/ProxySSO-auth/proxy1", "defaultRoleIfMissing=power");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoConfigurations.DeleteAsync("proxy1", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/admin/ProxySSO-auth/proxy1");

	[Fact]
	public async Task DisableAsync_SendsGetToDisable()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoConfigurations.DisableAsync("proxy1", ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/ProxySSO-auth/proxy1/disable");

	[Fact]
	public async Task EnableAsync_SendsGetToEnable()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoConfigurations.EnableAsync("proxy1", ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/ProxySSO-auth/proxy1/enable");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.ProxySsoConfigurations.GetAsync("proxy1", ct), ConfigurationJson);

		var proxy = feed.Entries.Should().ContainSingle().Subject.Content!;
		proxy.Title.Should().Be("proxy1");
		proxy.DefaultRoleIfMissing.Should().Be("user");
		proxy.ExcludedUsers.Should().Be("admin");
		proxy.ExcludedAutoMappedRoles.Should().Be("can_delete");
		proxy.BlacklistedUsers.Should().Be("admin");
		proxy.BlacklistedAutoMappedRoles.Should().Be("can_delete");
		proxy.Disabled.Should().BeFalse();
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.ProxySsoConfigurations.GetAsync("missing", ct));
}
