using Splunk.Api.Models;
using Splunk.Api.Models.Server;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class LoggersTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(
			c => c.Loggers.ListAsync(new ListOptions { Count = 3, Search = "Tcp" }, Calls.Token),
			HttpMethod.Get, "/services/server/logger", "?count=3&search=Tcp&output_mode=json", null);

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Loggers.GetAsync("TcpOutputProc", Calls.Token), HttpMethod.Get, "/services/server/logger/TcpOutputProc", Calls.JsonQuery, null);

	[Fact]
	public async Task UpdateAsync_PostsTheLevel()
		=> await Calls.AssertAsync(
			c => c.Loggers.UpdateAsync("TcpOutputProc", new LoggerUpdateRequest { Level = SplunkLogLevel.Debug }, Calls.Token),
			HttpMethod.Post, "/services/server/logger/TcpOutputProc", Calls.JsonQuery, "level=DEBUG");

	[Theory]
	[InlineData("DEBUG", SplunkLogLevel.Debug)]
	[InlineData("INFO", SplunkLogLevel.Info)]
	[InlineData("WARN", SplunkLogLevel.Warn)]
	[InlineData("ERROR", SplunkLogLevel.Error)]
	[InlineData("FATAL", SplunkLogLevel.Fatal)]
	[InlineData("CRIT", SplunkLogLevel.Critical)]
	[InlineData("NOISY", SplunkLogLevel.Unknown)]
	public async Task GetAsync_MapsTheLevel(string wire, SplunkLogLevel expected)
	{
		// Captured from Splunk 10.6.0.5: GET server/logger/TcpOutputProc.
		var feed = await Calls.MapAsync(
			c => c.Loggers.GetAsync("TcpOutputProc", Calls.Token),
			Feed.Of("TcpOutputProc", $$"""{"buffering":false,"eai:acl":null,"level":"{{wire}}"}"""));

		var logger = feed.Entries.Should().ContainSingle().Subject.Content!;
		logger.Level.Should().Be(expected);
		logger.Buffering.Should().BeFalse();
	}

	[Fact]
	public async Task UpdateAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(
			c => c.Loggers.UpdateAsync("nope", new LoggerUpdateRequest { Level = SplunkLogLevel.Info }, Calls.Token),
			HttpStatusCode.NotFound);
}
