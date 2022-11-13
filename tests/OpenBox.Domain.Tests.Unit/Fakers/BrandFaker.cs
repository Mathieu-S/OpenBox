using Bogus;
using OpenBox.Domain.Entities;

namespace OpenBox.Domain.Tests.Unit.Fakers;

public sealed class BrandFaker : Faker<Brand>
{
    public BrandFaker()
    {
        // Main object
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Company.CompanyName());
    }
}