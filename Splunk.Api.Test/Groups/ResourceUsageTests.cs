using Splunk.Api.Models;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ResourceUsageTests
{
	private const string Path = "/services/server/status/resource-usage";

	// Captured from Splunk 10.6.0.5 (GET server/status/resource-usage/hostwide); GUID replaced.
	private const string HostContent = """
		{
			"cpu_arch": "x86_64", "cpu_count": "8", "cpu_idle_pct": "96.73", "cpu_system_pct": "1.23", "cpu_user_pct": "2.03", "eai:acl": null,
			"forks": "591134", "instance_guid": "00000000-0000-0000-0000-000000000001", "mem": "48091.605", "mem_used": "6099.367",
			"normalized_load_avg_1min": "0.02", "os_build": "#1 SMP", "os_name": "Linux", "os_name_ext": "Linux", "os_version": "6.18.33.2",
			"pg_paged_out": "38889521", "pg_swapped_out": "0", "runnable_process_count": "2", "splunk_version": "10.6.0.5",
			"swap": "12288.000", "swap_used": "0.000", "virtual_cpu_count": "16"
		}
		""";

	// Captured from Splunk 10.6.0.5 (iostats and splunk-processes, first entry each).
	private const string IoContent = """
		{"avg_service_ms":"0.500","avg_total_ms":"0.750","cpu_pct":"0.10","device":"sda","eai:acl":null,"interval":"60","reads_kb_ps":"1.000","reads_ps":"2.000","writes_kb_ps":"3.000","writes_ps":"4.000"}
		""";

	private const string ProcessContent = """
		{"args":"-p 8089 start","eai:acl":null,"elapsed":"2313.6800","fd_used":"225","mem_used":"313.688","normalized_pct_cpu":"1.74","page_faults":"0","pct_cpu":"13.90","pct_memory":"0.65","pid":"1703","process":"splunkd","process_type":"splunkd_server","read_mb":"0.012","status":"W","t_count":"89","written_mb":"1188.965"}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ResourceUsage.ListAsync(null, Calls.Token), HttpMethod.Get, Path, Calls.JsonQuery, null);

	[Fact]
	public async Task GetHostwideAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ResourceUsage.GetHostwideAsync(Calls.Token), HttpMethod.Get, Path + "/hostwide", Calls.JsonQuery, null);

	[Fact]
	public async Task ListIoStatsAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ResourceUsage.ListIoStatsAsync(new ListOptions { Count = 0 }, Calls.Token), HttpMethod.Get, Path + "/iostats", "?count=0&output_mode=json", null);

	[Fact]
	public async Task ListSplunkProcessesAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ResourceUsage.ListSplunkProcessesAsync(null, Calls.Token), HttpMethod.Get, Path + "/splunk-processes", Calls.JsonQuery, null);

	[Fact]
	public async Task GetHostwideAsync_MapsEveryField()
	{
		var feed = await Calls.MapAsync(c => c.ResourceUsage.GetHostwideAsync(Calls.Token), Feed.Of("result", HostContent));

		var host = feed.Entries.Should().ContainSingle().Subject.Content!;
		host.CpuArchitecture.Should().Be("x86_64");
		host.CpuCount.Should().Be(8);
		host.CpuIdlePercent.Should().Be(96.73);
		host.CpuSystemPercent.Should().Be(1.23);
		host.CpuUserPercent.Should().Be(2.03);
		host.Forks.Should().Be(591134);
		host.InstanceGuid.Should().Be("00000000-0000-0000-0000-000000000001");
		host.MemoryMB.Should().Be(48091.605);
		host.MemoryUsedMB.Should().Be(6099.367);
		host.NormalizedLoadAverage1Min.Should().Be(0.02);
		host.OsBuild.Should().Be("#1 SMP");
		host.OsName.Should().Be("Linux");
		host.OsNameExtended.Should().Be("Linux");
		host.OsVersion.Should().Be("6.18.33.2");
		host.PagesPagedOut.Should().Be(38889521);
		host.PagesSwappedOut.Should().Be(0);
		host.RunnableProcessCount.Should().Be(2);
		host.SplunkVersion.Should().Be("10.6.0.5");
		host.SwapMB.Should().Be(12288);
		host.SwapUsedMB.Should().Be(0);
		host.VirtualCpuCount.Should().Be(16);
	}

	[Fact]
	public async Task ListIoStatsAsync_MapsEveryField()
	{
		var feed = await Calls.MapAsync(c => c.ResourceUsage.ListIoStatsAsync(null, Calls.Token), Feed.Of("0", IoContent));

		var io = feed.Entries.Should().ContainSingle().Subject.Content!;
		io.AverageServiceMs.Should().Be(0.5);
		io.AverageTotalMs.Should().Be(0.75);
		io.CpuPercent.Should().Be(0.1);
		io.Device.Should().Be("sda");
		io.Interval.Should().Be(60);
		io.ReadKBPerSecond.Should().Be(1);
		io.ReadsPerSecond.Should().Be(2);
		io.WriteKBPerSecond.Should().Be(3);
		io.WritesPerSecond.Should().Be(4);
	}

	[Fact]
	public async Task ListSplunkProcessesAsync_MapsEveryField()
	{
		var feed = await Calls.MapAsync(c => c.ResourceUsage.ListSplunkProcessesAsync(null, Calls.Token), Feed.Of("0", ProcessContent));

		var process = feed.Entries.Should().ContainSingle().Subject.Content!;
		process.Arguments.Should().Be("-p 8089 start");
		process.ElapsedSeconds.Should().Be(2313.68);
		process.FileDescriptorsUsed.Should().Be(225);
		process.MemoryUsedMB.Should().Be(313.688);
		process.NormalizedCpuPercent.Should().Be(1.74);
		process.PageFaults.Should().Be(0);
		process.CpuPercent.Should().Be(13.9);
		process.MemoryPercent.Should().Be(0.65);
		process.ProcessId.Should().Be(1703);
		process.Process.Should().Be("splunkd");
		process.ProcessType.Should().Be("splunkd_server");
		process.ReadMB.Should().Be(0.012);
		process.Status.Should().Be("W");
		process.ThreadCount.Should().Be(89);
		process.WrittenMB.Should().Be(1188.965);
	}

	[Fact]
	public async Task GetHostwideAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.ResourceUsage.GetHostwideAsync(Calls.Token), HttpStatusCode.Forbidden);
}
