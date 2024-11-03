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

public class GetProductByIdQueryHandlerTests
{
    private readonly GetProductByIdQueryHandler _handler;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _handler = new GetProductByIdQueryHandler(_productRepository, _mapper);
    }

    [Fact]
    public async Task GivenValidId_WhenGetProductByIdQueryIsHandled_ThenProductShouldBeReturned()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            ProductId = productId,
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = 10,
            Category = Category.Electronics
        };

        _productRepository.GetProductByIdAsync(productId)
            .Returns(Result<Product>.Success(product));

        var query = new GetProductByIdQuery { Id = productId };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.ProductId.Should().Be(productId);
        result.Data.Name.Should().Be(product.Name);
        result.Data.Description.Should().Be(product.Description);
        result.Data.Price.Should().Be(product.Price);
        result.Data.Stock.Should().Be(product.Stock);
        result.Data.Category.Should().Be(product.Category);
    }

    [Fact]
    public async Task GivenInvalidId_WhenGetProductByIdQueryIsHandled_ThenShouldReturnFailure()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        _productRepository.GetProductByIdAsync(invalidId)
            .Returns(Result<Product>.Failure("Product not found"));

        var query = new GetProductByIdQuery { Id = invalidId };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}