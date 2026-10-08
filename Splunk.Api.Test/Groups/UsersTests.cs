using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class UsersTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.Users.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/authentication/users");

	[Fact]
	public async Task CreateAsync_PostsTheUser()
		=> (await RequestAssert.SendAsync((c, ct) => c.Users.CreateAsync(
			new UserCreateRequest
			{
				Name = "jo",
				Password = "s3cret!",
				CreateRole = false,
				Roles = ["user", "power"],
				RealName = "Jo Bloggs",
				Email = "jo@example.com",
				DefaultApp = "search",
				TimeZone = "Europe/London",
				Language = "en-GB",
				ForceChangePassword = true,
				RestartBackgroundJobs = false,
				LockedOut = false
			},
			ct)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/authentication/users",
				"name=jo&password=s3cret%21&createrole=false&roles=user&roles=power&realname=Jo+Bloggs&email=jo%40example.com&defaultApp=search&tz=Europe%2FLondon&lang=en-GB&force-change-pass=true&restart_background_jobs=false&locked-out=false");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.Users.GetAsync("jo", ct)))
			.ShouldBe(HttpMethod.Get, "/services/authentication/users/jo");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await RequestAssert.SendAsync((c, ct) => c.Users.UpdateAsync("jo", new UserUpdateRequest { RealName = "Jo", Password = "new", OldPassword = "old" }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/authentication/users/jo", "realname=Jo&password=new&oldpassword=old");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.Users.DeleteAsync("jo", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/authentication/users/jo");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.Users.GetAsync("admin", ct), UserJson.User);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("admin");
		entry.Content!.ShouldBeTheCapturedAdmin();
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.Users.GetAsync("missing", ct));
}
