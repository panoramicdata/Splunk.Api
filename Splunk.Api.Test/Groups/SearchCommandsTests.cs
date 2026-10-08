using Splunk.Api.Models;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SearchCommandsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (data/commands/bucketdir), trimmed.
	private static readonly string CommandFeed = SearchRequestAssert.Feed("bucketdir", """
		{"changes_colorder":true,"disabled":false,"eai:acl":null,"eai:appName":"search","eai:userName":"admin","enableheader":true,"filename":"bucketdir.py","generates_timeorder":false,"generating":false,"is_risky":"false","maxinputs":50000,"outputheader":false,"passauth":false,"python.required":"latest","required_fields":"*","requires_preop":false,"retainsevents":false,"streaming":false,"supports_rawargs":true,"type":"python"}
		""", "nobody");

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetWithPaging()
	{
		var stub = TestClient.Stub(CommandFeed);
		using var client = TestClient.Create(stub);

		await client.SearchCommands.ListAsync(new ListOptions { Count = 1, Offset = 2 }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/data/commands", "?count=1&offset=2&output_mode=json");
	}

	[Fact]
	public async Task GetAsync_SendsGetToTheCommand()
	{
		var stub = TestClient.Stub(CommandFeed);
		using var client = TestClient.Create(stub);

		await client.SearchCommands.GetAsync("bucketdir", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/data/commands/bucketdir", "?output_mode=json");
	}

	[Fact]
	public async Task GetAsync_MapsTheCommand()
	{
		using var client = TestClient.Create(TestClient.Stub(CommandFeed));

		var command = (await client.SearchCommands.GetAsync("bucketdir", Ct)).Entries.Should().ContainSingle().Subject.Content!;

		command.Filename.Should().Be("bucketdir.py");
		command.Type.Should().Be("python");
		command.Streaming.Should().BeFalse();
		command.Generating.Should().BeFalse();
		command.GeneratesTimeOrder.Should().BeFalse();
		command.RetainsEvents.Should().BeFalse();
		command.ChangesColumnOrder.Should().BeTrue();
		command.RequiredFields.Should().Be("*");
		command.MaxInputs.Should().Be(50000);
		command.PassAuth.Should().BeFalse();
		command.IsRisky.Should().BeFalse();
		command.SupportsRawArgs.Should().BeTrue();
		command.PythonRequired.Should().Be("latest");
		command.Disabled.Should().BeFalse();
		command.EaiAppName.Should().Be("search");
		command.AdditionalProperties.Should().ContainKey("enableheader");
	}

	[Fact]
	public async Task GetAsync_Unknown_RaisesNotFound()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Could not find object id=eval"}]}""", HttpStatusCode.NotFound));

		await SearchRequestAssert.FailsWith(() => client.SearchCommands.GetAsync("eval", Ct), HttpStatusCode.NotFound, "Could not find object id=eval");
	}
}
