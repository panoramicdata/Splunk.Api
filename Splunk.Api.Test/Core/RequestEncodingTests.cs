using Refit;
using Splunk.Api.Models;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Core;

/// <summary>Pins how Refit and the Splunk serializer encode query strings and request bodies.</summary>
public class RequestEncodingTests
{
	public interface IProbe
	{
		[Get("services/probe")]
		Task<SplunkFeed<SplunkDynamicContent>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

		[Post("services/probe")]
		Task<SplunkFeed<SplunkDynamicContent>> AclAsync([Body] AclUpdateRequest request, CancellationToken cancellationToken);

		[Post("services/probe")]
		Task JsonAsync([Body] JsonBody<Dictionary<string, int>> body, CancellationToken cancellationToken);

		[Post("services/probe/{name}")]
		Task PairsAsync(string name, [Body] IDictionary<string, string?> body, CancellationToken cancellationToken);
	}

	private static (IProbe Probe, StubHandler Stub) Create()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.OK, """{"entry":[]}""");
		var client = new HttpClient(stub) { BaseAddress = new Uri(TestClient.BaseUrl) };
		return (RestService.For<IProbe>(client, SplunkClient.Settings), stub);
	}

	[Fact]
	public async Task ListOptions_AreFlattenedIntoTheQuery()
	{
		var (probe, stub) = Create();

		await probe.ListAsync(
			new ListOptions
			{
				Count = 0,
				Offset = 5,
				Search = "disabled=0",
				SortKey = "name",
				SortDirection = SortDirection.Descending,
				SortMode = SortMode.AlphabeticalCaseSensitive,
				Fields = ["title", "dispatch.*"],
				Summarize = true
			},
			TestContext.Current.CancellationToken);

		Uri.UnescapeDataString(stub.Calls[0].Uri.Query).Should().Be(
			"?count=0&offset=5&search=disabled=0&sort_key=name&sort_dir=desc&sort_mode=alpha_case&f=title&f=dispatch.*&summarize=true");
	}

	[Fact]
	public async Task NullListOptions_SendNoQuery()
	{
		var (probe, stub) = Create();

		await probe.ListAsync(null, TestContext.Current.CancellationToken);

		stub.Calls[0].Uri.Query.Should().BeEmpty();
	}

	[Fact]
	public async Task FormRequest_IsSentAsUrlEncodedFields_WithRepeatedLists()
	{
		var (probe, stub) = Create();

		await probe.AclAsync(
			new AclUpdateRequest { Sharing = "app", Owner = "nobody", Read = ["*"], Write = ["admin", "power"] },
			TestContext.Current.CancellationToken);

		stub.Calls[0].ContentType.Should().Be("application/x-www-form-urlencoded");
		stub.Calls[0].Body.Should().Be("sharing=app&owner=nobody&perms.read=%2A&perms.write=admin&perms.write=power");
	}

	[Fact]
	public async Task JsonBody_IsSentAsJson()
	{
		var (probe, stub) = Create();

		await probe.JsonAsync(new JsonBody<Dictionary<string, int>>(new() { ["a"] = 1 }), TestContext.Current.CancellationToken);

		stub.Calls[0].ContentType.Should().Be("application/json");
		stub.Calls[0].Body.Should().Be("""{"a":1}""");
	}

	[Fact]
	public async Task DictionaryBody_IsSentAsGiven_AndPathValuesAreOneSegment()
	{
		var (probe, stub) = Create();

		await probe.PairsAsync("a/b c", new Dictionary<string, string?> { ["x.y"] = "1", ["empty"] = null }, TestContext.Current.CancellationToken);

		stub.Calls[0].Uri.AbsolutePath.Should().Be("/services/probe/a%2Fb%20c");
		stub.Calls[0].Body.Should().Be("x.y=1&empty=");
	}
}
