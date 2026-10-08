using Splunk.Api.Models;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class AlertActionsTests
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetAndMapsTheAction()
	{
		var stub = TestClient.Stub(SearchRequestAssert.Feed("webhook", SavedSearchJson.AlertActionContent, "nobody"));
		using var client = TestClient.Create(stub);

		var feed = await client.AlertActions.ListAsync(new ListOptions { Search = "is_custom=1" }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/alerts/alert_actions", "?search=is_custom%3D1&output_mode=json");
		var action = feed.Entries.Should().ContainSingle().Subject.Content!;
		action.Label.Should().Be("Webhook");
		action.Description.Should().Be("Generic HTTP POST to a specified URL");
		action.Command.Should().StartWith("sendalert");
		action.IconPath.Should().Be("webhook.png");
		action.IsCustom.Should().BeTrue();
		action.MaxResults.Should().Be(10000);
		action.MaxTime.Should().Be("5m");
		action.TrackAlert.Should().BeFalse();
		action.Ttl.Should().Be("10p");
		action.PayloadFormat.Should().Be("json");
		action.AdditionalProperties.Should().ContainKey("param.user_agent");
	}

	[Fact]
	public async Task ListAsync_Error_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Unauthorized"}]}""", HttpStatusCode.Unauthorized));

		await SearchRequestAssert.FailsWith(() => client.AlertActions.ListAsync(null, Ct), HttpStatusCode.Unauthorized, "Unauthorized");
	}
}
