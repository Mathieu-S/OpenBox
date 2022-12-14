using Bogus;
using OpenBox.Application.Common;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using OpenBox.Domain.Tests.Unit.Fakers;
using OpenBox.Persistence.Common;
using OpenBox.Persistence.Repositories;
using OpenBox.Persistence.Tests.Unit.Fixtures;
using Xunit;

namespace OpenBox.Persistence.Tests.Unit.Repositories;

/// <summary>
/// Test the RepositoryBase logic.
/// </summary>
/// <remarks>The brand entity is used for this tests.</remarks>
public class GenericRepositoryTest : IClassFixture<BrandSeedDataFixture>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OpenBoxDbContext _dbContext;

    public GenericRepositoryTest(BrandSeedDataFixture fixture)
    {
        _brandRepository = new BrandRepository(fixture.DbContext);
        _unitOfWork = new UnitOfWork(fixture.DbContext);
        _dbContext = fixture.DbContext;
    }

    [Fact]
    public async Task Get_All()
    {
        // Arrange
        var brands = new BrandFaker().Generate(2);
        _dbContext.AddRange(brands);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _brandRepository.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IList<Brand>>(result);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    public async Task Get_All_With_Pagination(int pageIndex, int pageSize)
    {
        // Arrange
        var brands = new BrandFaker().Generate(2);
        _dbContext.AddRange(brands);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _brandRepository.GetAllAsync(pageIndex, pageSize, CancellationToken.None)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IList<Brand>>(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task Get_One()
    {
        // Arrange
        var brand = new BrandFaker().Generate();
        _dbContext.Add(brand);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _brandRepository.GetAsync(brand.Id, false, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Brand>(result);
        Assert.Equal(brand.Id, result.Id);
    }

    [Fact]
    public async Task Get_Unknown()
    {
        // Act
        var result = await _brandRepository.GetAsync(Guid.NewGuid(), false, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Add_One()
    {
        // Arrange
        var brand = new BrandFaker().Generate();

        // Act
        var result = _brandRepository.Add(brand);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var brandsInDatabase = _dbContext.Brands.ToList();
        Assert.NotEqual(Guid.Empty, result);
        Assert.Contains(brand, brandsInDatabase);
    }

    [Fact]
    public async Task Add_Many()
    {
        // Arrange
        var brands = new BrandFaker().Generate(2);

        // Act
        var result = _brandRepository.Add(brands);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var brandsInDatabase = _dbContext.Brands.ToList();
        Assert.NotEqual(Guid.Empty, result.FirstOrDefault());
        Assert.Contains(brands.FirstOrDefault(), brandsInDatabase);
    }

    [Fact]
    public async Task Update_One()
    {
        // Arrange
        var newBrandName = new Faker().Company.CompanyName();
        var brand = await _brandRepository.GetAsync(_dbContext.Brands.First().Id, true, CancellationToken.None);

        // Act
        brand!.Name = newBrandName;
        _brandRepository.Update(brand);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var modifiedBrand = _dbContext.Brands.First();
        Assert.Equal(newBrandName, modifiedBrand.Name);
    }

    [Fact]
    public async Task Update_Many()
    {
        // Arrange
        var faker = new Faker();
        var newBrandNameForFirstBrand = faker.Company.CompanyName();
        var newBrandNameForLastBrand = faker.Company.CompanyName();
        var firstBrand = await _brandRepository.GetAsync(_dbContext.Brands.First().Id, true, CancellationToken.None);
        var lastBrand = await _brandRepository.GetAsync(_dbContext.Brands.Last().Id, true, CancellationToken.None);
        firstBrand!.Name = newBrandNameForFirstBrand;
        lastBrand!.Name = newBrandNameForLastBrand;
        var brands = new List<Brand> { firstBrand, lastBrand };

        // Act

        _brandRepository.Update(brands);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var modifiedFirstBrand = _dbContext.Brands.First();
        var modifiedLastBrand = _dbContext.Brands.Last();
        Assert.Equal(newBrandNameForFirstBrand, modifiedFirstBrand.Name);
        Assert.Equal(newBrandNameForLastBrand, modifiedLastBrand.Name);
    }

    [Fact]
    public async Task Delete_One()
    {
        // Arrange
        var brand = await _brandRepository.GetAsync(_dbContext.Brands.First().Id, true, CancellationToken.None);

        // Act
        _brandRepository.Delete(brand!);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var brandsInDatabase = _dbContext.Brands.ToList();
        Assert.DoesNotContain(brand, brandsInDatabase);
    }

    [Fact]
    public async Task Delete_Many()
    {
        // Arrange
        var firstBrand = await _brandRepository.GetAsync(_dbContext.Brands.First().Id, true, CancellationToken.None);
        var lastBrand = await _brandRepository.GetAsync(_dbContext.Brands.First().Id, true, CancellationToken.None);
        var brands = new List<Brand> { firstBrand!, lastBrand! };

        // Act
        _brandRepository.Delete(brands);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var brandsInDatabase = _dbContext.Brands.ToList();
        Assert.DoesNotContain(firstBrand, brandsInDatabase);
        Assert.DoesNotContain(lastBrand, brandsInDatabase);
    }
}