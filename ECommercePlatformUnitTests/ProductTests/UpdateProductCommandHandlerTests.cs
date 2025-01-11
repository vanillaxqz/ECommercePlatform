using Application.UseCases.CommandHandlers.ProductCommandHandlers;
using Application.UseCases.Commands.ProductCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using MediatR;

namespace ECommercePlatformUnitTests.ProductTests
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly UpdateProductCommandHandler _handler;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UpdateProductCommandHandler(_productRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidCommand_WhenHandlingUpdateProduct_ThenShouldCallUpdateProductAsync()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.0m,
                Stock = 10,
                Category = Category.Electronics
            };

            var product = new Product
            {
                ProductId = command.ProductId,
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Stock = command.Stock,
                Category = command.Category
            };

            _mapper.Map<Product>(command).Returns(product);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _productRepository.Received(1).UpdateProductAsync(product);
            result.Should().Be(Unit.Value);
        }
    }
}

