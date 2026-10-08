using Splunk.Api.Models.WorkloadManagement;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class WorkloadPoolsTests
{
	// The live test instance has no pools; this content uses the keys the 10.6 reference documents, with the types the
	// categories endpoint returns.
	private const string PoolContent = """
		{"category":"search","cpu_allocated_percent":35,"cpu_shares":358,"cpu_weight":50,"default_category_pool":"1","eai:acl":null,"mem_allocated_percent":70,"mem_limit":"33664","mem_weight":100}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.WorkloadPools.ListAsync(null, Calls.Token), HttpMethod.Get, "/services/workloads/pools", Calls.JsonQuery, null);

	[Fact]
	public async Task CreateAsync_PostsEveryField()
		=> await Calls.AssertAsync(
			c => c.WorkloadPools.CreateAsync(
				new WorkloadPoolCreateRequest { Name = "fast", Category = "search", CpuWeight = 50, MemoryWeight = 100, DefaultCategoryPool = true },
				Calls.Token),
			HttpMethod.Post, "/services/workloads/pools", Calls.JsonQuery,
			"name=fast&category=search&default_category_pool=true&cpu_weight=50&mem_weight=100");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.WorkloadPools.ListAsync(null, Calls.Token), Feed.Of("fast", PoolContent));

		var pool = feed.Entries.Should().ContainSingle().Subject.Content!;
		pool.Category.Should().Be("search");
		pool.CpuAllocatedPercent.Should().Be(35);
		pool.CpuShares.Should().Be(358);
		pool.CpuWeight.Should().Be(50);
		pool.DefaultCategoryPool.Should().BeTrue();
		pool.MemoryAllocatedPercent.Should().Be(70);
		pool.MemoryLimit.Should().Be("33664");
		pool.MemoryWeight.Should().Be(100);
	}

	[Fact]
	public async Task CreateAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(
			c => c.WorkloadPools.CreateAsync(new WorkloadPoolCreateRequest { Name = "x", Category = "nope" }, Calls.Token),
			HttpStatusCode.BadRequest);
}
