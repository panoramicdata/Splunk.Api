using Splunk.Api.Test.Support;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Splunk.Api.Test.Core;

/// <summary>Server certificate validation on the transport <see cref="SplunkClient"/> creates for itself.</summary>
public sealed class TransportTests : IDisposable
{
	private readonly X509Certificate2 _pinned = TestCertificates.Create();
	private readonly X509Certificate2 _other = TestCertificates.Create("CN=other.test");
	private readonly HttpRequestMessage _request = new(HttpMethod.Get, "https://splunk.test:8089/");

	public void Dispose()
	{
		_pinned.Dispose();
		_other.Dispose();
		_request.Dispose();
	}

	/// <summary>The transport's TLS validation, invoked as SocketsHttpHandler does: with the request as the sender.</summary>
	private static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool> ValidationOf(SplunkClientOptions options)
	{
		using var handler = SplunkClient.CreateTransport(options);
		var validate = handler.SslOptions.RemoteCertificateValidationCallback!;
		return (request, certificate, chain, errors) => validate(request, certificate, chain, errors);
	}

	private static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool> PinnedCallback(string thumbprint)
		=> ValidationOf(new SplunkClientOptions { TrustedServerCertificateThumbprint = thumbprint });

	[Fact]
	public void NoThumbprintOrCallback_LeavesDefaultValidation()
	{
		using var handler = SplunkClient.CreateTransport(new SplunkClientOptions());

		handler.SslOptions.RemoteCertificateValidationCallback.Should().BeNull();
	}

	[Fact]
	public void PooledConnections_AreDroppedBeforeSplunkdClosesThem()
	{
		using var handler = SplunkClient.CreateTransport(new SplunkClientOptions());

		handler.PooledConnectionIdleTimeout.Should().BeLessThan(TimeSpan.FromSeconds(12), "splunkd's busyKeepAliveIdleTimeout defaults to 12 seconds");
		handler.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(5));
	}

	[Fact]
	public void CustomCallback_TakesPrecedenceOverThumbprint_AndReceivesTheRequest()
	{
		HttpRequestMessage? seen = null;
		var validate = ValidationOf(new SplunkClientOptions
		{
			ServerCertificateValidationCallback = (request, _, _, _) =>
			{
				seen = request;
				return false;
			},
			TrustedServerCertificateThumbprint = TestCertificates.Sha256(_pinned)
		});

		validate(_request, _pinned, null, SslPolicyErrors.RemoteCertificateChainErrors).Should().BeFalse("the custom callback rejects, although the thumbprint would trust it");
		seen.Should().BeSameAs(_request);
	}

	[Theory]
	[InlineData(SslPolicyErrors.RemoteCertificateChainErrors)]
	[InlineData(SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors)]
	public void Pinned_TrustsThePinnedCertificateDespiteErrors(SslPolicyErrors errors)
		=> PinnedCallback(TestCertificates.Sha256(_pinned))(_request, _pinned, null, errors).Should().BeTrue();

	[Fact]
	public void Pinned_MatchesIgnoringCaseAndSeparators()
	{
		var hex = TestCertificates.Sha256(_pinned).ToLowerInvariant();
		var separated = string.Join(':', Enumerable.Range(0, 32).Select(i => hex.Substring(i * 2, 2)));

		PinnedCallback(separated)(_request, _pinned, null, SslPolicyErrors.RemoteCertificateChainErrors).Should().BeTrue();
	}

	[Fact]
	public void Pinned_RejectsAnotherCertificateWithErrors()
		=> PinnedCallback(TestCertificates.Sha256(_pinned))(_request, _other, null, SslPolicyErrors.RemoteCertificateChainErrors).Should().BeFalse();

	[Fact]
	public void Pinned_RejectsAMissingCertificateWithErrors()
		=> PinnedCallback(TestCertificates.Sha256(_pinned))(_request, null, null, SslPolicyErrors.RemoteCertificateNotAvailable).Should().BeFalse();

	[Fact]
	public void Pinned_StillTrustsOtherCertificatesThatPassNormalValidation()
		=> PinnedCallback(TestCertificates.Sha256(_pinned))(_request, _other, null, SslPolicyErrors.None).Should().BeTrue();

	[Fact]
	public void CreateTransport_RequiresOptions()
	{
		var act = () => SplunkClient.CreateTransport(null!);

		act.Should().Throw<ArgumentNullException>();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	public void NormalizeThumbprint_NoneIsNull(string? thumbprint)
		=> SplunkClient.NormalizeThumbprint(thumbprint).Should().BeNull();

	[Theory]
	[InlineData("ab cd")]
	[InlineData("0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef00")]
	[InlineData("DA39A3EE5E6B4B0D3255BFEF95601890AFD80709")]
	public void NormalizeThumbprint_RejectsWrongLength(string thumbprint)
	{
		var act = () => SplunkClient.NormalizeThumbprint(thumbprint);

		act.Should().Throw<ArgumentException>().WithMessage("*SHA-256*");
	}

	[Fact]
	public void NormalizeThumbprint_KeepsOnlyHexDigits()
		=> SplunkClient.NormalizeThumbprint(" 01-23:45 67" + new string('a', 56)).Should().Be("01234567" + new string('a', 56));
}
