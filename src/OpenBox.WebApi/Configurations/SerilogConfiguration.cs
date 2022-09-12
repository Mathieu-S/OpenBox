using Serilog;

namespace OpenBox.WebApi.Configurations;

/// <summary>
/// Define the configuration about Serilog.
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    /// Setup the serilog configuration in <see cref="IHostBuilder"/>.
    /// </summary>
    /// <param name="builder">The host builder to configure.</param>
    public static void AddSerilogConfiguration(this IHostBuilder builder)
    {
        builder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());
    }
}