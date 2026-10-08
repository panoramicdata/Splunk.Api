using Splunk.Api.Models;
using Splunk.Api.Models.Access;
using System.Net;

namespace Splunk.Api.IntegrationTest.Access;

[Collection(SplunkTestGroup.Name)]
public class UsersIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task CurrentContext_IsTheConfiguredUser()
	{
		var feed = await fixture.Client.CurrentContext.GetAsync(TestContext.Current.CancellationToken);

		var context = feed.Entries.Should().ContainSingle().Subject;
		context.Name.Should().Be("context");
		context.Content!.Username.Should().Be(fixture.CreateOptions().Username);
		context.Content.Roles.Should().NotBeEmpty();
		context.Content.Capabilities.Should().NotBeEmpty();
		context.Content.Password.Should().Be("********");
	}

	[Fact]
	public async Task ListAsync_IncludesTheAdmin()
	{
		var feed = await fixture.Client.Users.ListAsync(new ListOptions { Count = 0 }, TestContext.Current.CancellationToken);

		var admin = feed.Entries.Should().Contain(e => e.Name == "admin").Subject;
		admin.Content!.Roles.Should().Contain("admin");
		admin.Content.Type.Should().Be("Splunk");
	}

	[Fact]
	public async Task CreateGetUpdateDelete_RoundTrips()
	{
		var ct = TestContext.Current.CancellationToken;
		var name = SplunkFixture.UniqueName("user");
		try
		{
			var created = await fixture.Client.Users.CreateAsync(
				new UserCreateRequest
				{
					Name = name,
					Password = "It-" + Guid.NewGuid().ToString("N"),
					Roles = ["user", "power"],
					RealName = "Integration Test",
					Email = "splunk-api-it@example.com",
					DefaultApp = "search",
					TimeZone = "Europe/London",
					ForceChangePassword = false
				},
				ct);
			created.Entries.Should().ContainSingle().Which.Name.Should().Be(name);

			var user = (await fixture.Client.Users.GetAsync(name, ct)).Entries.Should().ContainSingle().Subject.Content!;
			user.Roles.Order().Should().Equal("power", "user");
			user.RealName.Should().Be("Integration Test");
			user.Email.Should().Be("splunk-api-it@example.com");
			user.DefaultApp.Should().Be("search");
			user.DefaultAppIsUserOverride.Should().BeTrue();
			user.TimeZone.Should().Be("Europe/London");
			user.LockedOut.Should().BeFalse();

			var updated = await fixture.Client.Users.UpdateAsync(name, new UserUpdateRequest { RealName = "Renamed", Roles = ["user"] }, ct);
			updated.Entries.Should().ContainSingle().Which.Content!.RealName.Should().Be("Renamed");
			updated.Entries[0].Content!.Roles.Should().Equal("user");
		}
		finally
		{
			await fixture.Client.Users.DeleteAsync(name, ct);
		}

		var act = () => fixture.Client.Users.GetAsync(name, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
