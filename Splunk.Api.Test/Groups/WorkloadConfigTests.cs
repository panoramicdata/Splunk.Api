using Splunk.Api.Models.WorkloadManagement;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class WorkloadConfigTests
{
	// Captured from Splunk 10.6.0.5 in Docker on WSL2 (GET workloads/config/preflight-checks), trimmed to three checks.
	private const string PreflightContent = """
		{
			"cgroup_version": { "mitigation": "The cgroup version must be version 1 or 2, and cgroups must be properly mounted.", "preflight_check_status": true, "title": "Cgroup Configuration" },
			"cpu_splunk_base_dir_permission": { "mitigation": "CPU Splunk base directory splunk requires read and write permissions.", "preflight_check_status": false, "title": "CPU Splunk base directory permissions" },
			"eai:acl": null,
			"general": { "preflight_checks_status": false, "systemd_present": false },
			"platform_type": { "mitigation": "Operating system must be Linux.", "preflight_check_status": true, "title": "Operating System" }
		}
		""";

	[Fact]
	public async Task EnableAsync_PostsWithNoBody()
		=> await Calls.AssertAsync(c => c.WorkloadConfig.EnableAsync(Calls.Token), HttpMethod.Post, "/services/workloads/config/enable", Calls.JsonQuery, null);

	[Fact]
	public async Task DisableAsync_PostsWithNoBody()
		=> await Calls.AssertAsync(c => c.WorkloadConfig.DisableAsync(Calls.Token), HttpMethod.Post, "/services/workloads/config/disable", Calls.JsonQuery, null);

	[Fact]
	public async Task GetBaseDirectoryAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.WorkloadConfig.GetBaseDirectoryAsync(Calls.Token), HttpMethod.Get, "/services/workloads/config/get-base-dirname", Calls.JsonQuery, null);

	[Fact]
	public async Task GetPreflightChecksAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.WorkloadConfig.GetPreflightChecksAsync(Calls.Token), HttpMethod.Get, "/services/workloads/config/preflight-checks", Calls.JsonQuery, null);

	[Fact]
	public async Task SetBaseDirectoryAsync_PostsTheName()
		=> await Calls.AssertAsync(
			c => c.WorkloadConfig.SetBaseDirectoryAsync(new WorkloadBaseDirectoryRequest { Name = "splunk" }, Calls.Token),
			HttpMethod.Post, "/services/workloads/config/set-base-dirname", Calls.JsonQuery, "workload_pool_base_dir_name=splunk");

	[Fact]
	public async Task GetBaseDirectoryAsync_MapsTheName()
	{
		var feed = await Calls.MapAsync(
			c => c.WorkloadConfig.GetBaseDirectoryAsync(Calls.Token),
			Feed.Of("workload-pool-base-path", """{"eai:acl":null,"workload_pool_base_dir_name":"splunk"}"""));

		feed.Entries.Should().ContainSingle().Which.Content!.Name.Should().Be("splunk");
	}

	[Fact]
	public async Task GetPreflightChecksAsync_MapsTheSummaryAndEachCheck()
	{
		var feed = await Calls.MapAsync(c => c.WorkloadConfig.GetPreflightChecksAsync(Calls.Token), Feed.Of("workload-management-preflight-checks", PreflightContent));

		var checks = feed.Entries.Should().ContainSingle().Subject.Content!;
		checks.General!.Passed.Should().BeFalse();
		checks.General.SystemdPresent.Should().BeFalse();
		checks.Checks.Keys.Should().BeEquivalentTo("cgroup_version", "cpu_splunk_base_dir_permission", "platform_type");
		var permission = checks.Checks["cpu_splunk_base_dir_permission"];
		permission.Passed.Should().BeFalse();
		permission.Title.Should().Be("CPU Splunk base directory permissions");
		permission.Mitigation.Should().Be("CPU Splunk base directory splunk requires read and write permissions.");
		checks.Checks["platform_type"].Passed.Should().BeTrue();
	}

	[Fact]
	public async Task EnableAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.WorkloadConfig.EnableAsync(Calls.Token), HttpStatusCode.BadRequest);
}
