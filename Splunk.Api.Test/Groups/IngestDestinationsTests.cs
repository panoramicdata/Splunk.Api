using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class IngestDestinationsTests
{
	private const string Path = "/services/data/ingest/rfsdestinations";

	// Captured from Splunk 10.6.0.5 after creating a destination.
	private static readonly string DestinationJson = InputsTestKit.Feed("archive", """
		{
			"batchSizeThresholdKB": "131072", "batchTimeout": "30", "compression": "gzip", "compressionLevel": "3",
			"description": "Archive", "dropEventsOnUploadError": "false", "eai:acl": null, "format": "ndjson",
			"path": "s3://example-bucket/archive/", "remote.s3.access_key": "<hidden>", "remote.s3.encryption": "sse-s3",
			"remote.s3.endpoint": "https://s3.example.com", "remote.s3.secret_key": "<hidden>"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IngestDestinations.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IngestDestinations.CreateAsync(
			new IngestDestinationCreateRequest
			{
				Name = "archive",
				Path = "s3://example-bucket/archive/",
				Description = "Archive",
				Endpoint = "https://s3.example.com",
				AccessKey = "AKIA",
				SecretKey = "secret",
				Encryption = "sse-s3",
				Compression = "gzip",
				DropEventsOnUploadError = false,
				BatchTimeout = 30,
				BatchSizeThresholdKB = 131072,
				Target = "idx01"
			},
			ct)))
			.ShouldBePost(Path, "name=archive&path=s3%3A%2F%2Fexample-bucket%2Farchive%2F&description=Archive&remote.s3.endpoint=https%3A%2F%2Fs3.example.com&remote.s3.access_key=AKIA&remote.s3.secret_key=secret&remote.s3.encryption=sse-s3&compression=gzip&dropEventsOnUploadError=false&batchTimeout=30&batchSizeThresholdKB=131072&target=idx01");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IngestDestinations.DeleteAsync("archive", ct))).ShouldBeDelete(Path + "/archive");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var destination = (await InputsTestKit.MapEntryAsync((c, ct) => c.IngestDestinations.ListAsync(null, ct), DestinationJson)).Content!;

		destination.Path.Should().Be("s3://example-bucket/archive/");
		destination.Description.Should().Be("Archive");
		destination.Endpoint.Should().Be("https://s3.example.com");
		destination.AccessKey.Should().Be("<hidden>");
		destination.SecretKey.Should().Be("<hidden>");
		destination.Encryption.Should().Be("sse-s3");
		destination.Compression.Should().Be("gzip");
		destination.Format.Should().Be("ndjson");
		destination.DropEventsOnUploadError.Should().BeFalse();
		destination.BatchTimeout.Should().Be(30);
		destination.BatchSizeThresholdKB.Should().Be(131072);
	}

	[Fact]
	public Task DeleteAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.IngestDestinations.DeleteAsync("nope", ct));
}
