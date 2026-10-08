using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class KvStoreIntrospectionTests
{
	private const string Path = "/services/server/introspection/kvstore";

	// Captured from Splunk 10.6.0.5 (GET server/introspection/kvstore/collectionstats), one collection.
	private const string CollectionStatsContent = """
		{
			"data": ["{\"collection\":\"search.assets\",\"count\":16,\"indexSizes\":{\"pkey\":16384,\"byName\":8192},\"lastModifiedTime\":\"\",\"nindexes\":2,\"size\":106496,\"storageSize\":106497,\"totalIndexSize\":24576,\"kvstoreType\":\"external\",\"ns\":\"search.assets\"}"],
			"eai:acl": null
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.KvStoreIntrospection.ListAsync(Calls.Token), HttpMethod.Get, Path, Calls.JsonQuery, null);

	[Fact]
	public async Task GetCollectionStatsAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.KvStoreIntrospection.GetCollectionStatsAsync(Calls.Token), HttpMethod.Get, Path + "/collectionstats", Calls.JsonQuery, null);

	[Fact]
	public async Task GetReplicaSetStatsAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.KvStoreIntrospection.GetReplicaSetStatsAsync(Calls.Token), HttpMethod.Get, Path + "/replicasetstats", Calls.JsonQuery, null);

	[Fact]
	public async Task GetServerStatusAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.KvStoreIntrospection.GetServerStatusAsync(Calls.Token), HttpMethod.Get, Path + "/serverstatus", Calls.JsonQuery, null);

	[Fact]
	public async Task GetCollectionStatsAsync_ParsesEachCollection()
	{
		var feed = await Calls.MapAsync(c => c.KvStoreIntrospection.GetCollectionStatsAsync(Calls.Token), Feed.Of("collectionStats", CollectionStatsContent));

		var stats = feed.Entries.Should().ContainSingle().Subject.Content!;
		stats.Data.Should().ContainSingle();
		var collection = stats.Collections.Should().ContainSingle().Subject;
		collection.Collection.Should().Be("search.assets");
		collection.Namespace.Should().Be("search.assets");
		collection.Count.Should().Be(16);
		collection.Size.Should().Be(106496);
		collection.StorageSize.Should().Be(106497);
		collection.TotalIndexSize.Should().Be(24576);
		collection.IndexCount.Should().Be(2);
		collection.IndexSizes.Should().Contain("byName", 8192);
		collection.KvStoreType.Should().Be("external");
		collection.LastModifiedTime.Should().BeEmpty();
	}

	[Fact]
	public async Task GetServerStatusAsync_ParsesTheDocument()
	{
		// Captured from Splunk 10.6.0.5.
		var feed = await Calls.MapAsync(
			c => c.KvStoreIntrospection.GetServerStatusAsync(Calls.Token),
			Feed.Of("serverStatus", """{"data":"{\"database\":\"kvservice\",\"status\":\"healthy\"}","eai:acl":null}"""));

		var status = feed.Entries.Should().ContainSingle().Subject.Content!;
		status.ParseData()!.Value.GetProperty("status").GetString().Should().Be("healthy");
	}

	[Fact]
	public async Task GetServerStatusAsync_WithoutData_ParsesToNull()
	{
		var feed = await Calls.MapAsync(c => c.KvStoreIntrospection.GetServerStatusAsync(Calls.Token), Feed.Of("serverStatus", """{"eai:acl":null}"""));

		feed.Entries[0].Content!.ParseData().Should().BeNull();
	}

	[Fact]
	public async Task GetReplicaSetStatsAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.KvStoreIntrospection.GetReplicaSetStatsAsync(Calls.Token), HttpStatusCode.ServiceUnavailable);
}
