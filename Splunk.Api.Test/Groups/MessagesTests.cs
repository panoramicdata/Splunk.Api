using Splunk.Api.Models;
using Splunk.Api.Models.Server;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class MessagesTests
{
	// Captured from Splunk 10.6.0.5 (POST messages, then GET); server name replaced.
	private const string MessageContent = """
		{
			"capabilities": ["admin_all_objects"],
			"eai:acl": null,
			"help": "",
			"message": "hello world",
			"message_alternate": "",
			"roles": ["admin", "power"],
			"server": "splunk01",
			"severity": "info",
			"my_message": "hello world",
			"timeCreated_epochSecs": 1791467748,
			"timeCreated_iso": "2026-10-08T13:55:48+00:00"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(
			c => c.Messages.ListAsync(new ListOptions { Count = 10 }, Calls.Token),
			HttpMethod.Get, "/services/messages", "?count=10&output_mode=json", null);

	[Fact]
	public async Task CreateAsync_PostsEveryField()
		=> await Calls.AssertAsync(
			c => c.Messages.CreateAsync(
				new MessageCreateRequest
				{
					Name = "my_message",
					Value = "hello world",
					Severity = MessageSeverity.Info,
					Capabilities = ["admin_all_objects"],
					Roles = ["admin", "power"]
				},
				Calls.Token),
			HttpMethod.Post, "/services/messages", Calls.JsonQuery,
			"name=my_message&value=hello+world&severity=info&capability=admin_all_objects&role=admin&role=power");

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Messages.GetAsync("my_message", Calls.Token), HttpMethod.Get, "/services/messages/my_message", Calls.JsonQuery, null);

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> await Calls.AssertAsync(c => c.Messages.DeleteAsync("my_message", Calls.Token), HttpMethod.Delete, "/services/messages/my_message", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.Messages.GetAsync("my_message", Calls.Token), Feed.Of("my_message", MessageContent));

		var message = feed.Entries.Should().ContainSingle().Subject.Content!;
		message.Message.Should().Be("hello world");
		message.MessageAlternate.Should().BeEmpty();
		message.Severity.Should().Be(MessageSeverity.Info);
		message.Server.Should().Be("splunk01");
		message.Help.Should().BeEmpty();
		message.Capabilities.Should().Equal("admin_all_objects");
		message.Roles.Should().Equal("admin", "power");
		message.TimeCreated.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467748));
		message.TimeCreatedIso.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 55, 48, TimeSpan.Zero));
		message.AdditionalProperties["my_message"].GetString().Should().Be("hello world");
	}

	[Fact]
	public async Task DeleteAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.Messages.DeleteAsync("missing", Calls.Token), HttpStatusCode.NotFound);
}
