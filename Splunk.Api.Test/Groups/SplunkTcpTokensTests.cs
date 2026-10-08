using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class SplunkTcpTokensTests
{
	private const string Path = "/services/data/inputs/tcp/splunktcptoken";

	// Captured from Splunk 10.6.0.5 after creating a receiver token; the token value replaced.
	private static readonly string TokenJson = InputsTestKit.Feed("splunktcptoken://forwarders", """
		{ "_rcvbuf": 1572864, "disabled": false, "eai:acl": null, "host": "$decideOnStartup", "index": "default", "token": "00000000-0000-0000-0000-000000000002" }
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SplunkTcpTokens.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SplunkTcpTokens.CreateAsync(
			new SplunkTcpTokenCreateRequest { Name = "forwarders", Token = "00000000-0000-0000-0000-000000000002" }, ct)))
			.ShouldBePost(Path, "name=forwarders&token=00000000-0000-0000-0000-000000000002");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SplunkTcpTokens.GetAsync("forwarders", ct))).ShouldBeGet(Path + "/forwarders");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SplunkTcpTokens.UpdateAsync(
			"forwarders", new SplunkTcpTokenUpdateRequest { Token = "00000000-0000-0000-0000-000000000003" }, ct)))
			.ShouldBePost(Path + "/forwarders", "token=00000000-0000-0000-0000-000000000003");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SplunkTcpTokens.DeleteAsync("forwarders", ct))).ShouldBeDelete(Path + "/forwarders");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.SplunkTcpTokens.GetAsync("forwarders", ct), TokenJson);

		entry.Name.Should().Be("splunktcptoken://forwarders");
		entry.Content!.Token.Should().Be("00000000-0000-0000-0000-000000000002");
		entry.Content.Index.Should().Be("default");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.SplunkTcpTokens.GetAsync("nope", ct));
}
