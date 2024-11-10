using Application.UseCases.CommandHandlers.ProductCommandHandlers;
using Application.UseCases.Commands.ProductCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.ProductTests;

public class CreateProductCommandHandlerTests
{
    private readonly CreateProductCommandHandler _handler;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateProductCommandHandler(_productRepository, _mapper);
    }

    [Fact]
    public async Task GivenValidCommand_WhenHandlingCreateProduct_ThenShouldReturnSuccessResult()
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = 10,
            Category = Category.Electronics
        };

        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Stock = command.Stock,
            Category = command.Category
        };

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.AddProductAsync(product).Returns(Result<Guid>.Success(product.ProductId));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(product.ProductId);
    }

    [Fact]
    public async Task GivenInvalidCommand_WhenHandlingCreateProduct_ThenShouldReturnFailureResult()
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = "", // Invalid empty name
            Description = "",
            Price = -1, // Invalid negative price
            Stock = -5 // Invalid negative stock
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "Description", 99.99, 10, Category.Electronics)] // Empty name
    [InlineData("Name", "", 99.99, 10, Category.Electronics)] // Empty description
    [InlineData("Name", "Description", 0, 10, Category.Electronics)] // Zero price
    [InlineData("Name", "Description", 99.99, -1, Category.Electronics)] // Negative stock
    public async Task GivenInvalidProductData_WhenCreatingProduct_ThenShouldReturnFailureResult(
        string name, string description, decimal price, int stock, Category category)
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = name,
            Description = description,
            Price = price,
            Stock = stock,
            Category = category
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GivenNullMapper_WhenCreatingProduct_ThenShouldReturnFailureResult()
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = 10,
            Category = Category.Electronics
        };

        _mapper.Map<Product>(command).Returns((Product?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}