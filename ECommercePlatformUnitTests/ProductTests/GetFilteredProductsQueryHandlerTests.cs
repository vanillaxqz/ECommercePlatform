using Application.DTOs;
using Application.UseCases.QueryHandlers.ProductQueryHandlers;
using Application.UseCases.Queries.ProductQueries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Domain.Common;

namespace ECommercePlatformUnitTests.ProductTests;

public class GetFilteredProductsQueryHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetFilteredProductsQueryHandler _handler;

    public GetFilteredProductsQueryHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetFilteredProductsQueryHandler(_productRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { ProductId = Guid.NewGuid(), Name = "Product1" } };
        var productDtos = new List<ProductDto> { new ProductDto { ProductId = products[0].ProductId, Name = products[0].Name } };
        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

        var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(productDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoProductsMatch()
    {
        // Arrange
        var products = new List<Product>();
        var productDtos = new List<ProductDto>();
        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

        var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldApplyFiltersCorrectly()
    {
        // Arrange
        var productName = "Product1";
        var products = new List<Product>
        {
            new Product { ProductId = Guid.NewGuid(), Name = productName },
            new Product { ProductId = Guid.NewGuid(), Name = "Product2" }
        };
        var productDtos = new List<ProductDto>
        {
            new ProductDto { ProductId = products[0].ProductId, Name = products[0].Name }
        };
        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

        var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Name = productName };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(productDtos);
    }

    [Fact]
    public async Task Handle_ShouldApplyPagingCorrectly()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ProductId = Guid.NewGuid(), Name = "Product1" },
            new Product { ProductId = Guid.NewGuid(), Name = "Product2" }
        };
        var productDtos = new List<ProductDto>
        {
            new ProductDto { ProductId = products[0].ProductId, Name = products[0].Name }
        };
        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

        var query = new GetFilteredProductsQuery { Page = 1, PageSize = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenMappingFails()
    {
        // Arrange
        var products = new List<Product> { new Product { ProductId = Guid.NewGuid(), Name = "Product1" } };
        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(x => throw new Exception("Mapping failure"));

        var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10 };

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Mapping failure");
    }
    [Fact]
    public async Task Handle_ShouldApplyDescriptionFilterCorrectly()
    {
        // Arrange
        var description = "Description1";
        var products = new List<Product>
    {
        new Product { ProductId = Guid.NewGuid(), Name = "Product1", Description = description },
        new Product { ProductId = Guid.NewGuid(), Name = "Product2", Description = "Description2" }
    };
        var productDtos = new List<ProductDto>
    {
        new ProductDto { ProductId = products[0].ProductId, Name = products[0].Name, Description = products[0].Description }
    };
        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

        var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Description = description };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(productDtos);
    }

    [Fact]
    public async Task Handle_ShouldApplyPriceFilterCorrectly()
    {
        // Arrange
        var price = 100m;
        var products = new List<Product>
    {
        new Product { ProductId = Guid.NewGuid(), Name = "Product1", Price = price },
        new Product { ProductId = Guid.NewGuid(), Name = "Product2", Price = 200m }
    };
        var productDtos = new List<ProductDto>
    {
        new ProductDto { ProductId = products[0].ProductId, Name = products[0].Name, Price = products[0].Price }
    };
        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

        var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Price = price };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(productDtos);
    }

    [Fact]
    public async Task Handle_ShouldApplyStockFilterCorrectly()
    {
        // Arrange
        var stock = 10;
        var products = new List<Product>
    {
        new Product { ProductId = Guid.NewGuid(), Name = "Product1", Stock = stock },
        new Product { ProductId = Guid.NewGuid(), Name = "Product2", Stock = 20 }
    };
        var productDtos = new List<ProductDto>
    {
        new ProductDto { ProductId = products[0].ProductId, Name = products[0].Name, Stock = products[0].Stock }
    };
        _productRepository.GetAllProductsAsync().Returns(Result<IEnumerable<Product>>.Success(products));
        _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

        var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Stock = stock };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Data.Should().BeEquivalentTo(productDtos);
    }
}
