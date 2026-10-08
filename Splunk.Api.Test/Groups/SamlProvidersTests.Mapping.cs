using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class SamlProvidersTests
{
	// Shaped after the reference (enabling SAML on the shared test instance would change every login).
	private const string SamlJson = """
		{
			"entry": [
				{
					"name": "saml",
					"content": {
						"entityId": "splunk01",
						"idpSSOUrl": "https://idp.example.com/sso",
						"idpSLOUrl": "https://idp.example.com/slo",
						"idpAttributeQueryUrl": "https://idp.example.com/aq",
						"idpCertPath": "/opt/splunk/etc/auth/idpCerts",
						"assertionConsumerServiceUrl": "https://splunk01:8000/saml/acs",
						"singleLogoutServiceUrl": "https://splunk01:8000/saml/logout",
						"fqdn": "https://splunk.example.com",
						"redirectPort": "443",
						"redirectAfterLogoutToUrl": "https://example.com/bye",
						"defaultRoleIfMissing": "user",
						"signAuthnRequest": "1",
						"signedAssertion": "1",
						"signatureAlgorithm": "RSA-SHA256",
						"nameIdFormat": "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified",
						"ssoBinding": "HTTPPost",
						"sloBinding": "HTTPRedirect",
						"attributeAliasMail": "email",
						"attributeAliasRealName": "realName",
						"attributeAliasRole": "role",
						"attributeQueryRequestSigned": "0",
						"attributeQueryResponseSigned": "0",
						"attributeQuerySoapUsername": "soapuser",
						"attributeQueryTTL": "3600",
						"errorUrl": "https://example.com/help",
						"errorUrlLabel": "Click here to resolve SAML error.",
						"sslVerifyServerCert": "1",
						"sslVersions": "tls1.2",
						"caCertFile": "/opt/splunk/etc/auth/cacert.pem",
						"spCertPath": "/opt/splunk/etc/auth/server.pem",
						"maxAttributeQueryThreads": "2",
						"maxAttributeQueryQueueSize": "100",
						"disabled": "1"
					}
				}
			]
		}
		""";

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.SamlProviders.GetAsync("saml", ct), SamlJson);

		var saml = feed.Entries.Should().ContainSingle().Subject.Content!;
		saml.EntityId.Should().Be("splunk01");
		saml.IdpSsoUrl.Should().Be("https://idp.example.com/sso");
		saml.IdpSloUrl.Should().Be("https://idp.example.com/slo");
		saml.IdpAttributeQueryUrl.Should().Be("https://idp.example.com/aq");
		saml.IdpCertPath.Should().Be("/opt/splunk/etc/auth/idpCerts");
		saml.AssertionConsumerServiceUrl.Should().Be("https://splunk01:8000/saml/acs");
		saml.SingleLogoutServiceUrl.Should().Be("https://splunk01:8000/saml/logout");
		saml.Fqdn.Should().Be("https://splunk.example.com");
		saml.RedirectPort.Should().Be(443);
		saml.RedirectAfterLogoutToUrl.Should().Be("https://example.com/bye");
		saml.DefaultRoleIfMissing.Should().Be("user");
		saml.SignAuthnRequest.Should().BeTrue();
		saml.SignedAssertion.Should().BeTrue();
		saml.SignatureAlgorithm.Should().Be("RSA-SHA256");
		saml.NameIdFormat.Should().Be("urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified");
		saml.SsoBinding.Should().Be("HTTPPost");
		saml.SloBinding.Should().Be("HTTPRedirect");
		saml.AttributeAliasMail.Should().Be("email");
		saml.AttributeAliasRealName.Should().Be("realName");
		saml.AttributeAliasRole.Should().Be("role");
		saml.AttributeQueryRequestSigned.Should().BeFalse();
		saml.AttributeQueryResponseSigned.Should().BeFalse();
		saml.AttributeQuerySoapUsername.Should().Be("soapuser");
		saml.AttributeQueryTtl.Should().Be(3600);
		saml.ErrorUrl.Should().Be("https://example.com/help");
		saml.ErrorUrlLabel.Should().Be("Click here to resolve SAML error.");
		saml.SslVerifyServerCert.Should().BeTrue();
		saml.SslVersions.Should().Be("tls1.2");
		saml.CaCertFile.Should().Be("/opt/splunk/etc/auth/cacert.pem");
		saml.SpCertPath.Should().Be("/opt/splunk/etc/auth/server.pem");
		saml.MaxAttributeQueryThreads.Should().Be(2);
		saml.MaxAttributeQueryQueueSize.Should().Be(100);
		saml.Disabled.Should().BeTrue();
	}
}
