using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Splunk.Api.Test.Support;

/// <summary>Self-signed certificates created in memory, for certificate validation tests.</summary>
internal static class TestCertificates
{
	public static X509Certificate2 Create(string subject = "CN=splunk.test")
	{
		using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
		var request = new CertificateRequest(subject, key, HashAlgorithmName.SHA256);
		var now = DateTimeOffset.UtcNow;
		return request.CreateSelfSigned(now.AddDays(-1), now.AddDays(1));
	}

	/// <summary>The certificate's SHA-256 thumbprint as upper-case hex.</summary>
	public static string Sha256(X509Certificate2 certificate) => certificate.GetCertHashString(HashAlgorithmName.SHA256);
}
