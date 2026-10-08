using System.Net;

namespace Splunk.Api.IntegrationTest;

/// <summary>Assertions on the errors a live Splunk answers with.</summary>
internal static class SplunkAssert
{
	/// <summary>Asserts that <paramref name="call"/> raises <see cref="SplunkApiException"/> with the status and Splunk's message.</summary>
	public static async Task FailsAsync(Func<Task> call, HttpStatusCode status, string message)
	{
		var error = (await call.Should().ThrowAsync<SplunkApiException>()).Which;
		error.StatusCode.Should().Be(status);
		error.Message.Should().Be(message);
	}
}
