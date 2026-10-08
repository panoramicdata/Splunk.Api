<#
.SYNOPSIS
	Starts a disposable Splunk Enterprise in Docker for the Splunk.Api integration tests and stores its settings in the
	integration test project's user secrets.

.DESCRIPTION
	Runs splunk/splunk (the image tag given, default "latest") as container "splunk-api-test" with the management port
	published on localhost, waits until Splunk reports healthy, then sets these user secrets on
	Splunk.Api.IntegrationTest: Splunk:BaseUrl, Splunk:Username, Splunk:Password (a new random password) and
	Splunk:TrustedServerCertificateThumbprint (the SHA-256 thumbprint of the container's self-signed certificate).
	Nothing secret is written to the console or to the repository.

	By starting the container you accept the Splunk General Terms and the license on Splunk's behalf of the image.

.PARAMETER Port
	The host port for Splunk's management port (8089). Default 38089.

.PARAMETER Tag
	The splunk/splunk image tag. Default "latest".

.PARAMETER Replace
	Remove an existing "splunk-api-test" container first.

.EXAMPLE
	./docker/Start-SplunkTestInstance.ps1
	dotnet test --project Splunk.Api.IntegrationTest/Splunk.Api.IntegrationTest.csproj
#>
[CmdletBinding()]
param(
	[int] $Port = 38089,
	[string] $Tag = 'latest',
	[switch] $Replace
)

$ErrorActionPreference = 'Stop'
$containerName = 'splunk-api-test'
$project = Join-Path -Path $PSScriptRoot -ChildPath '..' -AdditionalChildPath 'Splunk.Api.IntegrationTest'

$existing = docker ps -a --filter "name=^$containerName$" --format '{{.Names}}'
if ($existing) {
	if (-not $Replace) {
		throw "Container '$containerName' already exists. Use -Replace to recreate it (its data is lost)."
	}

	docker rm -f $containerName | Out-Null
}

$alphabet = [char[]](48..57 + 65..90 + 97..122)
$password = -join (1..24 | ForEach-Object { $alphabet | Get-Random }) + '!a1'

docker run -d --name $containerName -p "${Port}:8089" `
	-e SPLUNK_START_ARGS=--accept-license `
	-e SPLUNK_GENERAL_TERMS=--accept-sgt-current-at-splunk-com `
	-e "SPLUNK_PASSWORD=$password" `
	"splunk/splunk:$Tag" | Out-Null
if ($LASTEXITCODE -ne 0) {
	throw "docker run failed (exit code $LASTEXITCODE)."
}

Write-Information -MessageData "Waiting for Splunk to become healthy (this takes a few minutes)..." -InformationAction Continue
$deadline = (Get-Date).AddMinutes(10)
do {
	Start-Sleep -Seconds 10
	$health = docker inspect --format '{{.State.Health.Status}}' $containerName
	if ((Get-Date) -gt $deadline) {
		throw "Splunk did not become healthy within 10 minutes (last status: $health)."
	}
} while ($health -ne 'healthy')

$tcp = [System.Net.Sockets.TcpClient]::new('localhost', $Port)
try {
	$ssl = [System.Net.Security.SslStream]::new($tcp.GetStream(), $false, { $true })
	$ssl.AuthenticateAsClient('localhost')
	$certificate = [System.Security.Cryptography.X509Certificates.X509Certificate2]::new($ssl.RemoteCertificate)
	$thumbprint = $certificate.GetCertHashString([System.Security.Cryptography.HashAlgorithmName]::SHA256)
	$ssl.Dispose()
}
finally {
	$tcp.Dispose()
}

dotnet user-secrets set 'Splunk:BaseUrl' "https://localhost:$Port" --project $project | Out-Null
dotnet user-secrets set 'Splunk:Username' 'admin' --project $project | Out-Null
dotnet user-secrets set 'Splunk:Password' $password --project $project | Out-Null
dotnet user-secrets set 'Splunk:TrustedServerCertificateThumbprint' $thumbprint --project $project | Out-Null

Write-Information -MessageData "Splunk is running at https://localhost:$Port and the integration test user secrets are set." -InformationAction Continue
