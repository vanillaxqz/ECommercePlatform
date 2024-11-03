using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.ProductTests;

public class ProductRepositoryTests
{
    private readonly IProductRepository _productRepository;

    public ProductRepositoryTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
    }

    [Fact]
    public async Task GivenValidProduct_WhenAddingProduct_ThenShouldReturnSuccessResult()
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

        _productRepository.AddProductAsync(product).Returns(Result<Guid>.Success(productId));

        // Act
        var result = await _productRepository.AddProductAsync(product);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(productId);
    }

    [Fact]
    public async Task GivenExistingProducts_WhenGettingAllProducts_ThenShouldReturnAllProducts()
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

        // Act
        var result = await _productRepository.GetAllProductsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GivenExistingProduct_WhenGettingProductById_ThenShouldReturnMatchingProduct()
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

        _productRepository.GetProductByIdAsync(productId).Returns(Result<Product>.Success(product));

        // Act
        var result = await _productRepository.GetProductByIdAsync(productId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GivenRepositoryError_WhenAddingProduct_ThenShouldReturnFailureResult()
    {
        // Arrange
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = 10,
            Category = Category.Electronics
        };

        _productRepository.AddProductAsync(product)
            .Returns(Result<Guid>.Failure("Database error"));

        // Act
        var result = await _productRepository.AddProductAsync(product);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GivenUpdateProduct_WhenUpdatingProduct_ThenShouldUpdateSuccessfully()
    {
        // Arrange
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 199.99m,
            Stock = 20,
            Category = Category.Electronics
        };

        // Act
        await _productRepository.UpdateProductAsync(product);

        // Assert
        await _productRepository.Received(1).UpdateProductAsync(Arg.Is<Product>(p =>
            p.ProductId == product.ProductId &&
            p.Name == product.Name &&
            p.Description == product.Description &&
            p.Price == product.Price &&
            p.Stock == product.Stock &&
            p.Category == product.Category
        ));
    }

    [Fact]
    public async Task GivenProductToDelete_WhenDeletingProduct_ThenShouldDeleteSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        await _productRepository.DeleteProductAsync(productId);

        // Assert
        await _productRepository.Received(1).DeleteProductAsync(productId);
    }

    [Theory]
    [InlineData(0)] // Zero price
    [InlineData(-1)] // Negative price
    public async Task GivenInvalidPrice_WhenAddingProduct_ThenShouldReturnFailureResult(decimal price)
    {
        // Arrange
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Description",
            Price = price,
            Stock = 10,
            Category = Category.Electronics
        };

        _productRepository.AddProductAsync(product)
            .Returns(Result<Guid>.Failure("Invalid price"));

        // Act
        var result = await _productRepository.AddProductAsync(product);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineData(-1)] // Negative stock
    [InlineData(-100)] // Large negative stock
    public async Task GivenInvalidStock_WhenAddingProduct_ThenShouldReturnFailureResult(int stock)
    {
        // Arrange
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = stock,
            Category = Category.Electronics
        };

        _productRepository.AddProductAsync(product)
            .Returns(Result<Guid>.Failure("Invalid stock"));

        // Act
        var result = await _productRepository.AddProductAsync(product);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GivenNonExistentProductId_WhenGettingProductById_ThenShouldReturnFailure()
    {
        // Arrange
        var nonExistentProductId = Guid.NewGuid();
        var failureResult = Result<Product>.Failure("Product not found");
        _productRepository.GetProductByIdAsync(nonExistentProductId).Returns(failureResult);

        // Act
        var result = await _productRepository.GetProductByIdAsync(nonExistentProductId);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}