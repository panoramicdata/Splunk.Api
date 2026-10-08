using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class GlobalBannersTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET data/ui/global-banner); the link added.
	private static readonly string BannerJson = KnowledgeTestKit.Feed("data/ui/global-banner", GlobalBannerCreateRequest.SingletonName, """
		{
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "search",
			"eai:userName": "admin",
			"global_banner.background_color": "blue",
			"global_banner.hyperlink": "https://status.example.com",
			"global_banner.hyperlink_text": "Status",
			"global_banner.message": "Sample banner notification text. Please replace with your own message.",
			"global_banner.visible": false
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToGlobalBanner()
		=> (await KnowledgeTestKit.SendAsync(c => c.GlobalBanners.ListAsync(TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/ui/global-banner");

	[Fact]
	public async Task CreateAsync_PostsThePrefixedFields()
		=> (await KnowledgeTestKit.SendAsync(c => c.GlobalBanners.CreateAsync(
				new GlobalBannerCreateRequest
				{
					Name = GlobalBannerCreateRequest.SingletonName,
					Message = "Maintenance tonight",
					Visible = true,
					BackgroundColor = "orange",
					Hyperlink = "https://status.example.com",
					HyperlinkText = "Status"
				},
				TestContext.Current.CancellationToken)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/data/ui/global-banner",
				body: "name=BANNER_MESSAGE_SINGLETON&global_banner.message=Maintenance+tonight&global_banner.visible=true&global_banner.background_color=orange&global_banner.hyperlink=https%3A%2F%2Fstatus.example.com&global_banner.hyperlink_text=Status");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.GlobalBanners.ListAsync(TestContext.Current.CancellationToken), BannerJson);

		entry.ShouldBeTheCapturedEntry("BANNER_MESSAGE_SINGLETON");
		var banner = entry.Content!;
		banner.Message.Should().StartWith("Sample banner notification text.");
		banner.Visible.Should().BeFalse();
		banner.BackgroundColor.Should().Be("blue");
		banner.Hyperlink.Should().Be("https://status.example.com");
		banner.HyperlinkText.Should().Be("Status");
	}

	[Fact]
	public Task ListAsync_Error_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.GlobalBanners.ListAsync(TestContext.Current.CancellationToken));
}
