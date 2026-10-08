using Splunk.Api.Handlers;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Core;

public partial class RetryHandlerTests
{
	/// <summary>A transport that fails the first <paramref name="failures"/> sends with <paramref name="error"/>, then answers 200.</summary>
	private static DelegateHandler FailingThenOk(int failures, HttpRequestError error)
	{
		var sends = 0;
		return new DelegateHandler((_, _) => Interlocked.Increment(ref sends) <= failures
			? throw new HttpRequestException(error, "connection failed")
			: Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
	}

	[Theory]
	[InlineData(HttpRequestError.ConnectionError)]
	[InlineData(HttpRequestError.SecureConnectionError)]
	[InlineData(HttpRequestError.NameResolutionError)]
	public async Task ConnectionFailure_IsRetried_ForAnyVerb_WithBackoff(HttpRequestError error)
	{
		var transport = FailingThenOk(2, error);
		using var harness = new Harness(inner: transport);

		using var response = await harness.SendAsync(HttpMethod.Post, new FormUrlEncodedContent([new("a", "1")]));

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		transport.Requests.Should().HaveCount(3);
		harness.Delays.Should().Equal(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
		harness.Logger.Messages.Should().Contain(m => m.Contains($"connection failed ({error})", StringComparison.Ordinal) && !m.Contains("secret", StringComparison.Ordinal));
	}

	[Fact]
	public async Task ConnectionFailure_IsRetried_WithoutALogger()
	{
		var transport = FailingThenOk(1, HttpRequestError.ConnectionError);
		using var harness = new Harness(o => o.Logger = null, transport);

		using var response = await harness.SendAsync(HttpMethod.Get);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		transport.Requests.Should().HaveCount(2);
	}

	[Fact]
	public async Task ConnectionFailure_BeyondMaxRetries_IsThrown()
	{
		var transport = FailingThenOk(10, HttpRequestError.SecureConnectionError);
		using var harness = new Harness(o => o.MaxRetries = 2, transport);

		var act = () => harness.SendAsync(HttpMethod.Get);

		(await act.Should().ThrowAsync<HttpRequestException>()).Which.HttpRequestError.Should().Be(HttpRequestError.SecureConnectionError);
		transport.Requests.Should().HaveCount(3);
	}

	[Fact]
	public async Task ConnectionFailure_WithStreamBody_IsNotRetried()
	{
		var transport = FailingThenOk(1, HttpRequestError.ConnectionError);
		using var harness = new Harness(inner: transport);

		var act = () => harness.SendAsync(HttpMethod.Post, new StreamContent(new MemoryStream([1, 2, 3])));

		await act.Should().ThrowAsync<HttpRequestException>();
		transport.Requests.Should().ContainSingle();
	}

	[Theory]
	[InlineData(HttpRequestError.ResponseEnded)]
	[InlineData(HttpRequestError.Unknown)]
	public async Task FailureAfterSending_IsNotRetried(HttpRequestError error)
	{
		var transport = FailingThenOk(1, error);
		using var harness = new Harness(inner: transport);

		var act = () => harness.SendAsync(HttpMethod.Get);

		await act.Should().ThrowAsync<HttpRequestException>();
		transport.Requests.Should().ContainSingle();
		RetryHandler.IsConnectionFailure(new HttpRequestException(error)).Should().BeFalse();
	}
}
