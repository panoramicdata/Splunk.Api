using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ServerInfoTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (docker splunk/splunk), trimmed; host names and GUIDs replaced.
	private const string ServerInfoJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/server/info",
			"updated": "2026-10-08T14:18:12+01:00",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [
				{
					"name": "server-info",
					"id": "https://splunk.test:8089/services/server/info/server-info",
					"updated": "1970-01-01T00:00:00+00:00",
					"links": { "alternate": "/services/server/info/server-info", "list": "/services/server/info/server-info" },
					"author": "system",
					"acl": {
						"app": "", "can_list": true, "can_write": true, "modifiable": false, "owner": "system",
						"perms": { "read": ["*"], "write": [] }, "removable": false, "sharing": "system"
					},
					"content": {
						"activeLicenseGroup": "Trial",
						"build": "86587d4e3b27",
						"cpu_arch": "x86_64",
						"eai:acl": null,
						"fips_mode": false,
						"guid": "00000000-0000-0000-0000-000000000001",
						"health_info": "green",
						"host": "splunk01",
						"host_fqdn": "splunk01.example.com",
						"isForwarding": false,
						"isFree": false,
						"isTrial": true,
						"kvStoreStatus": "ready",
						"licenseState": "OK",
						"mode": "normal",
						"numberOfCores": 8,
						"numberOfVirtualCores": 16,
						"os_name": "Linux",
						"os_version": "6.18.33.2",
						"physicalMemoryMB": 48091,
						"product_type": "enterprise",
						"serverName": "splunk01",
						"server_roles": ["indexer", "license_manager", "kv_store"],
						"shutting_down": "0",
						"startup_time": 1791465085,
						"version": "10.6.0.5",
						"versionControlEnabled": true
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task GetAsync_SendsGetToServerInfoAsJson()
	{
		var stub = TestClient.Stub(ServerInfoJson);
		using var client = TestClient.Create(stub);

		await client.ServerInfo.GetAsync(TestContext.Current.CancellationToken);

		stub.Calls.Should().ContainSingle();
		stub.Calls[0].Method.Should().Be(HttpMethod.Get);
		stub.Calls[0].Uri.AbsolutePath.Should().Be("/services/server/info");
		stub.Calls[0].Uri.Query.Should().Be("?output_mode=json");
		stub.Calls[0].Body.Should().BeNull();
	}

	[Fact]
	public async Task GetAsync_MapsTheEnvelopeAndEveryModelledField()
	{
		using var client = TestClient.Create(TestClient.Stub(ServerInfoJson));

		var feed = await client.ServerInfo.GetAsync(TestContext.Current.CancellationToken);

		feed.Generator!.Version.Should().Be("10.6.0.5");
		feed.Paging!.Total.Should().Be(1);
		feed.Messages.Should().BeEmpty();
		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("server-info");
		entry.Author.Should().Be("system");
		entry.Acl!.Sharing.Should().Be("system");
		entry.Acl.Permissions!.Read.Should().Equal("*");
		var info = entry.Content!;
		info.Version.Should().Be("10.6.0.5");
		info.Build.Should().Be("86587d4e3b27");
		info.ServerName.Should().Be("splunk01");
		info.Host.Should().Be("splunk01");
		info.HostFqdn.Should().Be("splunk01.example.com");
		info.ServerGuid.Should().Be("00000000-0000-0000-0000-000000000001");
		info.ProductType.Should().Be("enterprise");
		info.Mode.Should().Be("normal");
		info.ServerRoles.Should().Equal("indexer", "license_manager", "kv_store");
		info.HealthInfo.Should().Be("green");
		info.KvStoreStatus.Should().Be("ready");
		info.ActiveLicenseGroup.Should().Be("Trial");
		info.LicenseState.Should().Be("OK");
		info.IsTrial.Should().BeTrue();
		info.IsFree.Should().BeFalse();
		info.IsForwarding.Should().BeFalse();
		info.FipsMode.Should().BeFalse();
		info.ShuttingDown.Should().BeFalse();
		info.OsName.Should().Be("Linux");
		info.OsVersion.Should().Be("6.18.33.2");
		info.CpuArchitecture.Should().Be("x86_64");
		info.NumberOfCores.Should().Be(8);
		info.NumberOfVirtualCores.Should().Be(16);
		info.PhysicalMemoryMB.Should().Be(48091);
		info.StartupTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791465085));
		info.EaiAcl.Should().BeNull();
		info.AdditionalProperties["versionControlEnabled"].GetBoolean().Should().BeTrue();
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> TestClient.ShouldFailAsync(
			(client, ct) => client.ServerInfo.GetAsync(ct),
			HttpStatusCode.Unauthorized,
			"""{"messages":[{"type":"ERROR","text":"Unauthorized"}]}""",
			"Unauthorized");
}
