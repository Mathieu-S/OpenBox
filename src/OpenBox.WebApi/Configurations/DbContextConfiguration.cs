using Microsoft.EntityFrameworkCore;
using OpenBox.Persistence;

namespace OpenBox.WebApi.Configurations;

/// <summary>
/// Define the configuration about DbContext.
/// </summary>
public static class DbContextConfiguration
{
    /// <summary>
    /// Setup the DbContext configuration in <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> for web applications and services.</param>
    public static void AddDbContextConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<OpenBoxDbContext>(option =>
        {
            option.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
        });
    }

    /// <summary>
    /// Check if the database is reachable.
    /// </summary>
    /// <param name="app">The WebApplication instance this method extends.</param>
    /// <exception cref="TimeoutException">Throw if the database is unreachable.</exception>
    public static void CheckDatabaseConnection(this WebApplication app)
    {
        if (Convert.ToBoolean(app.Configuration["CheckDatabaseConnectionAtStartup"]))
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OpenBoxDbContext>();

            var canConnect = dbContext.Database.CanConnect();
            if (canConnect)
            {
                dbContext.Database.EnsureCreated();
            }
            else
            {
                throw new TimeoutException("The main database is unreachable.");
            }
        }
    }
}