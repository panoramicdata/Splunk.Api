using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LookupDefinitionsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET data/transforms/lookups?getsize=true), trimmed; temporal settings added.
	private static readonly string LookupJson = KnowledgeTestKit.Feed("data/transforms/lookups", "dmc_assets", """
		{
			"CAN_OPTIMIZE": true,
			"CLEAN_KEYS": true,
			"KEEP_EMPTY_VALS": false,
			"MV_ADD": false,
			"REGEX": "",
			"SOURCE_KEY": "_raw",
			"WRITE_META": "False",
			"case_sensitive_match": true,
			"collection": "",
			"default_match": "unknown",
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "splunk_monitoring_console",
			"eai:userName": "nobody",
			"external_cmd": "",
			"external_type": "",
			"fields_array": ["peerURI", "serverName", "host"],
			"fields_list": "peerURI,serverName,host",
			"filename": "assets.csv",
			"match_type": "WILDCARD(host)",
			"max_matches": 100,
			"max_offset_secs": "3600",
			"min_matches": 1,
			"min_offset_secs": "0",
			"replicate_delta": false,
			"size": 335,
			"time_field": "_time",
			"time_format": "%s",
			"type": "file"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetSize()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupDefinitions.ListAsync(new LookupDefinitionListOptions { GetSize = true, Count = 0 }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/transforms/lookups", "?getsize=true&count=0&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsEverySetting()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupDefinitions.CreateAsync(
				new LookupDefinitionCreateRequest
				{
					Name = "assets",
					FileName = "assets.csv",
					Collection = "assets_kv",
					ExternalCommand = "external_lookup.py host ip",
					ExternalType = "python",
					FieldsList = "host,ip",
					DefaultMatch = "unknown",
					MaxMatches = 5,
					MinMatches = 1,
					TimeField = "_time",
					TimeFormat = "%s",
					MaxOffsetSeconds = 3600,
					MinOffsetSeconds = 0,
					CaseSensitiveMatch = false,
					MatchType = "CIDR(ip)",
					ReplicateDelta = true,
					Disabled = false
				},
				TestContext.Current.CancellationToken)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/data/transforms/lookups",
				body: "name=assets&filename=assets.csv&collection=assets_kv&external_cmd=external_lookup.py+host+ip&external_type=python&fields_list=host%2Cip&default_match=unknown&max_matches=5&min_matches=1&time_field=_time&time_format=%25s&max_offset_secs=3600&min_offset_secs=0&case_sensitive_match=false&match_type=CIDR%28ip%29&replicate_delta=true&disabled=false");

	[Fact]
	public async Task GetAsync_SendsGetToTheEntry()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupDefinitions.GetAsync("assets", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/transforms/lookups/assets");

	[Fact]
	public async Task UpdateAsync_PostsTheSettings()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupDefinitions.UpdateAsync("assets", new LookupDefinitionUpdateRequest { FileName = "assets.csv", MaxMatches = 1 }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/data/transforms/lookups/assets", body: "filename=assets.csv&max_matches=1");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.LookupDefinitions.DeleteAsync("assets", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/data/transforms/lookups/assets");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.LookupDefinitions.GetAsync("dmc_assets", TestContext.Current.CancellationToken), LookupJson);

		entry.ShouldBeTheCapturedEntry("dmc_assets");
		var lookup = entry.Content!;
		lookup.Type.Should().Be("file");
		lookup.FileName.Should().Be("assets.csv");
		lookup.Collection.Should().BeEmpty();
		lookup.ExternalCommand.Should().BeEmpty();
		lookup.ExternalType.Should().BeEmpty();
		lookup.FieldsList.Should().Be("peerURI,serverName,host");
		lookup.Fields.Should().Equal("peerURI", "serverName", "host");
		lookup.DefaultMatch.Should().Be("unknown");
		lookup.MaxMatches.Should().Be(100);
		lookup.MinMatches.Should().Be(1);
		lookup.TimeField.Should().Be("_time");
		lookup.TimeFormat.Should().Be("%s");
		lookup.MaxOffsetSeconds.Should().Be(3600);
		lookup.MinOffsetSeconds.Should().Be(0);
		lookup.CaseSensitiveMatch.Should().BeTrue();
		lookup.MatchType.Should().Be("WILDCARD(host)");
		lookup.ReplicateDelta.Should().BeFalse();
		lookup.Size.Should().Be(335);
		lookup.SourceKey.Should().Be("_raw");
		lookup.EaiAppName.Should().Be("splunk_monitoring_console");
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.LookupDefinitions.GetAsync("missing", TestContext.Current.CancellationToken));
}
