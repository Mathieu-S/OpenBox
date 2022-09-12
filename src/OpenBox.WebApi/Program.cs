using OpenBox.WebApi.Configurations;
using Serilog;
using Serilog.Events;

namespace OpenBox.WebApi;

public class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting web host");
            LaunchWebApplication(args);
            return 0;
        }
        catch (TimeoutException ex)
        {
            Log.Error(ex.Message);
            return 2;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void LaunchWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Host.AddSerilogConfiguration();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.AddDependencyInjectionConfiguration();
        builder.AddDbContextConfiguration();
        builder.AddAuthenticationConfiguration();
        builder.AddAuthorizationConfiguration();

        if (builder.Environment.IsDevelopment())
        {
            builder.AddSwaggerConfiguration();
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error-local-development");
            app.UseSwaggerConfiguration();
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.CheckDatabaseConnection();
        app.Run();
    }
}