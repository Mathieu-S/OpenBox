using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenBox.Persistence;
using OpenBox.WebApi.Tests.Integration.TestUtils;
using Xunit;

namespace OpenBox.WebApi.Tests.Integration.Fixtures;

public class WebApplicationFixture : IDisposable
{
    public HttpClient Client { get; }

    public WebApplicationFixture()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Overwrite default DbContext
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OpenBoxDbContext>));
                    services.Remove(descriptor!);
                    services.AddDbContext<OpenBoxDbContext>(option => option.UseInMemoryDatabase("TestDatabase"));

                    // Bypass the JWT validation
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = TestAuthHandler.DefaultScheme;
                        options.DefaultScheme = TestAuthHandler.DefaultScheme;
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        TestAuthHandler.DefaultScheme, _ => { });
                });
            });

        // Seed the database
        application.SeedDatabase();

        Client = application.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}

[CollectionDefinition("WebApplication Context")]
public class DatabaseCollection : ICollectionFixture<WebApplicationFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}