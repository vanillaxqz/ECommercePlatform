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
        private readonly GetFilteredProductsQueryHandler _handler;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetFilteredProductsQueryHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetFilteredProductsQueryHandler(_productRepository, _mapper);
        }

        [Fact]
        public async Task GivenValidQuery_WhenHandlingGetFilteredProductsQuery_ThenShouldReturnSuccessResult()
        {
            // Arrange
            var query = new GetFilteredProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Name = "Test Product"
            };

            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            var productsDto = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>())
                .Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productsDto);
            result.Data.TotalCount.Should().Be(products.Count);
        }

        [Fact]
        public async Task GivenInvalidQuery_WhenHandlingGetFilteredProductsQuery_ThenShouldReturnFailureResult()
        {
            // Arrange
            var query = new GetFilteredProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Name = "Invalid Product"
            };

            var errorMessage = "Failure";

            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>())
                .Returns(Result<IEnumerable<Product>>.Failure(errorMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be(errorMessage);
        }

        [Fact]
        public async Task GivenNoProducts_WhenHandlingGetFilteredProductsQuery_ThenShouldReturnEmptyResult()
        {
            // Arrange
            var query = new GetFilteredProductsQuery
            {
                Page = 1,
                PageSize = 10
            };

            var products = new List<Product>();
            var productsDto = new List<ProductDto>();

            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>())
                .Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEmpty();
            result.Data.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GivenQueryWithName_WhenHandlingGetFilteredProductsQuery_ThenShouldReturnFilteredResult()
        {
            // Arrange
            var query = new GetFilteredProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Name = "Test Product"
            };

            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            var productsDto = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>())
                .Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productsDto);
            result.Data.TotalCount.Should().Be(products.Count);
        }

        [Fact]
        public async Task GivenQueryWithDescription_WhenHandlingGetFilteredProductsQuery_ThenShouldReturnFilteredResult()
        {
            // Arrange
            var query = new GetFilteredProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Description = "Test Description"
            };

            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            var productsDto = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>())
                .Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productsDto);
            result.Data.TotalCount.Should().Be(products.Count);
        }

        [Fact]
        public async Task GivenQueryWithPrice_WhenHandlingGetFilteredProductsQuery_ThenShouldReturnFilteredResult()
        {
            // Arrange
            var query = new GetFilteredProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Price = 100.0m
            };

            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            var productsDto = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>())
                .Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productsDto);
            result.Data.TotalCount.Should().Be(products.Count);
        }

        [Fact]
        public async Task GivenQueryWithStock_WhenHandlingGetFilteredProductsQuery_ThenShouldReturnFilteredResult()
        {
            // Arrange
            var query = new GetFilteredProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Stock = 10
            };

            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            var productsDto = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>())
                .Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productsDto);
            result.Data.TotalCount.Should().Be(products.Count);
        }

        [Fact]
        public async Task GivenQueryWithCategory_WhenHandlingGetFilteredProductsQuery_ThenShouldReturnFilteredResult()
        {
            // Arrange
            var query = new GetFilteredProductsQuery
            {
                Page = 1,
                PageSize = 10,
                Category = Category.Electronics
            };

            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            var productsDto = new List<ProductDto>
            {
                new ProductDto { ProductId = products[0].ProductId, Name = "Test Product", Description = "Test Description", Price = 100.0m, Stock = 10, Category = Category.Electronics }
            };

            _productRepository.GetAllProductsAsync(Arg.Any<Func<IQueryable<Product>, IQueryable<Product>>>())
                .Returns(Result<IEnumerable<Product>>.Success(products));
            _mapper.Map<List<ProductDto>>(Arg.Any<IEnumerable<Product>>()).Returns(productsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Should().BeEquivalentTo(productsDto);
            result.Data.TotalCount.Should().Be(products.Count);
        }
    }
}