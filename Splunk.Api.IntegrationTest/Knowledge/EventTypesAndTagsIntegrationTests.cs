using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>Round trips event types, search-time tags and field value tags in the search app.</summary>
[Collection(SplunkTestGroup.Name)]
public sealed class EventTypesAndTagsIntegrationTests(SplunkFixture fixture) : SearchAppIntegrationTests(fixture)
{
	[Fact]
	public async Task EventType_CreateGetUpdateDelete()
	{
		var name = SplunkFixture.UniqueName("et");
		try
		{
			await App.EventTypes.CreateAsync(
				new EventTypeCreateRequest { Name = name, Search = "index=_internal sourcetype=splunkd", Description = "it", Priority = 3 },
				Token);

			await App.EventTypes.UpdateAsync(name, new EventTypeUpdateRequest { Search = "index=_internal sourcetype=splunkd log_level=ERROR", Disabled = true }, Token);

			var eventType = (await App.EventTypes.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			eventType.Search.Should().Be("index=_internal sourcetype=splunkd log_level=ERROR");
			eventType.Description.Should().Be("it", "settings left out of an update keep their values");
			eventType.Priority.Should().Be(3);
			eventType.Disabled.Should().BeTrue();
			(await App.EventTypes.ListAsync(new ListOptions { Search = name }, Token)).Entries.Should().ContainSingle();
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.EventTypes.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.EventTypes.GetAsync(name, Token));
	}

	[Fact]
	public async Task Tag_AddListRemoveDelete()
	{
		var tag = SplunkFixture.UniqueName("tag");
		var host = SplunkFixture.UniqueName("host");
		try
		{
			var added = await App.SearchTags.UpdateAsync(tag, new TagUpdateRequest { Add = [$"host::{host}", "sourcetype::splunk_api_it_st"] }, Token);
			added.Messages.Should().ContainSingle().Which.Text.Should().Be("Processed adds/deletes for tag");

			(await App.SearchTags.GetAsync(tag, Token)).Entries.Select(e => e.Name).Should().BeEquivalentTo($"host::{host}", "sourcetype::splunk_api_it_st");
			(await App.SearchTags.ListAsync(Token)).Entries.Should().Contain(e => e.Name == tag);

			await App.SearchTags.UpdateAsync(tag, new TagUpdateRequest { Delete = ["sourcetype::splunk_api_it_st"] }, Token);
			(await App.SearchTags.GetAsync(tag, Token)).Entries.Should().ContainSingle().Which.Name.Should().Be($"host::{host}");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.SearchTags.DeleteAsync(tag, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => App.SearchTags.DeleteAsync(tag, Token));
	}

	[Fact]
	public async Task FieldTags_AddListRemove()
	{
		var tag = SplunkFixture.UniqueName("tag");
		var host = SplunkFixture.UniqueName("host");
		try
		{
			await App.SearchFields.UpdateTagsAsync("host", new FieldTagsUpdateRequest { Value = host, Add = [tag] }, Token);

			(await App.SearchFields.ListTagsAsync("host", Token)).Entries.Should().Contain(e => e.Name == $"{host}::{tag}");

			var removed = await App.SearchFields.UpdateTagsAsync("host", new FieldTagsUpdateRequest { Value = host, Delete = [tag] }, Token);
			removed.Messages.Should().ContainSingle().Which.Text.Should().Contain("processed adds/deletes for field host");
			(await App.SearchFields.ListTagsAsync("host", Token)).Entries.Should().NotContain(e => e.Name == $"{host}::{tag}");
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => App.SearchTags.DeleteAsync(tag, CancellationToken.None));
		}
	}

	[Fact]
	public async Task SearchFields_ListAndGet()
	{
		(await App.SearchFields.ListAsync(Token)).Entries.Should().Contain(e => e.Name == "sourcetype");

		var field = (await App.SearchFields.GetAsync("sourcetype", Token)).Entries.Should().ContainSingle().Subject;

		field.Content.Should().StartWith("PropertiesMap:").And.Contain("INDEXED");
	}
}
