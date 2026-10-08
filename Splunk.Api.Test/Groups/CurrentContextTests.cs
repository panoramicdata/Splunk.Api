using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class CurrentContextTests
{
	[Fact]
	public async Task GetAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.CurrentContext.GetAsync(ct), UserJson.Context))
			.ShouldBe(HttpMethod.Get, "/services/authentication/current-context");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.CurrentContext.GetAsync(ct), UserJson.Context);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("context");
		entry.Content!.Username.Should().Be("admin");
		entry.Content.ShouldBeTheCapturedAdmin();
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.CurrentContext.GetAsync(ct), HttpStatusCode.Unauthorized);
}
