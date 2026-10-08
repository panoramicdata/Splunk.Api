namespace Splunk.Api.Test.Core;

public class OptionsTests
{
	private const string Thumbprint = "AB:CD:EF:01:23:45:67:89:ab:cd:ef:01:23:45:67:89:AB:CD:EF:01:23:45:67:89:AB:CD:EF:01:23:45:67:89";

	private static SplunkClientOptions Valid(Action<SplunkClientOptions>? tweak = null)
	{
		var options = new SplunkClientOptions { BaseUrl = "https://splunk.test:8089", Token = "token" };
		tweak?.Invoke(options);
		return options;
	}

	public static TheoryData<string, Action<SplunkClientOptions>> InvalidOptions => new()
	{
		{ "BaseUrl", o => o.BaseUrl = "" },
		{ "BaseUrl", o => o.BaseUrl = "splunk.test:8089/x" },
		{ "BaseUrl", o => o.BaseUrl = "/relative/path" },
		{ "BaseUrl", o => o.BaseUrl = "ftp://splunk.test/" },
		{ "BaseUrl", o => o.BaseUrl = "https://admin:secret@splunk.test:8089/" },
		{ "BaseUrl", o => o.BaseUrl = "https://splunk.test:8089/?a=b" },
		{ "BaseUrl", o => o.BaseUrl = "https://splunk.test:8089/#top" },
		{ "Token", o => o.Username = "admin" },
		{ "Token", o => o.UseBasicAuthentication = true },
		{ "Username", o => o.Token = " " },
		{ "Username", o => (o.Token, o.Username) = (null, "admin") },
		{ "Username", o => (o.Token, o.Password) = (null, "secret") },
		{ "Username", o => (o.Token, o.Username, o.Password) = (null, " ", "secret") },
		{ "thumbprint", o => o.TrustedServerCertificateThumbprint = "ABCD" },
		{ "Owner", o => o.Namespace = new SplunkNamespace(" ", "search") },
		{ "App", o => o.Namespace = new SplunkNamespace("admin", "..") },
		{ "MaxRetries", o => o.MaxRetries = -1 },
		{ "Timeout", o => o.Timeout = TimeSpan.Zero },
		{ "Timeout", o => o.Timeout = TimeSpan.MaxValue },
		{ "RetryBaseDelay", o => o.RetryBaseDelay = TimeSpan.FromTicks(-1) },
		{ "MaxRetryDelay", o => o.MaxRetryDelay = TimeSpan.Zero },
		{ "MaxRetryDelay", o => o.MaxRetryDelay = TimeSpan.FromDays(30) }
	};

	[Theory]
	[MemberData(nameof(InvalidOptions))]
	public void Validate_RejectsInvalidOptions(string parameter, Action<SplunkClientOptions> tweak)
	{
		var options = Valid(tweak);

		var act = options.Validate;

		act.Should().Throw<ArgumentException>().Which.ParamName.Should().Be(parameter);
	}

	public static TheoryData<Action<SplunkClientOptions>> ValidOptions => new()
	{
		o => { },
		o => o.BaseUrl = "http://splunk.test/splunkd/__raw/",
		o => (o.Token, o.Username, o.Password) = (null, "admin", "secret"),
		o => (o.Token, o.Username, o.Password, o.UseBasicAuthentication) = (null, "admin", "secret", true),
		o => o.TrustedServerCertificateThumbprint = Thumbprint,
		o => o.TrustedServerCertificateThumbprint = " ",
		o => o.Namespace = SplunkNamespace.All,
		o => (o.MaxRetries, o.RetryBaseDelay) = (0, TimeSpan.Zero),
		o => (o.Timeout, o.MaxRetryDelay) = (TimeSpan.FromMilliseconds(int.MaxValue), TimeSpan.FromMilliseconds(int.MaxValue))
	};

	[Theory]
	[MemberData(nameof(ValidOptions))]
	public void Validate_AcceptsValidOptions(Action<SplunkClientOptions> tweak)
	{
		var act = Valid(tweak).Validate;

		act.Should().NotThrow();
	}

	[Fact]
	public void Defaults_AreDocumentedValues()
	{
		var options = new SplunkClientOptions();

		options.BaseUrl.Should().BeEmpty();
		options.Timeout.Should().Be(TimeSpan.FromSeconds(100));
		options.MaxRetries.Should().Be(3);
		options.RetryBaseDelay.Should().Be(TimeSpan.FromSeconds(1));
		options.MaxRetryDelay.Should().Be(TimeSpan.FromSeconds(30));
		options.ReadOnly.Should().BeFalse();
		options.UseBasicAuthentication.Should().BeFalse();
		options.Namespace.Should().BeNull();
		options.Logger.Should().BeNull();
		options.ServerCertificateValidationCallback.Should().BeNull();
	}

	[Theory]
	[InlineData("token", null, false, nameof(AuthenticationKind.Token))]
	[InlineData(" ", "admin", false, nameof(AuthenticationKind.Session))]
	[InlineData(null, "admin", false, nameof(AuthenticationKind.Session))]
	[InlineData(null, "admin", true, nameof(AuthenticationKind.Basic))]
	public void AuthenticationKind_FollowsTheCredentials(string? token, string? username, bool basic, string expected)
	{
		var options = new SplunkClientOptions { Token = token, Username = username, UseBasicAuthentication = basic };

		options.AuthenticationKind.ToString().Should().Be(expected);
	}

	[Fact]
	public void ToString_MasksSecrets()
	{
		var options = new SplunkClientOptions
		{
			BaseUrl = "https://splunk.test:8089",
			Token = "the-token",
			Username = "admin",
			Password = "the-password",
			Namespace = SplunkNamespace.Shared("search"),
			ReadOnly = true
		};

		options.ToString().Should().Be(
			"SplunkClientOptions { BaseUrl = https://splunk.test:8089, Token = ***, Username = admin, Password = ***, Namespace = nobody/search, ReadOnly = True }");
	}

	[Fact]
	public void ToString_ShowsMissingSecretsAsNone()
		=> new SplunkClientOptions().ToString().Should().Be(
			"SplunkClientOptions { BaseUrl = , Token = (none), Username = , Password = (none), Namespace = , ReadOnly = False }");

	[Theory]
	[InlineData("https://admin:secret@splunk.test:8089/x?y=1", "https://***@splunk.test:8089/x?y=1")]
	[InlineData("https://admin:p@ss@splunk.test", "https://***@splunk.test")]
	[InlineData("https://splunk.test/a@b", "https://splunk.test/a@b")]
	[InlineData("https://splunk.test", "https://splunk.test")]
	[InlineData("not a url@all", "not a url@all")]
	public void ToString_MasksCredentialsInBaseUrl(string baseUrl, string shown)
	{
		var text = new SplunkClientOptions { BaseUrl = baseUrl }.ToString();

		text.Should().StartWith($"SplunkClientOptions {{ BaseUrl = {shown}, ");
		text.Should().NotContain("secret").And.NotContain("p@ss");
	}
}
