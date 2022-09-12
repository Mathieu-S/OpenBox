using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace OpenBox.WebApi.Configurations;

/// <summary>
/// Define the configuration about Swagger service.
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Setup the swagger configuration in <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> for web applications and services.</param>
    public static void AddSwaggerConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApiDocument(document =>
        {
            // Define documentation information
            document.PostProcess = o =>
            {
                o.Info = new OpenApiInfo
                {
                    Title = "Open Box",
                    Description = "A simple ASP.NET Core web API",
                    TermsOfService = "None",
                    Contact = new OpenApiContact
                    {
                        Name = "Mathieu-S",
                        Email = string.Empty,
                        Url = "https://github.com/Mathieu-S"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "GPLv3",
                        Url = "https://www.gnu.org/licenses/gpl-3.0.en.html"
                    }
                };
            };

            // Define securities providers
            document.AddSecurity("JWT Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Description = "Type into the textbox: {your JWT token}."
            });

            document.AddSecurity("OAuth2", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.OAuth2,
                Description = "Keycloack form",
                Flow = OpenApiOAuth2Flow.AccessCode,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "Read access to protected resources" },
                            { "profile", "Write access to protected resources" },
                            { "email", "Write access to protected resources" }
                        },
                        AuthorizationUrl = builder.Configuration["Identity:AuthorizationUrl"],
                        TokenUrl = builder.Configuration["Identity:TokenUrl"]
                    }
                }
            });

            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT Bearer"));
            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("OAuth2"));
        });
    }

    /// <summary>
    /// Use a specific configuration of swagger.
    /// </summary>
    /// <param name="app">The WebApplication instance this method extends.</param>
    public static void UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseOpenApi();
        app.UseSwaggerUi3(settings =>
        {
            settings.OAuth2Client = new OAuth2ClientSettings
            {
                ClientId = app.Configuration["Identity:ClientId"],
                ClientSecret = app.Configuration["Identity:ClientSecret"],
                UsePkceWithAuthorizationCodeGrant = true
            };
        });
    }
}