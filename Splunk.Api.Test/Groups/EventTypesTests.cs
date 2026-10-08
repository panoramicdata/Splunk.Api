using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class EventTypesTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET saved/eventtypes), audit_trail's cim:audit_account.
	private static readonly string EventTypeJson = KnowledgeTestKit.Feed("saved/eventtypes", "cim:audit_account", """
		{
			"color": "none",
			"description": "",
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "audit_trail",
			"eai:userName": "nobody",
			"priority": 1,
			"search": "index=_audit sourcetype=audittrailv2* category=system data.type=account",
			"tags": ["account", "change"]
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToEventtypes()
		=> (await KnowledgeTestKit.SendAsync(c => c.EventTypes.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/saved/eventtypes");

	[Fact]
	public async Task CreateAsync_PostsEverySetting()
		=> (await KnowledgeTestKit.SendAsync(c => c.EventTypes.CreateAsync(
				new EventTypeCreateRequest { Name = "client-errors", Search = "status>=400", Description = "Client errors", Priority = 3, Color = "et_red", Disabled = false },
				TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/saved/eventtypes", body: "name=client-errors&search=status%3E%3D400&description=Client+errors&priority=3&color=et_red&disabled=false");

	[Fact]
	public async Task GetAsync_SendsGetToTheEscapedName()
		=> (await KnowledgeTestKit.SendAsync(c => c.EventTypes.GetAsync("cim:audit_account", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/saved/eventtypes/cim%3Aaudit_account");

	[Fact]
	public async Task UpdateAsync_PostsTheSearchAgain()
		=> (await KnowledgeTestKit.SendAsync(c => c.EventTypes.UpdateAsync("client-errors", new EventTypeUpdateRequest { Search = "status>=400", Disabled = true }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/saved/eventtypes/client-errors", body: "search=status%3E%3D400&disabled=true");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.EventTypes.DeleteAsync("client-errors", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/saved/eventtypes/client-errors");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.EventTypes.GetAsync("cim:audit_account", TestContext.Current.CancellationToken), EventTypeJson);

		entry.ShouldBeTheCapturedEntry("cim:audit_account");
		var eventType = entry.Content!;
		eventType.Search.Should().Be("index=_audit sourcetype=audittrailv2* category=system data.type=account");
		eventType.Description.Should().BeEmpty();
		eventType.Priority.Should().Be(1);
		eventType.Color.Should().Be("none");
		eventType.Tags.Should().Equal("account", "change");
		eventType.Disabled.Should().BeFalse();
		eventType.EaiAppName.Should().Be("audit_trail");
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.EventTypes.GetAsync("missing", TestContext.Current.CancellationToken));
}
