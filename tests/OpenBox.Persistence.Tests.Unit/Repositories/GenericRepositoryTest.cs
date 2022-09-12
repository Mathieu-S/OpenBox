using OpenBox.Application.Common;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
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
    private readonly IEnumerable<Brand> _brandsInDb;

    public GenericRepositoryTest(BrandSeedDataFixture fixture)
    {
        _brandRepository = new BrandRepository(fixture.DbContext);
        _unitOfWork = new UnitOfWork(fixture.DbContext);
        _brandsInDb = fixture.DbContext.Brands;
    }

    [Fact]
    public async Task Get_All()
    {
        // Act
        var result = await _brandRepository.GetAllAsync();

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
        // Act
        var result = await _brandRepository.GetAllAsync(pageIndex, pageSize);

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
        var brandId = _brandsInDb.First();

        // Act
        var result = await _brandRepository.GetAsync(brandId.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Brand>(result);
        Assert.Equal(brandId.Id, result.Id);
    }

    [Fact]
    public async Task Get_Unknown()
    {
        // Act
        var result = await _brandRepository.GetAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Add_One()
    {
        // Arrange
        var brand = new Brand
        {
            Id = Guid.Empty,
            Name = Faker.Company.Name()
        };

        // Act
        var result = _brandRepository.Add(brand);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var brandsInDatabase = _brandsInDb.ToList();
        Assert.NotEqual(Guid.Empty, result);
        Assert.Contains(brand, brandsInDatabase);
    }

    [Fact]
    public async Task Add_Many()
    {
        // Arrange
        var brands = new List<Brand>
        {
            new()
            {
                Id = Guid.Empty,
                Name = Faker.Company.Name()
            }
        };

        // Act
        var result = _brandRepository.Add(brands);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var brandsInDatabase = _brandsInDb.ToList();
        Assert.NotEqual(Guid.Empty, result.FirstOrDefault());
        Assert.Contains(brands.FirstOrDefault(), brandsInDatabase);
    }

    [Fact]
    public async Task Update_One()
    {
        // Arrange
        var newBrandName = Faker.Company.Name();
        var brand = await _brandRepository.GetAsync(_brandsInDb.First().Id, true);

        // Act
        brand!.Name = newBrandName;
        _brandRepository.Update(brand);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var modifiedBrand = _brandsInDb.First();
        Assert.Equal(newBrandName, modifiedBrand.Name);
    }

    [Fact]
    public async Task Update_Many()
    {
        // Arrange
        var newBrandNameForFirstBrand = Faker.Company.Name();
        var newBrandNameForLastBrand = Faker.Company.Name();
        var firstBrand = await _brandRepository.GetAsync(_brandsInDb.First().Id, true);
        var lastBrand = await _brandRepository.GetAsync(_brandsInDb.Last().Id, true);
        firstBrand!.Name = newBrandNameForFirstBrand;
        lastBrand!.Name = newBrandNameForLastBrand;
        var brands = new List<Brand> { firstBrand, lastBrand };
        
        // Act
        
        _brandRepository.Update(brands);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var modifiedFirstBrand = _brandsInDb.First();
        var modifiedLastBrand = _brandsInDb.Last();
        Assert.Equal(newBrandNameForFirstBrand, modifiedFirstBrand.Name);
        Assert.Equal(newBrandNameForLastBrand, modifiedLastBrand.Name);
    }
    
    [Fact]
    public async Task Delete_One()
    {
        // Arrange
        var brand = await _brandRepository.GetAsync(_brandsInDb.First().Id, true);

        // Act
        _brandRepository.Delete(brand!);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var brandsInDatabase = _brandsInDb.ToList();
        Assert.DoesNotContain(brand, brandsInDatabase);
    }
    
    [Fact]
    public async Task Delete_Many()
    {
        // Arrange
        var firstBrand = await _brandRepository.GetAsync(_brandsInDb.First().Id, true);
        var lastBrand = await _brandRepository.GetAsync(_brandsInDb.First().Id, true);
        var brands = new List<Brand> { firstBrand!, lastBrand! };

        // Act
        _brandRepository.Delete(brands);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var brandsInDatabase = _brandsInDb.ToList();
        Assert.DoesNotContain(firstBrand, brandsInDatabase);
        Assert.DoesNotContain(lastBrand, brandsInDatabase);
    }
}