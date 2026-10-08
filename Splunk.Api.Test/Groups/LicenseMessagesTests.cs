using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LicenseMessagesTests
{
	// Shaped after the reference (the shared test instance has no licenser messages).
	private const string MessagesJson = """
		{
			"entry": [
				{
					"name": "2541354125",
					"content": {
						"category": "pool_over_quota",
						"create_time": 1791465098,
						"description": "Daily indexing volume limit exceeded today.",
						"peer_id": "00000000-0000-0000-0000-000000000001",
						"pool_id": "auto_generated_pool_enterprise",
						"severity": "WARN",
						"stack_id": "enterprise"
					}
				}
			]
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.LicenseMessages.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/licenser/messages");

	[Fact]
	public async Task GetAsync_SendsGetForTheId()
		=> (await RequestAssert.SendAsync((c, ct) => c.LicenseMessages.GetAsync("2541354125", ct)))
			.ShouldBe(HttpMethod.Get, "/services/licenser/messages/2541354125");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.LicenseMessages.ListAsync(null, ct), MessagesJson);

		var message = feed.Entries.Should().ContainSingle().Subject.Content!;
		message.Category.Should().Be("pool_over_quota");
		message.CreateTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791465098));
		message.Description.Should().Be("Daily indexing volume limit exceeded today.");
		message.PeerId.Should().Be("00000000-0000-0000-0000-000000000001");
		message.PoolId.Should().Be("auto_generated_pool_enterprise");
		message.Severity.Should().Be("WARN");
		message.StackId.Should().Be("enterprise");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.LicenseMessages.GetAsync("missing", ct));
}
