using Bogus;
using OpenBox.Domain.Entities;

namespace OpenBox.Domain.Tests.Unit.Fakers;

public sealed class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        // Main object
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Random.Word());
        RuleFor(x => x.Description, f => f.Lorem.Sentence());
        RuleFor(x => x.Price, f => f.Random.UInt());
        // Relations
        RuleFor(x => x.Brand, new BrandFaker().Generate());
    }
}