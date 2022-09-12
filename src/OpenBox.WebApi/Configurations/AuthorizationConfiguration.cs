using Microsoft.AspNetCore.Authorization;

namespace OpenBox.WebApi.Configurations;

/// <summary>
/// Define the configuration about Authorization service.
/// </summary>
public static class AuthorizationConfiguration
{
    /// <summary>
    /// Setup the authorization configuration in <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> for web applications and services.</param>
    public static void AddAuthorizationConfiguration(this WebApplicationBuilder builder)
    {
        // builder.Services
        //     .AddAuthorization(options =>
        //     {
        //
        //     });
    }
}