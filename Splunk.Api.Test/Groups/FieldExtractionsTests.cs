using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class FieldExtractionsTests
{
	private const string EntryName = "ftp_log : EXTRACT-port";
	private const string EntryPath = "/services/data/props/extractions/ftp_log%20%3A%20EXTRACT-port";

	// Captured from Splunk Enterprise 10.6.0.5 (POST data/props/extractions), names changed.
	private static readonly string ExtractionJson = KnowledgeTestKit.Feed("data/props/extractions", EntryName, """
		{
			"attribute": "EXTRACT-port",
			"eai:acl": null,
			"stanza": "ftp_log",
			"type": "Inline",
			"value": "port (?<port>[0-9]+)"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToExtractions()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldExtractions.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/props/extractions");

	[Fact]
	public async Task CreateAsync_PostsTheTypeByItsWireName()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldExtractions.CreateAsync(
				new FieldExtractionCreateRequest { Name = "port", Stanza = "ftp_log", Type = FieldExtractionType.Report, Value = "port-transform" },
				TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/data/props/extractions", body: "name=port&stanza=ftp_log&type=REPORT&value=port-transform");

	[Fact]
	public async Task GetAsync_SendsGetToTheEntry()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldExtractions.GetAsync(EntryName, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, EntryPath);

	[Fact]
	public async Task UpdateAsync_PostsTheValue()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldExtractions.UpdateAsync(EntryName, new FieldExtractionUpdateRequest { Value = "port=(?<port>[0-9]+)" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, EntryPath, body: "value=port%3D%28%3F%3Cport%3E%5B0-9%5D%2B%29");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldExtractions.DeleteAsync(EntryName, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, EntryPath);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.FieldExtractions.GetAsync(EntryName, TestContext.Current.CancellationToken), ExtractionJson);

		entry.ShouldBeTheCapturedEntry(EntryName);
		var extraction = entry.Content!;
		extraction.Attribute.Should().Be("EXTRACT-port");
		extraction.Stanza.Should().Be("ftp_log");
		extraction.Type.Should().Be("Inline");
		extraction.Value.Should().Be("port (?<port>[0-9]+)");
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.FieldExtractions.GetAsync("missing", TestContext.Current.CancellationToken));
}
