using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class IndexingPreviewsTests
{
	private const string Path = "/services/indexing/preview";

	// Captured from Splunk 10.6.0.5, trimmed to a few inherited settings.
	private static readonly string PreviewJson = InputsTestKit.Feed("1791468575.2", """
		{
			"explicit": { "PREFERRED_SOURCETYPE": { "value": "splunk_version", "stanza": "" }, "SHOULD_LINEMERGE": { "value": "false", "stanza": "" } },
			"inherited": { "CHARSET": { "value": "UTF-8", "stanza": "default" }, "TRUNCATE": { "value": "10000", "stanza": "default" } }
		}
		""");

	// Captured from Splunk 10.6.0.5: the reply to creating a preview job.
	private const string CreatedJson = """{"messages":[{"type":"INFO","text":"1791468575.2"}]}""";

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IndexingPreviews.ListAsync(ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IndexingPreviews.CreateAsync(
			new IndexingPreviewCreateRequest
			{
				InputPath = "/opt/splunk/etc/splunk.version",
				AdditionalParameters = { ["props.SHOULD_LINEMERGE"] = "false" }
			},
			ct)))
			.ShouldBePost(Path, "input.path=%2Fopt%2Fsplunk%2Fetc%2Fsplunk.version&props.SHOULD_LINEMERGE=false");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IndexingPreviews.GetAsync("1791468575.2", ct))).ShouldBeGet(Path + "/1791468575.2");

	[Fact]
	public async Task CreateAsync_ReturnsTheJobIdAsAMessage()
	{
		var feed = await InputsTestKit.MapAsync(
			(c, ct) => c.IndexingPreviews.CreateAsync(new IndexingPreviewCreateRequest { InputPath = "/opt/splunk/etc/splunk.version" }, ct),
			CreatedJson);

		feed.Entries.Should().BeEmpty();
		feed.Messages.Should().ContainSingle().Which.Text.Should().Be("1791468575.2");
	}

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var preview = (await InputsTestKit.MapEntryAsync((c, ct) => c.IndexingPreviews.GetAsync("1791468575.2", ct), PreviewJson)).Content!;

		preview.Explicit.Should().HaveCount(2);
		preview.Explicit["SHOULD_LINEMERGE"].Value.Should().Be("false");
		preview.Explicit["SHOULD_LINEMERGE"].Stanza.Should().BeEmpty();
		preview.Inherited["CHARSET"].Value.Should().Be("UTF-8");
		preview.Inherited["CHARSET"].Stanza.Should().Be("default");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.IndexingPreviews.GetAsync("nope", ct));
}
