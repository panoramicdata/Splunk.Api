using Splunk.Api.Models;
using Splunk.Api.Models.FederatedSearch;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class FederatedIndexesTests
{
	private const string Path = "/services/data/federated/index";
	private const string Name = "federated:remote_main";
	private const string EscapedName = "federated%3Aremote_main";

	// Captured from Splunk Enterprise 10.6.0.5 (a Federated Search for Splunk index), with the Amazon S3 time fields
	// added from the reference.
	private const string IndexContent = """
		{
			"disabled": false,
			"eai:acl": null,
			"federated.dataset": "index:main",
			"federated.provider": "remote1",
			"federated.timefield": "field_1",
			"federated.timeformat": "%s",
			"federated.unixtimefield": "_time",
			"federated.partition.time.fields": "year,month",
			"federated.partition.time.formats": "%Y,%m",
			"federated.partition.time.types": "string,integer",
			"federated.partition.time.tz": "America/Los_Angeles"
		}
		""";

	private static FederatedIndexUpdateRequest TimeFields => new()
	{
		Dataset = "index:other",
		TimeField = "f",
		TimeFormat = "%s",
		UnixTimeField = "u",
		PartitionTimeFields = "y",
		PartitionTimeFormats = "%Y",
		PartitionTimeTypes = "string",
		PartitionTimeZone = "UTC"
	};

	private const string TimeFieldsBody = "&federated.timefield=f&federated.timeformat=%25s&federated.unixtimefield=u&federated.partition.time.fields=y"
		+ "&federated.partition.time.formats=%25Y&federated.partition.time.types=string&federated.partition.time.tz=UTC";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedIndexes.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task CreateAsync_SendsPostWithTheIndex()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedIndexes.CreateAsync(
			new FederatedIndexCreateRequest
			{
				Name = Name,
				Provider = "remote1",
				Dataset = "index:main",
				TimeField = TimeFields.TimeField,
				TimeFormat = TimeFields.TimeFormat,
				UnixTimeField = TimeFields.UnixTimeField,
				PartitionTimeFields = TimeFields.PartitionTimeFields,
				PartitionTimeFormats = TimeFields.PartitionTimeFormats,
				PartitionTimeTypes = TimeFields.PartitionTimeTypes,
				PartitionTimeZone = TimeFields.PartitionTimeZone
			},
			ct)))
			.ShouldBeProbed(HttpMethod.Post, Path, body: $"name={EscapedName}&federated.provider=remote1&federated.dataset=index%3Amain{TimeFieldsBody}");

	[Fact]
	public async Task GetAsync_SendsGetWithTheEscapedName()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedIndexes.GetAsync(Name, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/{EscapedName}");

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSetFields()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedIndexes.UpdateAsync(Name, TimeFields, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{EscapedName}", body: $"{TimeFieldsBody[1..]}&federated.dataset=index%3Aother");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedIndexes.DeleteAsync(Name, ct)))
			.ShouldBeProbed(HttpMethod.Delete, $"{Path}/{EscapedName}");

	[Fact]
	public async Task DisableAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedIndexes.DisableAsync(Name, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{EscapedName}/disable");

	[Fact]
	public async Task EnableAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedIndexes.EnableAsync(Name, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{EscapedName}/enable");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var index = await RequestProbe.ReadContentAsync((c, ct) => c.FederatedIndexes.GetAsync(Name, ct), Name, IndexContent);

		index.Provider.Should().Be("remote1");
		index.Dataset.Should().Be("index:main");
		index.TimeField.Should().Be("field_1");
		index.TimeFormat.Should().Be("%s");
		index.UnixTimeField.Should().Be("_time");
		index.PartitionTimeFields.Should().Be("year,month");
		index.PartitionTimeFormats.Should().Be("%Y,%m");
		index.PartitionTimeTypes.Should().Be("string,integer");
		index.PartitionTimeZone.Should().Be("America/Los_Angeles");
		index.Disabled.Should().BeFalse();
	}

	[Fact]
	public Task GetAsync_Missing_RaisesNotFound()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.FederatedIndexes.GetAsync("federated:x", ct),
			HttpStatusCode.NotFound,
			"""{"messages":[{"type":"ERROR","text":"Could not find object id=federated:x"}]}""",
			"Could not find object id=federated:x");
}
