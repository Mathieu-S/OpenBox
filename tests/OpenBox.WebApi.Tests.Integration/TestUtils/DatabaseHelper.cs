using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OpenBox.Domain.Entities;
using OpenBox.Persistence;

namespace OpenBox.WebApi.Tests.Integration.TestUtils;

public static class DatabaseHelper
{
    public static void SeedDatabase(this WebApplicationFactory<Program> application)
    {
        using var serviceScope = application.Services.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<OpenBoxDbContext>();

        dbContext.Brands.AddRange(new List<Brand>
        {
            new() { Id = Guid.Parse("129c5546-4a6d-454e-848a-fe75b58d5cf7"), Name = Faker.Company.Name() },
            new() { Id = Guid.Parse("b9371076-60d2-43c4-9856-8b8799e9ce69"), Name = Faker.Company.Name() },
            new() { Id = Guid.Parse("dfedf0f8-eb13-4f98-bf2f-6f7b4847c062"), Name = Faker.Company.Name() }
        });

        dbContext.SaveChanges();
    }
}