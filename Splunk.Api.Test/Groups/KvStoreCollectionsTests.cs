using Splunk.Api.Models;
using Splunk.Api.Models.KvStore;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class KvStoreCollectionsTests
{
	private const string Path = "/servicesNS/nobody/search/storage/collections/config";

	// Captured from Splunk 10.6.0.5: POST servicesNS/nobody/search/storage/collections/config (content).
	private const string CollectionContent = """
		{
			"accelerated_fields.byname": "{\"name\":1}",
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "search",
			"eai:userName": "nobody",
			"enforceTypes": "true",
			"field.flag": "bool",
			"field.n": "number",
			"field.name": "string",
			"field.odd": "geo",
			"field.weird": 5,
			"profilingEnabled": "false",
			"profilingThresholdMs": "1000",
			"replicate": "false",
			"replication_dump_maximum_file_size": "10240",
			"replication_dump_strategy": "auto",
			"type": "undefined"
		}
		""";

	private static SplunkClient App(SplunkClient client) => client.InNamespace("nobody", "search");

	[Fact]
	public async Task ListAsync_SendsGetInTheNamespace()
		=> await Calls.AssertAsync(
			c => App(c).KvStoreCollections.ListAsync(new ListOptions { Count = 0 }, Calls.Token),
			HttpMethod.Get, Path, "?count=0&output_mode=json", null);

	[Fact]
	public async Task CreateAsync_PostsSettingsFieldsAndAccelerations()
		=> await Calls.AssertAsync(
			c => App(c).KvStoreCollections.CreateAsync(
				new KvStoreCollectionCreateRequest
				{
					Name = "assets",
					EnforceTypes = true,
					Replicate = false,
					ProfilingEnabled = true,
					ProfilingThresholdMs = 250,
					Fields = new Dictionary<string, KvStoreFieldType> { ["name"] = KvStoreFieldType.String, ["n"] = KvStoreFieldType.Number },
					AcceleratedFields = new Dictionary<string, string> { ["byName"] = """{"name":1}""" }
				},
				Calls.Token),
			HttpMethod.Post, Path, Calls.JsonQuery,
			"name=assets&enforceTypes=true&replicate=false&profilingEnabled=true&profilingThresholdMs=250"
			+ "&field.name=string&field.n=number&accelerated_fields.byName=%7B%22name%22%3A1%7D");

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => App(c).KvStoreCollections.GetAsync("assets", Calls.Token), HttpMethod.Get, Path + "/assets", Calls.JsonQuery, null);

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> await Calls.AssertAsync(
			c => App(c).KvStoreCollections.UpdateAsync(
				"assets",
				new KvStoreCollectionUpdateRequest { Fields = new Dictionary<string, KvStoreFieldType> { ["flag"] = KvStoreFieldType.Bool } },
				Calls.Token),
			HttpMethod.Post, Path + "/assets", Calls.JsonQuery, "field.flag=bool");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> await Calls.AssertAsync(c => App(c).KvStoreCollections.DeleteAsync("assets", Calls.Token), HttpMethod.Delete, Path + "/assets", Calls.JsonQuery, null);

	[Fact]
	public void Settings_ReadBackFieldsAndAccelerations()
	{
		var request = new KvStoreCollectionUpdateRequest
		{
			AdditionalParameters = { ["other"] = "x", ["field.blank"] = null },
			Fields = new Dictionary<string, KvStoreFieldType> { ["ip"] = KvStoreFieldType.Cidr, ["when"] = KvStoreFieldType.Time, ["tags"] = KvStoreFieldType.Array },
			AcceleratedFields = new Dictionary<string, string> { ["byIp"] = """{"ip":1}""" }
		};

		request.Fields.Should().BeEquivalentTo(new Dictionary<string, KvStoreFieldType>
		{
			["blank"] = KvStoreFieldType.Unknown,
			["ip"] = KvStoreFieldType.Cidr,
			["when"] = KvStoreFieldType.Time,
			["tags"] = KvStoreFieldType.Array
		});
		request.AcceleratedFields.Should().Equal(new Dictionary<string, string> { ["byIp"] = """{"ip":1}""" });
	}

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => App(c).KvStoreCollections.GetAsync("assets", Calls.Token), Feed.Of("assets", CollectionContent));

		var collection = feed.Entries.Should().ContainSingle().Subject.Content!;
		collection.EnforceTypes.Should().BeTrue();
		collection.Replicate.Should().BeFalse();
		collection.ProfilingEnabled.Should().BeFalse();
		collection.ProfilingThresholdMs.Should().Be(1000);
		collection.ReplicationDumpMaximumFileSize.Should().Be(10240);
		collection.ReplicationDumpStrategy.Should().Be("auto");
		collection.Type.Should().Be("undefined");
		collection.EaiAppName.Should().Be("search");
		collection.Fields.Should().BeEquivalentTo(new Dictionary<string, KvStoreFieldType>
		{
			["flag"] = KvStoreFieldType.Bool,
			["n"] = KvStoreFieldType.Number,
			["name"] = KvStoreFieldType.String,
			["odd"] = KvStoreFieldType.Unknown
		});
		collection.AcceleratedFields.Should().Equal(new Dictionary<string, string> { ["byname"] = """{"name":1}""" });
	}

	[Fact]
	public async Task ListAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.KvStoreCollections.ListAsync(null, Calls.Token), HttpStatusCode.BadRequest);
}
