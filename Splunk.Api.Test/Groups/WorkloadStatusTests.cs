using Splunk.Api.Models.WorkloadManagement;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class WorkloadStatusTests
{
	// Captured from Splunk 10.6.0.5 (GET workloads/status?advanced=true); search-filter-rules filled in to show a list.
	private const string AdmissionContent = """{"eai:acl":null,"enabled":"0","search-filter-rules":[{"name":"r1"}]}""";

	private const string ManagementContent = """
		{
			"eai:acl": null,
			"general": {
				"allow_basic": "0",
				"enabled": "0",
				"error_message": "",
				"isSupported": "1",
				"mode": "basic",
				"os_build": "#1 SMP PREEMPT_DYNAMIC",
				"os_extended_name": "Linux",
				"os_name": "Linux",
				"os_version": "6.18.33.2"
			},
			"workload-rules": null
		}
		""";

	[Fact]
	public async Task GetAsync_SendsAdvanced()
		=> await Calls.AssertAsync(
			c => c.WorkloadStatus.GetAsync(new WorkloadStatusOptions { Advanced = true }, Calls.Token),
			HttpMethod.Get, "/services/workloads/status", "?advanced=true&output_mode=json", null);

	[Fact]
	public async Task GetAsync_MapsBothEntries()
	{
		var feed = await Calls.MapAsync(
			c => c.WorkloadStatus.GetAsync(null, Calls.Token),
			Feed.Of(("admission-control-status", AdmissionContent), ("workload-management-status", ManagementContent)));

		var admission = feed.Entries[0].Content!;
		admission.Enabled.Should().BeFalse();
		admission.SearchFilterRules!.Value.GetArrayLength().Should().Be(1);
		var management = feed.Entries[1].Content!;
		management.WorkloadRules.Should().BeNull();
		var general = management.General!;
		general.AllowBasic.Should().BeFalse();
		general.Enabled.Should().BeFalse();
		general.ErrorMessage.Should().BeEmpty();
		general.IsSupported.Should().BeTrue();
		general.Mode.Should().Be("basic");
		general.OsBuild.Should().Be("#1 SMP PREEMPT_DYNAMIC");
		general.OsExtendedName.Should().Be("Linux");
		general.OsName.Should().Be("Linux");
		general.OsVersion.Should().Be("6.18.33.2");
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.WorkloadStatus.GetAsync(null, Calls.Token), HttpStatusCode.Forbidden);
}
