using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests
{
    public class OrderItemTests
    {
        [Fact]
        public void OrderItem_ShouldSetAndGetProperties()
        {
            // Arrange
            var orderItemId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var name = "Item Name";
            var description = "Item Description";
            var price = 99.99m;
            var imagePath = "/images/item.png";

            // Act
            var orderItem = new OrderItem
            {
                OrderItemId = orderItemId,
                OrderId = orderId,
                Name = name,
                Description = description,
                Price = price,
                ImagePath = imagePath
            };

            // Assert
            orderItem.OrderItemId.Should().Be(orderItemId);
            orderItem.OrderId.Should().Be(orderId);
            orderItem.Name.Should().Be(name);
            orderItem.Description.Should().Be(description);
            orderItem.Price.Should().Be(price);
            orderItem.ImagePath.Should().Be(imagePath);
        }

        [Fact]
        public void OrderItem_ShouldAllowNullProperties()
        {
            // Arrange & Act
            var orderItem = new OrderItem
            {
                OrderItemId = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                Name = null,
                Description = null,
                Price = 0,
                ImagePath = null
            };

            // Assert
            orderItem.Name.Should().BeNull();
            orderItem.Description.Should().BeNull();
            orderItem.ImagePath.Should().BeNull();
        }
    }
}