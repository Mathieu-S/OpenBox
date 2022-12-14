using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OpenBox.Domain.Entities;
using OpenBox.Domain.Tests.Unit.Fakers;
using OpenBox.Persistence;

namespace OpenBox.WebApi.Tests.Integration.TestUtils;

public static class DatabaseHelper
{
    public static void SeedDatabase(this WebApplicationFactory<Program> application)
    {
        using var serviceScope = application.Services.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<OpenBoxDbContext>();

        var brands = new BrandFaker().Generate(3);
        brands[0].Id = Guid.Parse("129c5546-4a6d-454e-848a-fe75b58d5cf7");
        brands[1].Id = Guid.Parse("b9371076-60d2-43c4-9856-8b8799e9ce69");
        brands[2].Id = Guid.Parse("dfedf0f8-eb13-4f98-bf2f-6f7b4847c062");

        dbContext.Brands.AddRange(brands);

        dbContext.SaveChanges();
    }
}