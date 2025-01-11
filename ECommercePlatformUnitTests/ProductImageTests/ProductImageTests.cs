using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests
{
    public class ProductImageTests
    {
        [Fact]
        public void ProductImage_ShouldSetAndGetProperties()
        {
            // Arrange
            var productImageId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var imagePath = "/images/product.png";

            // Act
            var productImage = new ProductImage
            {
                ProductImageId = productImageId,
                ProductId = productId,
                ImagePath = imagePath
            };

            // Assert
            productImage.ProductImageId.Should().Be(productImageId);
            productImage.ProductId.Should().Be(productId);
            productImage.ImagePath.Should().Be(imagePath);
        }

        [Fact]
        public void ProductImage_ShouldAllowNullImagePath()
        {
            // Arrange & Act
            var productImage = new ProductImage
            {
                ProductImageId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                ImagePath = null
            };

            // Assert
            productImage.ImagePath.Should().BeNull();
        }
    }
}