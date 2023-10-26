// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Microsoft.AspNetCore.Authentication.OpenIdConnect;

public class StateDataFormattedContext : BaseContext<OpenIdConnectOptions>
{
    /// <summary>
    /// Gets or sets the <see cref="OpenIdConnectMessage"/>.
    /// </summary>
    public OpenIdConnectMessage ProtocolMessage { get; init; }

    /// <summary>
    /// If true, will skip any default logic for this redirect.
    /// </summary>
    public bool Handled { get; private set; }

    /// <summary>
    /// Skips any default logic for this redirect.
    /// </summary>
    public void HandleResponse() => Handled = true;

    public StateDataFormattedContext(
        HttpContext context,
        AuthenticationScheme scheme,
        OpenIdConnectOptions options,
        OpenIdConnectMessage message) : base(context, scheme, options)
    {
        ProtocolMessage = message;
    }
}
