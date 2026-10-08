namespace Splunk.Api.Test.Core;

/// <summary>Pins how error bodies other than Splunk's <c>{"messages":[...]}</c> are turned into messages.</summary>
public class ErrorShapeTests
{
	[Fact]
	public void OpenApiErrorBody_BecomesOneErrorMessage()
	{
		var messages = SplunkErrorMapper.ParseMessages("""{"code":"not_found","message":"Module not found.","details":[{"name":"apps.search.x"}]}""");

		var message = messages.Should().ContainSingle().Subject;
		message.Type.Should().Be("ERROR");
		message.Text.Should().Be("Module not found.");
	}

	[Theory]
	[InlineData("""{"code":"500"}""")]
	[InlineData("""{"code":"500","message":""}""")]
	[InlineData("""{"code":"500","message":3}""")]
	public void JsonBodyWithoutMessages_GivesNoMessages(string body)
		=> SplunkErrorMapper.ParseMessages(body).Should().BeEmpty();
}
