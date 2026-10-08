using Splunk.Api.Models.Applications;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class AppsTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.Apps.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/apps/local");

	[Fact]
	public async Task CreateAsync_PostsTheApp()
		=> (await RequestAssert.SendAsync((c, ct) => c.Apps.CreateAsync(
			new AppCreateRequest
			{
				Name = "my_app",
				Template = "barebones",
				Filename = false,
				ExplicitAppName = "mine",
				Update = false,
				Auth = "a",
				Session = "s",
				Label = "My App",
				Description = "d",
				Author = "me",
				Version = "1.0.0",
				Visible = true,
				Configured = false
			},
			ct)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/apps/local",
				"name=my_app&template=barebones&filename=false&explicit_appname=mine&update=false&auth=a&session=s&label=My+App&description=d&author=me&version=1.0.0&visible=true&configured=false");

	[Fact]
	public async Task GetAsync_SendsRefresh()
		=> (await RequestAssert.SendAsync((c, ct) => c.Apps.GetAsync("search", new AppGetOptions { Refresh = true }, ct)))
			.ShouldBe(HttpMethod.Get, "/services/apps/local/search", query: "?refresh=true&output_mode=json");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await RequestAssert.SendAsync((c, ct) => c.Apps.UpdateAsync("my_app", new AppUpdateRequest { Description = "new", CheckForUpdates = false }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/apps/local/my_app", "description=new&check_for_updates=false");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.Apps.DeleteAsync("my_app", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/apps/local/my_app");

	[Fact]
	public async Task GetSetupAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.Apps.GetSetupAsync("search", ct)))
			.ShouldBe(HttpMethod.Get, "/services/apps/local/search/setup");

	[Fact]
	public async Task CheckForUpdateAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.Apps.CheckForUpdateAsync("search", ct)))
			.ShouldBe(HttpMethod.Get, "/services/apps/local/search/update");

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.Apps.DeleteAsync("missing", ct));
}
