using Splunk.Api.Models;
using Splunk.Api.Models.Spl2;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class Spl2ModulesTests
{
	private const string PermissionsPath = ModulesPath + "/permissions";

	// From the Splunk 10.6 OpenAPI specification's example, trimmed to one statement.
	private const string DispatchJson = """
		{"module":"$s1 = from _internal | head 3; export {$s1}","namespace":"apps.search","queryParameters":{"s1":{"earliest":"-1h@h","latest":"now","timezone":"Etc/UTC","relativeTimeAnchor":"1737717330","enablePreview":false,"collectFieldSummary":true,"maxTime":3600,"sid":"1682980180.52","jobId":"1682980180.52","runtime":"search.ec","status":"running"}},"wipModules":{}}
		""";

	[Fact]
	public async Task DispatchAsync_SendsTheModuleAndStatementsAsJson()
	{
		var stub = TestClient.Stub(DispatchJson);
		using var client = TestClient.Create(stub);

		await client.Spl2Modules.DispatchAsync(
			new JsonBody<Spl2DispatchRequest>(new Spl2DispatchRequest
			{
				Module = "$s1 = from _internal | head 3; export {$s1}",
				Namespace = "apps.search",
				QueryParameters = new Dictionary<string, Spl2DispatchQuery>
				{
					["s1"] = new()
					{
						Earliest = "-1h@h",
						Latest = "now",
						Timezone = "Etc/UTC",
						RelativeTimeAnchor = "1737717330",
						CollectEventSummary = false,
						CollectFieldSummary = true,
						CollectTimeBuckets = false,
						AdhocSearchLevel = "fast"
					}
				}
			}),
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Post,
			ModulesPath + "/dispatch",
			"?output_mode=json",
			"""{"module":"$s1 = from _internal | head 3; export {$s1}","namespace":"apps.search","queryParameters":{"s1":{"earliest":"-1h@h","latest":"now","timezone":"Etc/UTC","relativeTimeAnchor":"1737717330","collectEventSummary":false,"collectFieldSummary":true,"collectTimeBuckets":false,"adhocSearchLevel":"fast"}}}""");
	}

	[Fact]
	public async Task DispatchAsync_MapsEachStatementsJob()
	{
		using var client = TestClient.Create(TestClient.Stub(DispatchJson));

		var result = await client.Spl2Modules.DispatchAsync(
			new JsonBody<Spl2DispatchRequest>(new Spl2DispatchRequest { Module = "m", QueryParameters = new Dictionary<string, Spl2DispatchQuery> { ["s1"] = new() } }),
			Ct);

		result.Module.Should().StartWith("$s1");
		result.Namespace.Should().Be("apps.search");
		result.WipModules!.Value.EnumerateObject().Should().BeEmpty();
		var job = result.QueryParameters["s1"];
		job.Sid.Should().Be("1682980180.52");
		job.JobId.Should().Be("1682980180.52");
		job.Status.Should().Be("running");
		job.Runtime.Should().Be("search.ec");
		job.Earliest.Should().Be("-1h@h");
		job.Latest.Should().Be("now");
		job.Timezone.Should().Be("Etc/UTC");
		job.MaxTime.Should().Be(3600);
		job.AdditionalProperties["collectFieldSummary"].GetBoolean().Should().BeTrue();
	}

	[Fact]
	public async Task GetPermissionsAsync_SendsTheResourceName()
	{
		var stub = TestClient.Stub("""[{"resourceType":"module","resourceName":"apps.search.my_module","role":"user","operations":["read","execute"]}]""");
		using var client = TestClient.Create(stub);

		var permissions = await client.Spl2Modules.GetPermissionsAsync("apps.search.my_module", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, PermissionsPath, "?resourceName=apps.search.my_module&output_mode=json");
		var permission = permissions.Should().ContainSingle().Subject;
		permission.ResourceType.Should().Be("module");
		permission.ResourceName.Should().Be("apps.search.my_module");
		permission.Role.Should().Be("user");
		permission.Operations.Should().Equal("read", "execute");
	}

	[Fact]
	public async Task UpdatePermissionsAsync_SendsThePermissionsAsJson()
	{
		var stub = TestClient.Stub("""{"code":201}""");
		using var client = TestClient.Create(stub);

		var result = await client.Spl2Modules.UpdatePermissionsAsync(
			"apps.search.my_module",
			new JsonBody<Spl2ModulePermissionsRequest>(new Spl2ModulePermissionsRequest
			{
				ResourceName = "apps.search.my_module",
				Permissions = [new("read", ["admin", "user"]), new("write", ["admin"])]
			}),
			Ct);

		SearchRequestAssert.Sent(
			stub,
			HttpMethod.Put,
			PermissionsPath,
			"?resourceName=apps.search.my_module&output_mode=json",
			"""{"resourceType":"module","resourceName":"apps.search.my_module","permissions":[{"operation":"read","roles":["admin","user"]},{"operation":"write","roles":["admin"]}]}""");
		result.Code.Should().Be(201);
	}
}
