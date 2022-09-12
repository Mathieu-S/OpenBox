using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using OpenBox.Application.Handlers.Brands.Commands;
using OpenBox.WebApi.Tests.Integration.Fixtures;
using Xunit;

namespace OpenBox.WebApi.Tests.Integration.UseCases.Authenticated;

[Collection("WebApplication Context")]
public class BrandTest
{
    private readonly WebApplicationFixture _application;

    public BrandTest(WebApplicationFixture fixture)
    {
        _application = fixture;
        _application.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }
    
    [Fact]
    public async Task Get_Many()
    {
        // Act
        var response = await _application.Client.GetAsync("/brand");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_One()
    {
        // Arrange
        var brandToGet = "129c5546-4a6d-454e-848a-fe75b58d5cf7";

        // Act
        var response = await _application.Client.GetAsync($"/brand/{brandToGet}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Post()
    {
        // Arrange
        var newBrand = new CreateBrand(Faker.Company.Name());

        // Act
        var response = await _application.Client.PostAsJsonAsync("/brand", newBrand);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task Put()
    {
        // Arrange
        var brandToEdit = new UpdateBrand(Guid.Parse("b9371076-60d2-43c4-9856-8b8799e9ce69"), Faker.Company.Name());

        // Act
        var response = await _application.Client.PutAsJsonAsync($"/brand/{brandToEdit.Id}", brandToEdit);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete()
    {
        // Arrange
        var brandToDelete = Guid.Parse("dfedf0f8-eb13-4f98-bf2f-6f7b4847c062");

        // Act
        var response = await _application.Client.DeleteAsync($"/brand/{brandToDelete}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}