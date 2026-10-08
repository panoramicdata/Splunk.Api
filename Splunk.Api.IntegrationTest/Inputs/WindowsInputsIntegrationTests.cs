using Splunk.Api.Models.Inputs;
using System.Net;

namespace Splunk.Api.IntegrationTest.Inputs;

/// <summary>Windows-only inputs: the Linux test instance answers every operation with 404.</summary>
[Collection(SplunkTestGroup.Name)]
public class WindowsInputsIntegrationTests(SplunkFixture fixture)
{
	private const string Name = "splunk_api_it_windows";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private static readonly Dictionary<string, Func<SplunkClient, Task>> Calls = new()
	{
		["ad.list"] = client => client.ActiveDirectoryInputs.ListAsync(null, Ct),
		["ad.create"] = client => client.ActiveDirectoryInputs.CreateAsync(new ActiveDirectoryInputCreateRequest { Name = Name, MonitorSubtree = true }, Ct),
		["ad.get"] = client => client.ActiveDirectoryInputs.GetAsync(Name, Ct),
		["ad.update"] = client => client.ActiveDirectoryInputs.UpdateAsync(Name, new ActiveDirectoryInputUpdateRequest { MonitorSubtree = false }, Ct),
		["ad.delete"] = client => client.ActiveDirectoryInputs.DeleteAsync(Name, Ct),
		["registry.list"] = client => client.RegistryInputs.ListAsync(null, Ct),
		["registry.create"] = client => client.RegistryInputs.CreateAsync(new RegistryInputCreateRequest { Name = Name, Baseline = false, Hive = "HKLM", Process = ".*", Type = "set" }, Ct),
		["registry.get"] = client => client.RegistryInputs.GetAsync(Name, Ct),
		["registry.update"] = client => client.RegistryInputs.UpdateAsync(Name, new RegistryInputUpdateRequest { Baseline = false, Hive = "HKLM", Process = ".*", Type = "set" }, Ct),
		["registry.delete"] = client => client.RegistryInputs.DeleteAsync(Name, Ct),
		["eventlog.list"] = client => client.WindowsEventLogInputs.ListAsync(null, Ct),
		["eventlog.create"] = client => client.WindowsEventLogInputs.CreateAsync(new WindowsEventLogInputCreateRequest { Name = Name, LookupHost = "localhost" }, Ct),
		["eventlog.get"] = client => client.WindowsEventLogInputs.GetAsync(Name, null, Ct),
		["eventlog.update"] = client => client.WindowsEventLogInputs.UpdateAsync(Name, new WindowsEventLogInputUpdateRequest { LookupHost = "localhost" }, Ct),
		["eventlog.delete"] = client => client.WindowsEventLogInputs.DeleteAsync(Name, Ct),
		["wmi.list"] = client => client.WmiInputs.ListAsync(null, Ct),
		["wmi.create"] = client => client.WmiInputs.CreateAsync(new WmiInputCreateRequest { Name = Name, Classes = "Win32_Processor", Interval = 60, LookupHost = "localhost" }, Ct),
		["wmi.get"] = client => client.WmiInputs.GetAsync(Name, Ct),
		["wmi.update"] = client => client.WmiInputs.UpdateAsync(Name, new WmiInputUpdateRequest { Classes = "Win32_Processor", Interval = 60, LookupHost = "localhost" }, Ct),
		["wmi.delete"] = client => client.WmiInputs.DeleteAsync(Name, Ct),
		["perfmon.list"] = client => client.PerfmonInputs.ListAsync(null, Ct),
		["perfmon.create"] = client => client.PerfmonInputs.CreateAsync(new PerfmonInputCreateRequest { Name = Name, PerformanceObject = "Memory" }, Ct),
		["perfmon.get"] = client => client.PerfmonInputs.GetAsync(Name, Ct),
		["perfmon.update"] = client => client.PerfmonInputs.UpdateAsync(Name, new PerfmonInputUpdateRequest { Interval = 60 }, Ct),
		["perfmon.delete"] = client => client.PerfmonInputs.DeleteAsync(Name, Ct)
	};

	public static TheoryData<string> Operations => [.. Calls.Keys];

	[Theory]
	[MemberData(nameof(Operations))]
	public async Task Operation_OnLinux_IsNotFound(string operation)
	{
		var act = () => Calls[operation](fixture.Client);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
