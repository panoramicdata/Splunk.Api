using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterManagerOperationsTests
{
	private const string Path = "/services/cluster/manager/control/default";

	[Fact]
	public async Task AbortRestartAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerOperations.AbortRestartAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/abort_restart");

	[Fact]
	public async Task ApplyBundleAsync_SendsPostWithTheOptions()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerOperations.ApplyBundleAsync(new ClusterApplyBundleRequest { SkipValidation = true, IgnoreIdenticalBundle = false }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/apply", body: "skip-validation=true&ignore_identical_bundle=false");

	[Fact]
	public async Task CancelBundlePushAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerOperations.CancelBundlePushAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/cancel_bundle_push");

	[Fact]
	public async Task SetMaintenanceModeAsync_SendsPostWithTheMode()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerOperations.SetMaintenanceModeAsync(new ClusterMaintenanceModeRequest { Mode = true }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/maintenance", body: "mode=true");

	[Fact]
	public async Task RollbackBundleAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerOperations.RollbackBundleAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/rollback");

	[Fact]
	public async Task ValidateBundleAsync_SendsPostWithCheckRestart()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerOperations.ValidateBundleAsync(new ClusterValidateBundleRequest { CheckRestart = true }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/validate_bundle", body: "check-restart=true");

	[Fact]
	public async Task ApplyBundleAsync_MapsTheBundleChecksum()
		=> (await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManagerOperations.ApplyBundleAsync(new ClusterApplyBundleRequest(), ct), "clusterbundles", """{"checksum":"288845778D5B1952F534AB16DD82881E"}"""))
			.Checksum.Should().Be("288845778D5B1952F534AB16DD82881E");

	[Fact]
	public async Task AbortRestartAsync_MapsTheMessages()
	{
		// From the reference's example.
		var feed = await RequestProbe.ReadAsync(
			(c, ct) => c.ClusterManagerOperations.AbortRestartAsync(ct),
			"""{"links":{},"entry":[],"paging":{"total":0,"perPage":30,"offset":0},"messages":[{"type":"INFO","text":"Aborting the rolling restart initiated successfully. List of peers skipped restarting: E30CA8C0"}]}""");

		feed.Messages.Should().ContainSingle().Which.Type.Should().Be("INFO");
	}

	[Fact]
	public Task SetMaintenanceModeAsync_NotAManager_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterManagerOperations.SetMaintenanceModeAsync(new ClusterMaintenanceModeRequest { Mode = false }, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ManagerNotEnabled,
			"Cluster manager is not enabled on this node");
}
