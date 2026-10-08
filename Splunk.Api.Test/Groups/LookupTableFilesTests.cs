using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LookupTableFilesTests
{
	private const string StagedPath = "/opt/splunk/var/run/splunk/lookup_tmp/assets.csv";

	// Captured from Splunk Enterprise 10.6.0.5 (GET data/lookup-table-files/{name}).
	private static readonly string FileJson = KnowledgeTestKit.Feed("data/lookup-table-files", "action_types.csv", """
		{
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "audit_trail",
			"eai:data": "/opt/splunk/etc/apps/audit_trail/lookups/action_types.csv",
			"eai:userName": "nobody",
			"fields_array": ["DEST_ACTION", "ORIG_ACTION"],
			"lastModifiedTime": 1790586550,
			"size": 173
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToLookupTableFiles()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupTableFiles.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/lookup-table-files");

	[Fact]
	public async Task CreateAsync_PostsNameAndStagedPath_InTheNamespace()
		=> (await KnowledgeTestKit.SendInSearchAppAsync(c => c.LookupTableFiles.CreateAsync(new LookupTableFileCreateRequest { Name = "assets.csv", StagedPath = StagedPath }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/servicesNS/nobody/search/data/lookup-table-files", body: "name=assets.csv&eai%3Adata=%2Fopt%2Fsplunk%2Fvar%2Frun%2Fsplunk%2Flookup_tmp%2Fassets.csv");

	[Fact]
	public async Task GetAsync_SendsGetToTheFile()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupTableFiles.GetAsync("assets.csv", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/lookup-table-files/assets.csv");

	[Fact]
	public async Task UpdateAsync_PostsTheStagedPath()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupTableFiles.UpdateAsync("assets.csv", new LookupTableFileUpdateRequest { StagedPath = StagedPath }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/data/lookup-table-files/assets.csv", body: "eai%3Adata=%2Fopt%2Fsplunk%2Fvar%2Frun%2Fsplunk%2Flookup_tmp%2Fassets.csv");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupTableFiles.DeleteAsync("assets.csv", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/data/lookup-table-files/assets.csv");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.LookupTableFiles.GetAsync("action_types.csv", TestContext.Current.CancellationToken), FileJson);

		entry.ShouldBeTheCapturedEntry("action_types.csv");
		var file = entry.Content!;
		file.Path.Should().Be("/opt/splunk/etc/apps/audit_trail/lookups/action_types.csv");
		file.Fields.Should().Equal("DEST_ACTION", "ORIG_ACTION");
		file.Size.Should().Be(173);
		file.LastModifiedTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1790586550));
		file.Disabled.Should().BeFalse();
		file.EaiAppName.Should().Be("audit_trail");
		file.EaiUserName.Should().Be("nobody");
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.LookupTableFiles.GetAsync("missing", TestContext.Current.CancellationToken));
}
