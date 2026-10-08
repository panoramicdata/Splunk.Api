using Splunk.Api.Models;
using System.Net;
using System.Text.Json;

namespace Splunk.Api.Test.Core;

/// <summary>The shared root models and small public types.</summary>
public class ModelTests
{
	[Fact]
	public void Namespace_WellKnownValues()
	{
		SplunkNamespace.All.Should().Be(new SplunkNamespace("-", "-"));
		SplunkNamespace.Shared("search").Should().Be(new SplunkNamespace("nobody", "search"));
		new SplunkNamespace("admin", "search").ToString().Should().Be("admin/search");
	}

	[Theory]
	[InlineData("-", "-", "servicesNS/-/-/")]
	[InlineData("a b", "x/y", "servicesNS/a%20b/x%2Fy/")]
	[InlineData("...", ".x", "servicesNS/.../.x/")]
	[InlineData("é", "%", "servicesNS/%C3%A9/%25/")]
	public void Namespace_PathPrefixEscapesEachPart(string owner, string app, string expected)
		=> new SplunkNamespace(owner, app).PathPrefix.Should().Be(expected);

	[Theory]
	[InlineData(null, "app", "Owner")]
	[InlineData("", "app", "Owner")]
	[InlineData(".", "app", "Owner")]
	[InlineData("..", "app", "Owner")]
	[InlineData("admin", null, "App")]
	[InlineData("admin", " ", "App")]
	[InlineData("admin", "..", "App")]
	public void Namespace_InvalidParts_AreRejected(string? owner, string? app, string parameter)
	{
		var act = () => new SplunkNamespace(owner!, app!).PathPrefix;

		act.Should().Throw<ArgumentException>().Which.ParamName.Should().Be(parameter);
	}

	[Fact]
	public void ReadOnlyException_CarriesMethodAndPath()
	{
		var exception = new SplunkReadOnlyException("DELETE", "https://splunk.test/services/x");

		exception.Should().BeAssignableTo<InvalidOperationException>();
		exception.Method.Should().Be("DELETE");
		exception.Path.Should().Be("https://splunk.test/services/x");
		exception.Message.Should().Be("The Splunk client is read-only and refused DELETE https://splunk.test/services/x.");
	}

	[Fact]
	public void ApiException_CarriesStatusAndMessages()
	{
		SplunkMessage[] messages = [new() { Type = "ERROR", Text = "bad" }];

		var exception = new SplunkApiException(HttpStatusCode.BadRequest, messages, "bad");

		exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		exception.Messages.Should().BeSameAs(messages);
		exception.Message.Should().Be("bad");
	}

	[Fact]
	public void Message_DefaultsAndToString()
	{
		var message = new SplunkMessage();

		message.Type.Should().BeEmpty();
		message.Text.Should().BeEmpty();
		new SplunkMessage { Type = "WARN", Text = "w" }.ToString().Should().Be("WARN: w");
	}

	private const string FeedJson = """
		{
			"title": "savedsearch", "origin": "https://splunk.test:8089/services/saved/searches",
			"updated": "2026-10-08T14:18:12+01:00", "generator": {"build": "b", "version": "10.6.0"},
			"links": {"create": "/services/saved/searches/_new"},
			"entry": [{
				"name": "Errors", "id": "https://splunk.test:8089/servicesNS/nobody/search/saved/searches/Errors",
				"updated": "2026-10-08T14:18:12+01:00", "author": "nobody", "links": {"edit": "/e"},
				"acl": {"app": "search", "owner": "nobody", "sharing": "app", "perms": {"read": ["*"], "write": ["admin"]},
					"can_change_perms": "1", "can_list": true, "can_share_app": true, "can_share_global": 1, "can_share_user": "0",
					"can_write": true, "removable": false, "modifiable": true, "extra": 5},
				"content": {"eai:acl": null, "eai:appName": "search", "eai:userName": "admin", "disabled": "0", "search": "error"}
			}],
			"paging": {"total": "1", "perPage": 30, "offset": 0},
			"messages": [{"type": "INFO", "text": "ok"}]
		}
		""";

	[Fact]
	public void Feed_MapsTheEnvelope()
	{
		var feed = JsonSerializer.Deserialize<SplunkFeed<SplunkDynamicContent>>(FeedJson, SplunkJson.Options)!;

		feed.Title.Should().Be("savedsearch");
		feed.Origin.Should().Be("https://splunk.test:8089/services/saved/searches");
		feed.Updated.Should().Be(new DateTimeOffset(2026, 10, 8, 14, 18, 12, TimeSpan.FromHours(1)));
		feed.Generator!.Build.Should().Be("b");
		feed.Links.Should().ContainKey("create");
		feed.Paging!.Total.Should().Be(1);
		feed.Paging.PerPage.Should().Be(30);
		feed.Paging.Offset.Should().Be(0);
		feed.Messages.Should().ContainSingle().Which.Text.Should().Be("ok");
		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("Errors");
		entry.Id.Should().EndWith("/Errors");
		entry.Updated.Should().NotBeNull();
		entry.Author.Should().Be("nobody");
		entry.Links["edit"].Should().Be("/e");
		var acl = entry.Acl!;
		(acl.App, acl.Owner, acl.Sharing).Should().Be(("search", "nobody", "app"));
		acl.Permissions!.Read.Should().Equal("*");
		acl.Permissions.Write.Should().Equal("admin");
		(acl.CanChangePermissions, acl.CanList, acl.CanShareApp, acl.CanShareGlobal, acl.CanShareUser).Should().Be((true, true, true, true, false));
		(acl.CanWrite, acl.Removable, acl.Modifiable).Should().Be((true, false, true));
		acl.AdditionalProperties["extra"].GetInt32().Should().Be(5);
		var content = entry.Content!;
		content.EaiAcl.Should().BeNull();
		(content.EaiAppName, content.EaiUserName, content.Disabled).Should().Be(("search", "admin", false));
		content.AdditionalProperties["search"].GetString().Should().Be("error");
	}

	[Fact]
	public void Defaults_AreEmptyNotNull()
	{
		var feed = new SplunkFeed<SplunkDynamicContent>();
		var entry = new SplunkEntry<SplunkDynamicContent>();

		feed.Entries.Should().BeEmpty();
		feed.Messages.Should().BeEmpty();
		feed.Links.Should().BeEmpty();
		entry.Name.Should().BeEmpty();
		entry.Links.Should().BeEmpty();
		new SplunkAcl().AdditionalProperties.Should().BeEmpty();
		new SplunkPermissions().Read.Should().BeEmpty();
		new SplunkPermissions().Write.Should().BeEmpty();
		new SplunkDynamicContent().AdditionalProperties.Should().BeEmpty();
		new MoveRequest { App = "a", User = "u" }.AdditionalParameters.Should().BeEmpty();
	}

	[Fact]
	public void JsonBody_ExposesValueAndDeclaredType()
	{
		IJsonBody body = new JsonBody<IReadOnlyList<int>>([1]);

		body.Value.Should().BeEquivalentTo(new List<int> { 1 });
		body.ValueType.Should().Be<IReadOnlyList<int>>();
	}
}
