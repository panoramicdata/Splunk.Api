using Splunk.Api.Models;
using Splunk.Api.Models.FederatedSearch;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class FederatedProvidersTests
{
	private const string Path = "/services/data/federated/provider";

	// Captured from Splunk Enterprise 10.6.0.5 after creating a provider; host and account replaced.
	private const string SplunkProviderContent = """
		{
			"appContext": "search",
			"connectivityStatus": "unknown",
			"disabled": false,
			"eai:acl": null,
			"fedSrchIndexesAllowed": "*",
			"hostPort": "192.0.2.1:8089",
			"mode": "standard",
			"serviceAccount": "svc",
			"type": "splunk",
			"useAppContextFromSearch": "0",
			"useFSHKnowledgeObjects": "0"
		}
		""";

	// From the reference's Amazon S3 provider example (Splunk Cloud Platform only).
	private const string S3ProviderContent = """
		{
			"aws_account_id": "123456789012",
			"aws_glue_tables_allowlist": "table_1,table_2",
			"aws_kms_keys_arn_allowlist": "arn:aws:kms:us-east-1:123456789012:key/b1e51ce6",
			"aws_region": "us-west-2",
			"aws_s3_paths_allowlist": "s3://bucket1,s3://bucket2/folder2/",
			"data_catalog": "glue:arn:aws:glue:us-west-2:123456789012:catalog",
			"database": "database_1",
			"disabled": "1",
			"type": "aws_s3"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGetWithOptions()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.ListAsync(new ListOptions { Count = 0 }, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path, "?count=0&output_mode=json");

	[Fact]
	public async Task CreateAsync_SendsPostWithTheProvider()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.CreateAsync(
			new FederatedProviderCreateRequest
			{
				Name = "remote1",
				Type = FederatedProviderType.Splunk,
				Mode = FederatedProviderMode.Standard,
				HostPort = "192.0.2.1:8089",
				ServiceAccount = "svc",
				Password = "p&ss",
				AppContext = "search",
				AwsAccountId = "123456789012",
				AwsGlueTablesAllowlist = "t1",
				AwsKmsKeysArnAllowlist = "k1",
				AwsS3PathsAllowlist = "s3://b",
				Database = "db"
			},
			ct)))
			.ShouldBeProbed(HttpMethod.Post, Path, body:
				"name=remote1&type=splunk&mode=standard&hostPort=192.0.2.1%3A8089&serviceAccount=svc&password=p%26ss&appContext=search"
				+ "&aws_account_id=123456789012&aws_glue_tables_allowlist=t1&aws_kms_keys_arn_allowlist=k1&aws_s3_paths_allowlist=s3%3A%2F%2Fb&database=db");

	[Fact]
	public async Task DisableAllAsync_SendsPostWithTheType()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.DisableAllAsync(new FederatedProviderBatchDisableRequest { Type = FederatedProviderType.AwsS3 }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/turnOffProvidersInBatch", body: "type=aws_s3");

	[Fact]
	public async Task DisableAllAsync_WithoutType_SendsAnEmptyForm()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.DisableAllAsync(new FederatedProviderBatchDisableRequest(), ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/turnOffProvidersInBatch", body: string.Empty);

	[Fact]
	public async Task GetAsync_SendsGetWithTheName()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.GetAsync("remote 1", ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/remote%201");

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSetFields()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.UpdateAsync(
			"remote1",
			new FederatedProviderUpdateRequest
			{
				AppContext = "search",
				HostPort = "h:8089",
				ServiceAccount = "svc2",
				Password = "pw",
				FederatedIndexesAllowed = "idx*",
				UseAppContextFromSearch = true,
				AwsAccountId = "1",
				AwsGlueTablesAllowlist = "t",
				AwsKmsKeysArnAllowlist = "k",
				AwsS3PathsAllowlist = "p"
			},
			ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/remote1", body:
				"appContext=search&hostPort=h%3A8089&serviceAccount=svc2&password=pw&fedSrchIndexesAllowed=idx%2A&useAppContextFromSearch=true"
				+ "&aws_account_id=1&aws_glue_tables_allowlist=t&aws_kms_keys_arn_allowlist=k&aws_s3_paths_allowlist=p");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.DeleteAsync("remote1", ct)))
			.ShouldBeProbed(HttpMethod.Delete, $"{Path}/remote1");

	[Fact]
	public async Task DisableAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.DisableAsync("remote1", ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/remote1/disable");

	[Fact]
	public async Task EnableAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedProviders.EnableAsync("remote1", ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/remote1/enable");

	[Fact]
	public async Task GetAsync_MapsASplunkProvider()
	{
		var provider = await RequestProbe.ReadContentAsync((c, ct) => c.FederatedProviders.GetAsync("remote1", ct), "remote1", SplunkProviderContent);

		provider.Type.Should().Be(FederatedProviderType.Splunk);
		provider.Mode.Should().Be(FederatedProviderMode.Standard);
		provider.AppContext.Should().Be("search");
		provider.HostPort.Should().Be("192.0.2.1:8089");
		provider.ServiceAccount.Should().Be("svc");
		provider.UseFshKnowledgeObjects.Should().BeFalse();
		provider.UseAppContextFromSearch.Should().BeFalse();
		provider.FederatedIndexesAllowed.Should().Be("*");
		provider.ConnectivityStatus.Should().Be("unknown");
		provider.Disabled.Should().BeFalse();
	}

	[Fact]
	public async Task GetAsync_MapsAnAmazonS3Provider()
	{
		var provider = await RequestProbe.ReadContentAsync((c, ct) => c.FederatedProviders.GetAsync("s3", ct), "s3", S3ProviderContent);

		provider.Type.Should().Be(FederatedProviderType.AwsS3);
		provider.Mode.Should().Be(FederatedProviderMode.Unknown);
		provider.AwsAccountId.Should().Be("123456789012");
		provider.AwsGlueTablesAllowlist.Should().Be("table_1,table_2");
		provider.AwsKmsKeysArnAllowlist.Should().Be("arn:aws:kms:us-east-1:123456789012:key/b1e51ce6");
		provider.AwsRegion.Should().Be("us-west-2");
		provider.AwsS3PathsAllowlist.Should().Be("s3://bucket1,s3://bucket2/folder2/");
		provider.DataCatalog.Should().Be("glue:arn:aws:glue:us-west-2:123456789012:catalog");
		provider.Database.Should().Be("database_1");
		provider.Disabled.Should().BeTrue();
	}

	[Fact]
	public Task GetAsync_Missing_RaisesNotFound()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.FederatedProviders.GetAsync("x", ct),
			HttpStatusCode.NotFound,
			"""{"messages":[{"type":"ERROR","text":"Could not find object id=provider://x"}]}""",
			"Could not find object id=provider://x");
}
