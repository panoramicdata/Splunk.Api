namespace Splunk.Api.Models.Access;

/// <summary>Changes a ProxySSO configuration (<c>POST admin/ProxySSO-auth/{proxy_name}</c>).</summary>
/// <remarks>
/// The reference lists <c>name</c> as a required parameter here too; the configuration is already named by the path, so
/// it is not sent. Add it to <see cref="SplunkFormRequest.AdditionalParameters"/> if a Splunk version insists.
/// </remarks>
public sealed class ProxySsoConfigurationUpdateRequest : ProxySsoSettings;
