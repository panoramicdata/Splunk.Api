using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class IndexVolumesTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.IndexVolumes.ListAsync(null, Calls.Token), HttpMethod.Get, "/services/data/index-volumes", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(
			c => c.IndexVolumes.GetAsync("_splunk_summaries", Calls.Token),
			HttpMethod.Get, "/services/data/index-volumes/_splunk_summaries", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		// Captured from Splunk 10.6.0.5, with total_size as Splunk reports it for a capped volume.
		var feed = await Calls.MapAsync(
			c => c.IndexVolumes.GetAsync("_splunk_summaries", Calls.Token),
			Feed.Of("_splunk_summaries", """{"eai:acl":null,"max_size":"infinite","name":"_splunk_summaries","total_size":"12.5","volume_path":"/opt/splunk/var/lib/splunk"}"""));

		var volume = feed.Entries.Should().ContainSingle().Subject.Content!;
		volume.Name.Should().Be("_splunk_summaries");
		volume.MaxSize.Should().Be("infinite");
		volume.TotalSizeMB.Should().Be(12.5);
		volume.VolumePath.Should().Be("/opt/splunk/var/lib/splunk");
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.IndexVolumes.GetAsync("nope", Calls.Token), HttpStatusCode.NotFound);
}
