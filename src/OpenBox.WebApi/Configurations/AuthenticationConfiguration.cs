using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OpenBox.WebApi.Configurations;

/// <summary>
/// Define the configuration about Authentication service.
/// </summary>
public static class AuthenticationConfiguration
{
    /// <summary>
    /// Setup the authentication configuration in <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> for web applications and services.</param>
    public static void AddAuthenticationConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(options =>
                {
                    options.Audience = builder.Configuration["Identity:Audience"];
                    options.MetadataAddress = builder.Configuration["Identity:MetadataAddress"];
                    options.SaveToken = true;
                    if (builder.Environment.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                    }
                }
            );
    }
}