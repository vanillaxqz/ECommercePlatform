using Application.UseCases.Queries.ProductQueries;
using Application.UseCases.QueryHandlers.ProductQueryHandlers;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.ProductTests;

public class GetAllProductsQueryHandlerTests
{
    private readonly GetAllProductsQueryHandler _handler;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _handler = new GetAllProductsQueryHandler(_productRepository, _mapper);
    }

    [Fact]
    public async Task GivenMultipleProducts_WhenGetAllProductsQueryIsHandled_ThenAllProductsShouldBeReturned()
    {
        // Arrange
        var products = new List<Product>
        {
            new()
            {
                ProductId = Guid.NewGuid(),
                Name = "Product 1",
                Description = "Description 1",
                Price = 99.99m,
                Stock = 10,
                Category = Category.Electronics
            },
            new()
            {
                ProductId = Guid.NewGuid(),
                Name = "Product 2",
                Description = "Description 2",
                Price = 149.99m,
                Stock = 5,
                Category = Category.Fashion
            }
        };

        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));

        var query = new GetAllProductsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().AllSatisfy(p =>
        {
            p.ProductId.Should().NotBe(Guid.Empty);
            p.Name.Should().NotBeNullOrEmpty();
            p.Description.Should().NotBeNullOrEmpty();
            p.Price.Should().BeGreaterThan(0);
            p.Stock.Should().BeGreaterThan(0);
        });
    }

    [Fact]
    public async Task GivenNoProducts_WhenGetAllProductsQueryIsHandled_ThenShouldReturnEmptyList()
    {
        // Arrange
        _productRepository.GetAllProductsAsync()
            .Returns(Result<IEnumerable<Product>>.Success(new List<Product>()));

        var query = new GetAllProductsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task GivenRepositoryError_WhenGettingAllProducts_ThenShouldReturnFailure()
    {
        // Arrange
        _productRepository.GetAllProductsAsync()
            .Returns(Result<IEnumerable<Product>>.Failure("Database error"));

        var query = new GetAllProductsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GivenDifferentCategories_WhenGettingAllProducts_ThenShouldReturnAllCategories()
    {
        // Arrange
        var products = new List<Product>();
        foreach (Category category in Enum.GetValues(typeof(Category)))
            products.Add(new Product
            {
                ProductId = Guid.NewGuid(),
                Name = $"Product {category}",
                Description = $"Description {category}",
                Price = 99.99m,
                Stock = 10,
                Category = category
            });

        _productRepository.GetAllProductsAsync()
            .Returns(Result<IEnumerable<Product>>.Success(products));

        var query = new GetAllProductsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Select(p => p.Category).Distinct().Count()
            .Should().Be(Enum.GetValues(typeof(Category)).Length);
    }

    [Fact]
    public async Task GivenMaximumValues_WhenGettingProducts_ThenShouldHandleMaxValues()
    {
        // Arrange
        var products = new List<Product>
        {
            new()
            {
                ProductId = Guid.NewGuid(),
                Name = new string('A', 200), // Max length
                Description = new string('A', 500), // Max length
                Price = decimal.MaxValue,
                Stock = int.MaxValue,
                Category = Category.Electronics
            }
        };

        _productRepository.GetAllProductsAsync()
            .Returns(Result<IEnumerable<Product>>.Success(products));

        var query = new GetAllProductsQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var product = result.Data.First();
        product.Price.Should().Be(decimal.MaxValue);
        product.Stock.Should().Be(int.MaxValue);
    }
}