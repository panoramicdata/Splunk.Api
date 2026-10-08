namespace Splunk.Api.Models;

/// <summary>
/// Entry content with no modelled properties: everything Splunk returned is in
/// <see cref="SplunkContent.AdditionalProperties"/>. Used by endpoints whose properties are open-ended, such as
/// configuration file stanzas.
/// </summary>
public sealed class SplunkDynamicContent : SplunkContent;
