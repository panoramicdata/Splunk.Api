using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class FieldTransformsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET data/transforms/extractions/{name}), names changed; DELIMS and FIELDS added.
	private static readonly string TransformJson = KnowledgeTestKit.Feed("data/transforms/extractions", "kv_pairs", """
		{
			"CAN_OPTIMIZE": true,
			"CLEAN_KEYS": false,
			"DEFAULT_VALUE": "",
			"DELIMS": "\"|\", \"=\"",
			"DEPTH_LIMIT": "1000",
			"DEST_KEY": "",
			"FIELDS": "a,b",
			"FORMAT": "$1::$2",
			"KEEP_EMPTY_VALS": false,
			"LOOKAHEAD": "4096",
			"MATCH_LIMIT": "100000",
			"MV_ADD": true,
			"REGEX": "(?<k>[a-z]+)=(?<v>[a-z]+)",
			"SOURCE_KEY": "_raw",
			"WRITE_META": "False",
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "search",
			"eai:userName": "nobody"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToTransformsExtractions()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldTransforms.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/transforms/extractions");

	[Fact]
	public async Task CreateAsync_PostsTheUppercaseSettings()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldTransforms.CreateAsync(
				new FieldTransformCreateRequest
				{
					Name = "kv_pairs",
					Regex = "(?<k>[a-z]+)=(?<v>[a-z]+)",
					SourceKey = "_raw",
					Format = "$1::$2",
					Delimiters = "\"|\", \"=\"",
					Fields = "a,b",
					CanOptimize = true,
					CleanKeys = false,
					KeepEmptyValues = true,
					MultivalueAdd = true,
					Disabled = false
				},
				TestContext.Current.CancellationToken)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/data/transforms/extractions",
				body: "name=kv_pairs&REGEX=%28%3F%3Ck%3E%5Ba-z%5D%2B%29%3D%28%3F%3Cv%3E%5Ba-z%5D%2B%29&SOURCE_KEY=_raw&FORMAT=%241%3A%3A%242&DELIMS=%22%7C%22%2C+%22%3D%22&FIELDS=a%2Cb&CAN_OPTIMIZE=true&CLEAN_KEYS=false&KEEP_EMPTY_VALS=true&MV_ADD=true&disabled=false");

	[Fact]
	public async Task GetAsync_SendsGetToTheEntry()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldTransforms.GetAsync("kv_pairs", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/transforms/extractions/kv_pairs");

	[Fact]
	public async Task UpdateAsync_PostsOnlyTheSettingsGiven()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldTransforms.UpdateAsync("kv_pairs", new FieldTransformUpdateRequest { Regex = "x=(?<x>.)" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/data/transforms/extractions/kv_pairs", body: "REGEX=x%3D%28%3F%3Cx%3E.%29");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.FieldTransforms.DeleteAsync("kv_pairs", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/data/transforms/extractions/kv_pairs");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.FieldTransforms.GetAsync("kv_pairs", TestContext.Current.CancellationToken), TransformJson);

		entry.ShouldBeTheCapturedEntry("kv_pairs");
		var transform = entry.Content!;
		transform.Regex.Should().Be("(?<k>[a-z]+)=(?<v>[a-z]+)");
		transform.SourceKey.Should().Be("_raw");
		transform.Format.Should().Be("$1::$2");
		transform.Delimiters.Should().Be("\"|\", \"=\"");
		transform.Fields.Should().Be("a,b");
		transform.DestinationKey.Should().BeEmpty();
		transform.DefaultValue.Should().BeEmpty();
		transform.CanOptimize.Should().BeTrue();
		transform.CleanKeys.Should().BeFalse();
		transform.KeepEmptyValues.Should().BeFalse();
		transform.MultivalueAdd.Should().BeTrue();
		transform.WriteMeta.Should().BeFalse();
		transform.Lookahead.Should().Be(4096);
		transform.DepthLimit.Should().Be(1000);
		transform.MatchLimit.Should().Be(100000);
		transform.Disabled.Should().BeFalse();
		transform.EaiAppName.Should().Be("search");
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.FieldTransforms.GetAsync("missing", TestContext.Current.CancellationToken));
}
