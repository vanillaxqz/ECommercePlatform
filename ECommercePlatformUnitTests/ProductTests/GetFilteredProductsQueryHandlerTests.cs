using Application.DTOs;
using Application.UseCases.QueryHandlers.ProductQueryHandlers;
using Application.UseCases.Queries.ProductQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ECommercePlatformUnitTests.ProductTests
{
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
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Success(products));
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
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Success(products));
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
            var name = "Product1";
            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = name },
                new Product { ProductId = Guid.NewGuid(), Name = "Product2" }
            };
            var productDtos = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Name = products[0].Name }
            };
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

            var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Name = name };

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
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

            var query = new GetFilteredProductsQuery { Page = 1, PageSize = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
        {
            // Arrange
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Failure("Repository failure"));

            var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Repository failure");
        }

        [Fact]
        public async Task Handle_ShouldFilterByDescription()
        {
            // Arrange
            var description = "Description1";
            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Description = description },
                new Product { ProductId = Guid.NewGuid(), Description = "Description2" }
            };
            var productDtos = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Description = products[0].Description }
            };
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

            var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Description = description };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productDtos);
        }

        [Fact]
        public async Task Handle_ShouldFilterByPrice()
        {
            // Arrange
            var price = 100m;
            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Price = price },
                new Product { ProductId = Guid.NewGuid(), Price = 200m }
            };
            var productDtos = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Price = products[0].Price }
            };
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

            var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Price = price };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productDtos);
        }

        [Fact]
        public async Task Handle_ShouldFilterByStock()
        {
            // Arrange
            var stock = 10;
            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Stock = stock },
                new Product { ProductId = Guid.NewGuid(), Stock = 20 }
            };
            var productDtos = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Stock = products[0].Stock }
            };
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

            var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Stock = stock };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productDtos);
        }

        [Fact]
        public async Task Handle_ShouldFilterByCategory()
        {
            // Arrange
            var category = Category.Electronics;
            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Category = category },
                new Product { ProductId = Guid.NewGuid(), Category = Category.Fashion }
            };
            var productDtos = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Category = products[0].Category }
            };
            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>()).Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productDtos);

            var query = new GetFilteredProductsQuery { Page = 1, PageSize = 10, Category = category };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productDtos);
        }
    }
}
