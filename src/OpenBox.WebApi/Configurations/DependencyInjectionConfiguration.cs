using System.Reflection;
using OpenBox.Application.Common;
using OpenBox.Persistence.Common;

namespace OpenBox.WebApi.Configurations;

/// <summary>
/// Define the configuration about dependency injection.
/// </summary>
public static class DependencyInjectionConfiguration
{
    /// <summary>
    /// Setup the dependency injection configuration in <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> for web applications and services.</param>
    public static void AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
    {
        // Register by reflexion on specified assemblies
        builder.Services.Scan(scan => scan
            .FromAssemblies(new List<Assembly>
                { Assembly.Load("OpenBox.Application"), Assembly.Load("OpenBox.Persistence") })

            // Queries
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>))
                .Where(c => !c.IsAbstract && !c.IsGenericTypeDefinition))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped)

            // Commands
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>))
                .Where(c => !c.IsAbstract && !c.IsGenericTypeDefinition))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped)
            
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>))
                .Where(c => !c.IsAbstract && !c.IsGenericTypeDefinition))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped)

            // Repositories
            .AddClasses(classes => classes.AssignableTo(typeof(IRepositoryBase<>))
                .Where(c => !c.IsAbstract && !c.IsGenericTypeDefinition))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped)
        );

        // Others
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}