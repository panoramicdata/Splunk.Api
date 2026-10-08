using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SearchTypeaheadTests
{
	private const string SuggestionsJson = """{"results":[{"content":"index=\"_audit\"","count":0,"operator":true},{"content":"index=\"_internal\"","count":"1200","operator":false}]}""";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task GetAsync_SendsThePrefixAndCount()
	{
		var stub = TestClient.Stub(SuggestionsJson);
		using var client = TestClient.Create(stub);

		await client.Typeahead.GetAsync("index=_", 3, 1, Ct);

		stub.ShouldHaveSent(HttpMethod.Get, "/services/search/typeahead", "?prefix=index%3D_&count=3&max_servers=1&output_mode=json");
	}

	[Fact]
	public async Task GetAsync_LeavesOutMaxServersWhenNull()
	{
		var stub = TestClient.Stub(SuggestionsJson);
		using var client = TestClient.Create(stub);

		var suggestions = await client.Typeahead.GetAsync("index=_", 3, null, Ct);

		stub.Calls[0].Uri.Query.Should().Be("?prefix=index%3D_&count=3&output_mode=json");
		suggestions.Results.Should().HaveCount(2);
		suggestions.Results[0].Content.Should().Be("index=\"_audit\"");
		suggestions.Results[0].Operator.Should().BeTrue();
		suggestions.Results[1].Count.Should().Be(1200);
		suggestions.Results[1].Operator.Should().BeFalse();
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Missing prefix."}]}""", HttpStatusCode.BadRequest));

		var act = () => client.Typeahead.GetAsync(string.Empty, 3, null, Ct);

		await act.ShouldFailWith(HttpStatusCode.BadRequest, "Missing prefix.");
	}
}
