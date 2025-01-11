using Application.UseCases.CommandHandlers.ProductCommandHandlers;
using Application.UseCases.Commands.ProductCommands;
using AutoMapper;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using MediatR;

namespace ECommercePlatformUnitTests.ProductTests
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly DeleteProductCommandHandler _handler;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DeleteProductCommandHandler(_productRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidCommand_WhenHandlingDeleteProduct_ThenShouldCallDeleteProductAsync()
        {
            // Arrange
            var command = new DeleteProductCommand { Id = Guid.NewGuid() };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _productRepository.Received(1).DeleteProductAsync(command.Id);
            result.Should().Be(Unit.Value);
        }
    }
}
