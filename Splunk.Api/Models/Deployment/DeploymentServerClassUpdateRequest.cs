namespace Splunk.Api.Models.Deployment;

/// <summary>Changes to a server class (<c>POST deployment/server/serverclasses/{name}</c>). Only the properties set are sent.</summary>
/// <remarks>Whitelist and blacklist filters are numbered families (<c>whitelist.0</c>, <c>whitelist.1</c>, <c>blacklist.0</c>, ...; ordinals start at 0 and are consecutive): set them in <see cref="SplunkFormRequest.AdditionalParameters"/>.</remarks>
public sealed class DeploymentServerClassUpdateRequest : DeploymentServerClassSettings
{
}
