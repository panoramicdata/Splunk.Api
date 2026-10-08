using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class CalculatedFieldsTests
{
	private const string EntryName = "access_combined : EVAL-response_seconds";
	private const string EscapedName = "access_combined%20%3A%20EVAL-response_seconds";

	// Captured from Splunk Enterprise 10.6.0.5 (POST data/props/calcfields), names changed.
	private static readonly string CalculatedFieldJson = KnowledgeTestKit.Feed("data/props/calcfields", EntryName, """
		{
			"attribute": "EVAL-response_seconds",
			"eai:acl": null,
			"eai:appName": "search",
			"eai:userName": "admin",
			"field.name": "response_seconds",
			"stanza": "access_combined",
			"type": "EVAL",
			"value": "response_time/1000"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToCalcfields()
		=> (await KnowledgeTestKit.SendAsync(c => c.CalculatedFields.ListAsync(new ListOptions { Count = 5 }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/props/calcfields", "?count=5&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsNameStanzaAndValue()
		=> (await KnowledgeTestKit.SendAsync(c => c.CalculatedFields.CreateAsync(
				new CalculatedFieldCreateRequest { Name = "response_seconds", Stanza = "access_combined", Value = "response_time/1000" },
				TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/data/props/calcfields", body: "name=response_seconds&stanza=access_combined&value=response_time%2F1000");

	[Fact]
	public async Task GetAsync_SendsGetToTheEscapedEntryName()
		=> (await KnowledgeTestKit.SendAsync(c => c.CalculatedFields.GetAsync(EntryName, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, $"/services/data/props/calcfields/{EscapedName}");

	[Fact]
	public async Task UpdateAsync_PostsTheValue()
		=> (await KnowledgeTestKit.SendAsync(c => c.CalculatedFields.UpdateAsync(EntryName, new CalculatedFieldUpdateRequest { Value = "len(_raw)" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, $"/services/data/props/calcfields/{EscapedName}", body: "value=len%28_raw%29");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.CalculatedFields.DeleteAsync(EntryName, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, $"/services/data/props/calcfields/{EscapedName}");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.CalculatedFields.GetAsync(EntryName, TestContext.Current.CancellationToken), CalculatedFieldJson);

		entry.ShouldBeTheCapturedEntry(EntryName);
		var field = entry.Content!;
		field.Attribute.Should().Be("EVAL-response_seconds");
		field.FieldName.Should().Be("response_seconds");
		field.Stanza.Should().Be("access_combined");
		field.Type.Should().Be("EVAL");
		field.Value.Should().Be("response_time/1000");
		field.EaiAppName.Should().Be("search");
		field.EaiUserName.Should().Be("admin");
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.CalculatedFields.GetAsync("missing", TestContext.Current.CancellationToken));
}
