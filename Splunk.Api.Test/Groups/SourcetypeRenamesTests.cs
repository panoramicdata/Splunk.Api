using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class SourcetypeRenamesTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (POST data/props/sourcetype-rename), names changed.
	private static readonly string RenameJson = KnowledgeTestKit.Feed("data/props/sourcetype-rename", "hardware", """
		{
			"attribute": "rename",
			"eai:acl": null,
			"stanza": "hardware",
			"type": "rename",
			"value": "hw"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToSourcetypeRename()
		=> (await KnowledgeTestKit.SendAsync(c => c.SourcetypeRenames.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/props/sourcetype-rename");

	[Fact]
	public async Task CreateAsync_PostsNameAndValue()
		=> (await KnowledgeTestKit.SendAsync(c => c.SourcetypeRenames.CreateAsync(new SourcetypeRenameCreateRequest { Name = "hardware", Value = "hw" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/data/props/sourcetype-rename", body: "name=hardware&value=hw");

	[Fact]
	public async Task GetAsync_SendsGetToTheEntry()
		=> (await KnowledgeTestKit.SendAsync(c => c.SourcetypeRenames.GetAsync("hardware", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/props/sourcetype-rename/hardware");

	[Fact]
	public async Task UpdateAsync_PostsTheValue()
		=> (await KnowledgeTestKit.SendAsync(c => c.SourcetypeRenames.UpdateAsync("hardware", new SourcetypeRenameUpdateRequest { Value = "hrdwr" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/data/props/sourcetype-rename/hardware", body: "value=hrdwr");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.SourcetypeRenames.DeleteAsync("hardware", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/data/props/sourcetype-rename/hardware");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.SourcetypeRenames.GetAsync("hardware", TestContext.Current.CancellationToken), RenameJson);

		entry.ShouldBeTheCapturedEntry("hardware");
		entry.Content!.Attribute.Should().Be("rename");
		entry.Content.Stanza.Should().Be("hardware");
		entry.Content.Type.Should().Be("rename");
		entry.Content.Value.Should().Be("hw");
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.SourcetypeRenames.GetAsync("missing", TestContext.Current.CancellationToken));
}
