using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SystemInfoTests
{
	// Captured from Splunk 10.6.0.5 (GET server/sysinfo).
	private const string SysInfoContent = """
		{
			"cpu_arch": "x86_64", "eai:acl": null, "numberOfCores": 8, "numberOfVirtualCores": 16, "os_build": "#1 SMP",
			"os_name": "Linux", "os_name_extended": "Linux", "os_version": "6.18.33.2", "physicalMemoryMB": 48091,
			"transparent_hugepages": { "defrag": "madvise", "effective_state": "ok", "enabled": "madvise" },
			"ulimits": {
				"core_file_size": 0, "cpu_time": -1, "data_file_size": -1, "data_segment_size": -1, "nice": 0, "open_files": 1048576,
				"resident_memory_size": -1, "stack_size": 8388608, "user_processes": -1, "virtual_address_space_size": -1
			}
		}
		""";

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.SystemInfo.GetAsync(Calls.Token), HttpMethod.Get, "/services/server/sysinfo", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_MapsEveryField()
	{
		var feed = await Calls.MapAsync(c => c.SystemInfo.GetAsync(Calls.Token), Feed.Of("system-info", SysInfoContent));

		var info = feed.Entries.Should().ContainSingle().Subject.Content!;
		info.CpuArchitecture.Should().Be("x86_64");
		info.NumberOfCores.Should().Be(8);
		info.NumberOfVirtualCores.Should().Be(16);
		info.OsBuild.Should().Be("#1 SMP");
		info.OsName.Should().Be("Linux");
		info.OsNameExtended.Should().Be("Linux");
		info.OsVersion.Should().Be("6.18.33.2");
		info.PhysicalMemoryMB.Should().Be(48091);
		info.TransparentHugePages!.Defrag.Should().Be("madvise");
		info.TransparentHugePages.EffectiveState.Should().Be("ok");
		info.TransparentHugePages.Enabled.Should().Be("madvise");
		var limits = info.Limits!;
		limits.CoreFileSize.Should().Be(0);
		limits.CpuTime.Should().Be(-1);
		limits.DataFileSize.Should().Be(-1);
		limits.DataSegmentSize.Should().Be(-1);
		limits.Nice.Should().Be(0);
		limits.OpenFiles.Should().Be(1048576);
		limits.ResidentMemorySize.Should().Be(-1);
		limits.StackSize.Should().Be(8388608);
		limits.UserProcesses.Should().Be(-1);
		limits.VirtualAddressSpaceSize.Should().Be(-1);
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.SystemInfo.GetAsync(Calls.Token), HttpStatusCode.Forbidden);
}
