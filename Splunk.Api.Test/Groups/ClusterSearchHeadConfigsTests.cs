using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterSearchHeadConfigsTests
{
	private const string Path = "/services/cluster/searchhead/searchheadconfig";
	private const string Name = "https:%2F%2Fcm:8089";
	private const string EscapedName = "https%3A%252F%252Fcm%3A8089";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterSearchHeadConfigs.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task CreateAsync_SendsPostWithManagerAndSecret()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterSearchHeadConfigs.CreateAsync(new ClusterSearchHeadConfigCreateRequest { Name = "https://cm:8089", Secret = "s3cret" }, ct)))
			.ShouldBeProbed(HttpMethod.Post, Path, body: "name=https%3A%2F%2Fcm%3A8089&secret=s3cret");

	[Fact]
	public async Task GetAsync_SendsGetWithTheName()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterSearchHeadConfigs.GetAsync(Name, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/{EscapedName}");

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSettings()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterSearchHeadConfigs.UpdateAsync(Name, new ClusterSearchHeadConfigUpdateRequest { ManagerUri = "https://cm2:8089", Secret = "new" }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{EscapedName}", body: "manager_uri=https%3A%2F%2Fcm2%3A8089&secret=new");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterSearchHeadConfigs.DeleteAsync(Name, ct)))
			.ShouldBeProbed(HttpMethod.Delete, $"{Path}/{EscapedName}");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		// From the reference's example.
		var config = await RequestProbe.ReadContentAsync(
			(c, ct) => c.ClusterSearchHeadConfigs.GetAsync(Name, ct),
			"https://cm:8089",
			"""{"manager_uri":"https://cm:8089","secret":"********"}""");

		config.ManagerUri.Should().Be("https://cm:8089");
		config.Secret.Should().Be("********");
	}

	[Fact]
	public Task DeleteAsync_NotASearchHead_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterSearchHeadConfigs.DeleteAsync(Name, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.SearchHeadNotEnabled,
			"Searchhead is not enabled on this node");
}
