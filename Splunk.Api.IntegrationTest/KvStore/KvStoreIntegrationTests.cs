using Splunk.Api.Models;
using Splunk.Api.Models.KvStore;
using System.Net;
using System.Text.Json.Serialization;

namespace Splunk.Api.IntegrationTest.KvStore;

/// <summary>
/// KV store round trips in a collection created (and deleted) by each test. Backup creation and maintenance mode would
/// affect everyone sharing the instance, so they are covered by unit tests only.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class KvStoreIntegrationTests(SplunkFixture fixture)
{
	public sealed class Asset : KvStoreDocument
	{
		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("n")]
		public int N { get; set; }
	}

	private static CancellationToken Token => TestContext.Current.CancellationToken;

	private SplunkClient App => fixture.Client.InNamespace("nobody", "search");

	[Fact]
	public async Task Status_IsReady()
	{
		var feed = await fixture.Client.KvStore.GetStatusAsync(Token);

		var status = feed.Entries.Should().ContainSingle().Subject.Content!;
		status.Current!.Status.Should().Be("ready");
		status.Current.Standalone.Should().BeTrue();
	}

	[Fact]
	public async Task Restore_OfAMissingArchive_IsRejected()
	{
		var act = () => fixture.Client.KvStore.RestoreBackupAsync(
			new KvStoreRestoreRequest { ArchiveName = SplunkFixture.UniqueName("missing") + ".tar.gz", AppName = "search", CollectionName = SplunkFixture.UniqueName("none") },
			Token);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Collections_InTheGlobalContext_AreRejected()
	{
		var act = () => fixture.Client.KvStoreCollections.ListAsync(null, Token);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Contain("nobody");
	}

	[Fact]
	public async Task Collection_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("kv");
		var created = await App.KvStoreCollections.CreateAsync(
			new KvStoreCollectionCreateRequest
			{
				Name = name,
				EnforceTypes = true,
				Fields = new Dictionary<string, KvStoreFieldType> { ["name"] = KvStoreFieldType.String, ["n"] = KvStoreFieldType.Number },
				AcceleratedFields = new Dictionary<string, string> { ["byName"] = """{"name":1}""" }
			},
			Token);
		try
		{
			var definition = created.Entries.Should().ContainSingle().Subject.Content!;
			definition.EnforceTypes.Should().BeTrue();
			definition.Fields.Should().Contain("n", KvStoreFieldType.Number);
			definition.AcceleratedFields.Should().ContainKey("byName");

			await App.KvStoreCollections.UpdateAsync(
				name,
				new KvStoreCollectionUpdateRequest { Fields = new Dictionary<string, KvStoreFieldType> { ["flag"] = KvStoreFieldType.Bool } },
				Token);
			(await App.KvStoreCollections.GetAsync(name, Token)).Entries.Single().Content!.Fields.Should().Contain("flag", KvStoreFieldType.Bool);
			(await App.KvStoreCollections.ListAsync(new ListOptions { Count = 0, Search = name }, Token)).Entries.Select(e => e.Name).Should().Contain(name);

			await DocumentsRoundTripAsync(name);
		}
		finally
		{
			await App.KvStoreCollections.DeleteAsync(name, CancellationToken.None);
		}

		var act = () => App.KvStoreCollections.GetAsync(name, Token);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	private async Task DocumentsRoundTripAsync(string collection)
	{
		var data = App.KvStoreData;

		var a = await data.InsertAsync(collection, new JsonBody<Asset>(new Asset { Name = "a", N = 1 }), Token);
		var saved = await data.BatchSaveAsync(collection, new JsonBody<IEnumerable<Asset>>([new Asset { Name = "b", N = 2 }, new Asset { Key = "k3", Name = "c", N = 3 }]), Token);
		saved.Should().HaveCount(2).And.EndWith("k3");

		var c = await data.GetAsync<Asset>(collection, "k3", Token);
		c.Name.Should().Be("c");
		c.User.Should().Be("nobody");
		c.Name = "c2";
		c.N = 33;
		(await data.UpdateAsync(collection, "k3", new JsonBody<Asset>(c), Token)).Key.Should().Be("k3");

		var query = await data.QueryAsync<Asset>(collection, new KvStoreQuery { Query = """{"n":{"$gt":1}}""", Sort = "n:-1", Limit = 5, Fields = ["name", "n", "_key"] }, Token);
		query.Select(d => d.Name).Should().Equal("c2", "b");

		var batch = await data.BatchFindAsync<Asset>(
			collection,
			new JsonBody<IEnumerable<KvStoreBatchQuery>>(
			[
				new KvStoreBatchQuery { Query = new { name = "a" } },
				new KvStoreBatchQuery { Query = new Dictionary<string, object> { ["n"] = new Dictionary<string, int> { ["$gte"] = 2 } }, Sort = [new Dictionary<string, int> { ["n"] = 1 }], Limit = 1 }
			]),
			Token);
		batch[0].Should().ContainSingle().Which.Key.Should().Be(a.Key);
		batch[1].Should().ContainSingle().Which.Name.Should().Be("b");

		await StatisticsAsync(collection);

		await data.DeleteAsync(collection, a.Key, Token);
		await data.DeleteWhereAsync(collection, new KvStoreDeleteOptions { Query = """{"name":"b"}""" }, Token);
		(await data.QueryAsync<Asset>(collection, null, Token)).Should().ContainSingle().Which.Key.Should().Be("k3");
		await data.DeleteWhereAsync(collection, null, Token);
		(await data.QueryAsync<Asset>(collection, null, Token)).Should().BeEmpty();

		var missing = () => data.GetAsync<Asset>(collection, "k3", Token);
		(await missing.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	private async Task StatisticsAsync(string collection)
	{
		var whole = await App.KvStoreStatistics.GetCollectionAsync(collection, Token);
		whole.Collection.Should().Be(collection);
		whole.App.Should().Be("search");
		whole.Stats!.RecordCount.Should().Be(3);

		var fields = await App.KvStoreStatistics.GetFieldsAsync(
			new KvStoreFieldStatisticsOptions { Collection = collection, Filter = """{"n":{"$gt":1}}""", Fields = ["n"], Aggregations = ["count", "max"] },
			Token);
		fields.Stats!.Count.Should().Be(2);
		fields.Stats.Aggregations["n"]["max"].GetDouble().Should().Be(33);
	}
}
