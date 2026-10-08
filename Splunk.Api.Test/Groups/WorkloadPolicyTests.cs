using Splunk.Api.Models.WorkloadManagement;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class WorkloadPolicyTests
{
	private const string Path = "/services/workloads/policy/search_admission_control";

	[Fact]
	public async Task GetSearchAdmissionControlAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.WorkloadPolicy.GetSearchAdmissionControlAsync(Calls.Token), HttpMethod.Get, Path, Calls.JsonQuery, null);

	[Fact]
	public async Task UpdateSearchAdmissionControlAsync_PostsTheFlag()
		=> await Calls.AssertAsync(
			c => c.WorkloadPolicy.UpdateSearchAdmissionControlAsync(new SearchAdmissionControlUpdateRequest { AdmissionRulesEnabled = true }, Calls.Token),
			HttpMethod.Post, Path, Calls.JsonQuery, "admission_rules_enabled=true");

	[Fact]
	public async Task GetSearchAdmissionControlAsync_MapsTheFlag()
	{
		// Captured from Splunk 10.6.0.5.
		var feed = await Calls.MapAsync(
			c => c.WorkloadPolicy.GetSearchAdmissionControlAsync(Calls.Token),
			Feed.Of("search_admission_control", """{"admission_rules_enabled":false,"eai:acl":null}"""));

		feed.Entries.Should().ContainSingle().Which.Content!.AdmissionRulesEnabled.Should().BeFalse();
	}

	[Fact]
	public async Task UpdateSearchAdmissionControlAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(
			c => c.WorkloadPolicy.UpdateSearchAdmissionControlAsync(new SearchAdmissionControlUpdateRequest { AdmissionRulesEnabled = false }, Calls.Token),
			HttpStatusCode.Forbidden);
}
