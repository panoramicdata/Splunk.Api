using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class ActiveDirectoryInputsTests
{
	private const string Path = "/services/data/inputs/ad";

	// Shaped as the reference's example (a Linux test instance has no Active Directory inputs).
	private static readonly string AdJson = InputsTestKit.Feed("corp", """
		{
			"baseline": "1", "disabled": "1", "eai:acl": null, "index": "default", "monitorSubtree": "1", "printSchema": "0",
			"startingNode": "OU=Staff,DC=example,DC=com", "targetDc": "dc01.example.com"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ActiveDirectoryInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ActiveDirectoryInputs.CreateAsync(
			new ActiveDirectoryInputCreateRequest
			{
				Name = "corp",
				MonitorSubtree = true,
				Baseline = false,
				Host = "dc01",
				Index = "ad",
				PrintSchema = false,
				Source = "ad",
				Sourcetype = "ActiveDirectory",
				StartingNode = "DC=example,DC=com",
				TargetDomainController = "dc01.example.com"
			},
			ct)))
			.ShouldBePost(Path, "name=corp&monitorSubtree=true&baseline=false&host=dc01&index=ad&printSchema=false&source=ad&sourcetype=ActiveDirectory&startingNode=DC%3Dexample%2CDC%3Dcom&targetDc=dc01.example.com");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ActiveDirectoryInputs.GetAsync("corp", ct))).ShouldBeGet(Path + "/corp");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ActiveDirectoryInputs.UpdateAsync("corp", new ActiveDirectoryInputUpdateRequest { MonitorSubtree = false }, ct)))
			.ShouldBePost(Path + "/corp", "monitorSubtree=false");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ActiveDirectoryInputs.DeleteAsync("corp", ct))).ShouldBeDelete(Path + "/corp");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.ActiveDirectoryInputs.GetAsync("corp", ct), AdJson)).Content!;

		input.Baseline.Should().BeTrue();
		input.Disabled.Should().BeTrue();
		input.MonitorSubtree.Should().BeTrue();
		input.PrintSchema.Should().BeFalse();
		input.StartingNode.Should().Be("OU=Staff,DC=example,DC=com");
		input.TargetDomainController.Should().Be("dc01.example.com");
	}

	[Fact]
	public Task ListAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.ActiveDirectoryInputs.ListAsync(null, ct));
}
