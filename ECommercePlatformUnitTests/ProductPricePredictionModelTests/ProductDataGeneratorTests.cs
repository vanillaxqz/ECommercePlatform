using Application.AIML;
using FluentAssertions;

namespace Application.UnitTests.AIML
{
    public class ProductDataGeneratorTests
    {
        [Fact]
        public void GetProductData_ShouldReturnNonEmptyList()
        {
            // Act
            var productDataList = ProductDataGenerator.GetProductData();

            // Assert
            productDataList.Should().NotBeEmpty();
        }

        [Fact]
        public void GetProductData_ShouldReturnCorrectNumberOfProducts()
        {
            // Act
            var productDataList = ProductDataGenerator.GetProductData();

            // Assert
            productDataList.Should().HaveCount(3);
        }

        [Fact]
        public void GetProductData_ShouldReturnProductsWithCorrectProperties()
        {
            // Act
            var productDataList = ProductDataGenerator.GetProductData();

            // Assert
            productDataList[0].Name.Should().Be("Shirt");
            productDataList[0].Description.Should().Be("A palain black shirt");
            productDataList[0].Price.Should().Be(5.0f);
            productDataList[0].Stock.Should().Be(100);
            productDataList[0].Category.Should().Be(2);

            productDataList[1].Name.Should().Be("Terraria");
            productDataList[1].Description.Should().Be("A sand-box survival 2D classic");
            productDataList[1].Price.Should().Be(10.0f);
            productDataList[1].Stock.Should().Be(200);
            productDataList[1].Category.Should().Be(7);

            productDataList[2].Name.Should().Be("Nintendo Switch");
            productDataList[2].Description.Should().Be("The 2017 hit handheld console by Nintendo");
            productDataList[2].Price.Should().Be(300.0f);
            productDataList[2].Stock.Should().Be(150);
            productDataList[2].Category.Should().Be(1);
        }
    }
}