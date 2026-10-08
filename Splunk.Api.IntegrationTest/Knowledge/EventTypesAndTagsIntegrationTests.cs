using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>Round trips event types, search-time tags and field value tags in the search app.</summary>
[Collection(SplunkTestGroup.Name)]
public sealed class EventTypesAndTagsIntegrationTests(SplunkFixture fixture) : IDisposable
{
	private readonly SplunkClient _app = fixture.Client.InNamespace("nobody", "search");

	private static CancellationToken Token => TestContext.Current.CancellationToken;

	public void Dispose() => _app.Dispose();

	[Fact]
	public async Task EventType_CreateGetUpdateDelete()
	{
		var name = SplunkFixture.UniqueName("et");
		try
		{
			await _app.EventTypes.CreateAsync(
				new EventTypeCreateRequest { Name = name, Search = "index=_internal sourcetype=splunkd", Description = "it", Priority = 3 },
				Token);

			await _app.EventTypes.UpdateAsync(name, new EventTypeUpdateRequest { Search = "index=_internal sourcetype=splunkd log_level=ERROR", Disabled = true }, Token);

			var eventType = (await _app.EventTypes.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			eventType.Search.Should().Be("index=_internal sourcetype=splunkd log_level=ERROR");
			eventType.Description.Should().Be("it", "settings left out of an update keep their values");
			eventType.Priority.Should().Be(3);
			eventType.Disabled.Should().BeTrue();
			(await _app.EventTypes.ListAsync(new ListOptions { Search = name }, Token)).Entries.Should().ContainSingle();
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => _app.EventTypes.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => _app.EventTypes.GetAsync(name, Token));
	}

	[Fact]
	public async Task Tag_AddListRemoveDelete()
	{
		var tag = SplunkFixture.UniqueName("tag");
		var host = SplunkFixture.UniqueName("host");
		try
		{
			var added = await _app.SearchTags.UpdateAsync(tag, new TagUpdateRequest { Add = [$"host::{host}", "sourcetype::splunk_api_it_st"] }, Token);
			added.Messages.Should().ContainSingle().Which.Text.Should().Be("Processed adds/deletes for tag");

			(await _app.SearchTags.GetAsync(tag, Token)).Entries.Select(e => e.Name).Should().BeEquivalentTo($"host::{host}", "sourcetype::splunk_api_it_st");
			(await _app.SearchTags.ListAsync(Token)).Entries.Should().Contain(e => e.Name == tag);

			await _app.SearchTags.UpdateAsync(tag, new TagUpdateRequest { Delete = ["sourcetype::splunk_api_it_st"] }, Token);
			(await _app.SearchTags.GetAsync(tag, Token)).Entries.Should().ContainSingle().Which.Name.Should().Be($"host::{host}");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => _app.SearchTags.DeleteAsync(tag, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => _app.SearchTags.DeleteAsync(tag, Token));
	}

	[Fact]
	public async Task FieldTags_AddListRemove()
	{
		var tag = SplunkFixture.UniqueName("tag");
		var host = SplunkFixture.UniqueName("host");
		try
		{
			await _app.SearchFields.UpdateTagsAsync("host", new FieldTagsUpdateRequest { Value = host, Add = [tag] }, Token);

			(await _app.SearchFields.ListTagsAsync("host", Token)).Entries.Should().Contain(e => e.Name == $"{host}::{tag}");

			var removed = await _app.SearchFields.UpdateTagsAsync("host", new FieldTagsUpdateRequest { Value = host, Delete = [tag] }, Token);
			removed.Messages.Should().ContainSingle().Which.Text.Should().Contain("processed adds/deletes for field host");
			(await _app.SearchFields.ListTagsAsync("host", Token)).Entries.Should().NotContain(e => e.Name == $"{host}::{tag}");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => _app.SearchTags.DeleteAsync(tag, CancellationToken.None));
		}
	}

	[Fact]
	public async Task SearchFields_ListAndGet()
	{
		(await _app.SearchFields.ListAsync(Token)).Entries.Should().Contain(e => e.Name == "sourcetype");

		var field = (await _app.SearchFields.GetAsync("sourcetype", Token)).Entries.Should().ContainSingle().Subject;

		field.Content.Should().StartWith("PropertiesMap:").And.Contain("INDEXED");
	}
}
