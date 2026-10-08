using Splunk.Api.Handlers;

namespace Splunk.Api.Test.Core;

/// <summary>
/// Pins the two ways the Topology REST API (<c>stack-explainer/...</c>, a sidecar) differs from splunkd: it rejects any
/// <c>output_mode</c>, and it reports errors as <c>{"error":"..."}</c>.
/// </summary>
public class StackExplainerConventionsTests
{
	[Theory]
	[InlineData("https://splunk.test:8089/services/stack-explainer/v1/topology")]
	[InlineData("https://splunk.test:8089/services/stack-explainer/v1/topology?include_unmanaged_actors")]
	[InlineData("https://splunk.test:8089/servicesNS/nobody/search/stack-explainer/v1/node-identity")]
	public void OutputMode_IsNotAddedToStackExplainerRequests(string url)
		=> OutputModeHandler.WithJsonOutputMode(new Uri(url)).Should().Be(new Uri(url));

	[Fact]
	public void OutputMode_IsStillAddedElsewhere()
		=> OutputModeHandler.WithJsonOutputMode(new Uri("https://splunk.test:8089/services/cluster/config")).Query
			.Should().Be("?output_mode=json");

	[Fact]
	public void ErrorBody_IsReadAsOneErrorMessage()
	{
		var messages = SplunkErrorMapper.ParseMessages("""{"error":"unrecognized query parameter: output_mode"}""");

		var message = messages.Should().ContainSingle().Subject;
		message.Type.Should().Be("ERROR");
		message.Text.Should().Be("unrecognized query parameter: output_mode");
	}

	[Theory]
	[InlineData("""{"error":""}""")]
	[InlineData("""{"error":42}""")]
	[InlineData("""{"other":"x"}""")]
	public void ErrorBody_WithoutErrorText_HasNoMessages(string body)
		=> SplunkErrorMapper.ParseMessages(body).Should().BeEmpty();
}
