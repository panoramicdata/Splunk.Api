using Splunk.Api.Interfaces;
using Splunk.Api.Models;
using Splunk.Api.Models.KvStore;
using Splunk.Api.Test.Support.Platform;
using System.Net;
using System.Text.Json.Serialization;

namespace Splunk.Api.Test.Groups;

public class KvStoreDataTests
{
	private const string Path = "/servicesNS/nobody/search/storage/collections/data/assets";

	// Captured from Splunk 10.6.0.5 (GET storage/collections/data/{collection}); generated keys replaced.
	private const string DocumentsJson = """
		[
			{"n":1,"name":"a","_user":"nobody","_key":"key-a"},
			{"n":2,"name":"b","_user":"nobody","_key":"key-b"}
		]
		""";

	public sealed class Asset : KvStoreDocument
	{
		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("n")]
		public int N { get; set; }
	}

	private static IKvStoreData Data(SplunkClient client) => client.InNamespace("nobody", "search").KvStoreData;

	[Fact]
	public async Task QueryAsync_SendsEveryQueryOption()
	{
		var call = await Calls.RecordAsync(
			c => Data(c).QueryAsync<Asset>(
				"assets",
				new KvStoreQuery { Query = """{"n":{"$gt":1}}""", Fields = ["name", "n"], Sort = "n:-1", Limit = 5, Skip = 10, Shared = true },
				Calls.Token),
			"[]");

		call.Method.Should().Be(HttpMethod.Get);
		call.Uri.AbsolutePath.Should().Be(Path);
		Uri.UnescapeDataString(call.Uri.Query).Should().Be("""?query={"n":{"$gt":1}}&fields=name,n&sort=n:-1&limit=5&skip=10&shared=true&output_mode=json""");
		call.Body.Should().BeNull();
	}

	[Fact]
	public async Task QueryAsync_WithoutOptions_SendsNoQuery()
		=> await AssertJsonCallAsync(c => Data(c).QueryAsync<Asset>("assets", null, Calls.Token), "[]", HttpMethod.Get, Path, Calls.JsonQuery, null);

	[Fact]
	public async Task InsertAsync_PostsTheDocumentAsJson()
		=> await AssertJsonCallAsync(
			c => Data(c).InsertAsync("assets", new JsonBody<Asset>(new Asset { Name = "a", N = 1 }), Calls.Token),
			"""{"_key":"key-a"}""", HttpMethod.Post, Path, Calls.JsonQuery, """{"name":"a","n":1}""");

	[Fact]
	public async Task DeleteWhereAsync_SendsTheFilter()
	{
		var call = await Calls.RecordAsync(c => Data(c).DeleteWhereAsync("assets", new KvStoreDeleteOptions { Query = """{"name":"b"}""" }, Calls.Token), "");

		call.Method.Should().Be(HttpMethod.Delete);
		call.Uri.AbsolutePath.Should().Be(Path);
		Uri.UnescapeDataString(call.Uri.Query).Should().Be("""?query={"name":"b"}&output_mode=json""");
	}

	[Fact]
	public async Task GetAsync_SendsGetForTheKey()
		=> await AssertJsonCallAsync(
			c => Data(c).GetAsync<Asset>("assets", "key/a", Calls.Token),
			"""{"_key":"key/a"}""", HttpMethod.Get, Path + "/key%2Fa", Calls.JsonQuery, null);

	[Fact]
	public async Task UpdateAsync_PostsTheWholeDocument()
		=> await AssertJsonCallAsync(
			c => Data(c).UpdateAsync("assets", "key-a", new JsonBody<Asset>(new Asset { Key = "key-a", User = "nobody", Name = "a2", N = 3 }), Calls.Token),
			"""{"_key":"key-a"}""", HttpMethod.Post, Path + "/key-a", Calls.JsonQuery, """{"name":"a2","n":3,"_key":"key-a","_user":"nobody"}""");

	[Fact]
	public async Task DeleteAsync_SendsDeleteForTheKey()
		=> await AssertJsonCallAsync(c => Data(c).DeleteAsync("assets", "key-a", Calls.Token), "", HttpMethod.Delete, Path + "/key-a", Calls.JsonQuery, null);

	[Fact]
	public async Task BatchFindAsync_PostsTheQueries()
		=> await AssertJsonCallAsync(
			c => Data(c).BatchFindAsync<Asset>(
				"assets",
				new JsonBody<IEnumerable<KvStoreBatchQuery>>(
				[
					new KvStoreBatchQuery { Query = new { name = "a" } },
					new KvStoreBatchQuery
					{
						Query = new Dictionary<string, object> { ["n"] = new Dictionary<string, int> { ["$gte"] = 2 } },
						Fields = ["name"],
						Sort = [new Dictionary<string, int> { ["n"] = -1 }],
						Limit = 1,
						Skip = 0,
						Shared = false
					}
				]),
				Calls.Token),
			"[[],[]]", HttpMethod.Post, Path + "/batch_find", Calls.JsonQuery,
			"""[{"query":{"name":"a"}},{"query":{"n":{"$gte":2}},"fields":["name"],"sort":[{"n":-1}],"limit":1,"skip":0,"shared":false}]""");

	[Fact]
	public async Task BatchSaveAsync_PostsTheDocuments()
		=> await AssertJsonCallAsync(
			c => Data(c).BatchSaveAsync<Asset>("assets", new JsonBody<IEnumerable<Asset>>([new Asset { Name = "b", N = 2 }, new Asset { Key = "k3", Name = "c", N = 3 }]), Calls.Token),
			"""["key-b","k3"]""", HttpMethod.Post, Path + "/batch_save", Calls.JsonQuery,
			"""[{"name":"b","n":2},{"name":"c","n":3,"_key":"k3"}]""");

	[Fact]
	public async Task QueryAsync_MapsDocuments()
	{
		var documents = await Calls.MapAsync(c => Data(c).QueryAsync<Asset>("assets", null, Calls.Token), DocumentsJson);

		documents.Should().HaveCount(2);
		documents[0].Key.Should().Be("key-a");
		documents[0].User.Should().Be("nobody");
		documents[0].Name.Should().Be("a");
		documents[0].N.Should().Be(1);
		documents[1].Key.Should().Be("key-b");
	}

	[Fact]
	public async Task InsertAsync_MapsTheKey()
	{
		var key = await Calls.MapAsync(c => Data(c).InsertAsync("assets", new JsonBody<Asset>(new Asset()), Calls.Token), """{"_key":"6ac7a06eca0f0da123bb7234"}""");

		key.Key.Should().Be("6ac7a06eca0f0da123bb7234");
	}

	[Fact]
	public async Task BatchFindAsync_MapsOneListPerQuery()
	{
		var results = await Calls.MapAsync(
			c => Data(c).BatchFindAsync<Asset>("assets", new JsonBody<IEnumerable<KvStoreBatchQuery>>([new KvStoreBatchQuery(), new KvStoreBatchQuery()]), Calls.Token),
			"""[[{"n":1,"name":"a","_user":"nobody","_key":"key-a"}],[{"name":"c2"}]]""");

		results.Should().HaveCount(2);
		results[0].Should().ContainSingle().Which.Key.Should().Be("key-a");
		results[1].Should().ContainSingle().Which.Name.Should().Be("c2");
	}

	[Fact]
	public async Task BatchSaveAsync_MapsTheKeys()
	{
		var keys = await Calls.MapAsync(c => Data(c).BatchSaveAsync<Asset>("assets", new JsonBody<IEnumerable<Asset>>([]), Calls.Token), """["key-b","k3"]""");

		keys.Should().Equal("key-b", "k3");
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.KvStoreData.GetAsync<Asset>("assets", "missing", Calls.Token), HttpStatusCode.NotFound);

	private static async Task AssertJsonCallAsync(Func<SplunkClient, Task> call, string response, HttpMethod method, string path, string query, string? body)
	{
		var recorded = await Calls.RecordAsync(call, response);

		recorded.Method.Should().Be(method);
		recorded.Uri.AbsolutePath.Should().Be(path);
		recorded.Uri.Query.Should().Be(query);
		recorded.Body.Should().Be(body);
		if (body is not null)
		{
			recorded.ContentType.Should().Be("application/json");
		}
	}

}
