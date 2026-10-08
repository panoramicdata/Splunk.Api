using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SamlMetadataTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET admin/SAML-sp-metadata), trimmed; certificate shortened, host replaced.
	private const string SpMetadataJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/admin/SAML-sp-metadata",
			"entry": [
				{
					"name": "spMetadata",
					"author": "system",
					"acl": { "app": "", "owner": "system", "sharing": "system", "perms": { "read": ["admin"], "write": ["admin"] } },
					"content": {
						"eai:acl": null,
						"spMetadata": "<md:EntityDescriptor entityID=\"splunkEntityId\"><md:SPSSODescriptor><md:AssertionConsumerService Location=\"http://splunk01:8000/saml/acs\" index=\"0\"><\/md:AssertionConsumerService><\/md:SPSSODescriptor><\/md:EntityDescriptor>"
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	private const string IdpMetadataJson = """
		{ "entry": [ { "name": "idpMetadata", "content": { "idpMetadataPayload": "<md:EntityDescriptor entityID=\"https://idp.example.com\"/>" } } ] }
		""";

	[Fact]
	public async Task GetIdentityProviderMetadataAsync_SendsTheFile()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlMetadata.GetIdentityProviderMetadataAsync(new SamlIdpMetadataOptions { IdpMetadataFile = "/opt/idp.xml" }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/SAML-idp-metadata", query: "?idpMetadataFile=%2Fopt%2Fidp.xml&output_mode=json");

	[Fact]
	public async Task GetServiceProviderMetadataAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlMetadata.GetServiceProviderMetadataAsync(ct), SpMetadataJson))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/SAML-sp-metadata");

	[Fact]
	public async Task ReplicateCertificatesAsync_Posts()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlMetadata.ReplicateCertificatesAsync(ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/admin/replicate-SAML-certs");

	[Fact]
	public async Task ServiceProviderMetadata_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.SamlMetadata.GetServiceProviderMetadataAsync(ct), SpMetadataJson);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("spMetadata");
		entry.Content!.SpMetadata.Should().StartWith("<md:EntityDescriptor entityID=\"splunkEntityId\">").And.EndWith("</md:EntityDescriptor>");
	}

	[Fact]
	public async Task IdentityProviderMetadata_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.SamlMetadata.GetIdentityProviderMetadataAsync(null, ct), IdpMetadataJson);

		feed.Entries.Should().ContainSingle().Subject.Content!.IdpMetadataPayload.Should().Be("<md:EntityDescriptor entityID=\"https://idp.example.com\"/>");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.SamlMetadata.GetIdentityProviderMetadataAsync(null, ct), HttpStatusCode.BadRequest);
}
