using Application.AIML;
using FluentAssertions;

namespace Application.UnitTests.AIML
{
    public class ProductPricePredictionModelTests
    {
        private readonly ProductPricePredictionModel _model;

        public ProductPricePredictionModelTests()
        {
            _model = new ProductPricePredictionModel();
        }

        [Fact]
        public void TrainModel_ShouldTrainModelWithoutErrors()
        {
            // Arrange
            var csvFilePath = "products.csv";
            var csvContent = "Name,Description,Price,Stock,Category\n" +
                             "Product1,Description1,10.0,100,1\n" +
                             "Product2,Description2,20.0,200,2\n";

            File.WriteAllText(csvFilePath, csvContent);

            // Act
            Action act = () => _model.TrainModel();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void PredictPrice_ShouldReturnPredictedPrice()
        {
            // Arrange
            var productData = new ProductData
            {
                Name = "TestProduct",
                Description = "TestDescription",
                Stock = 50,
                Category = 1
            };

            var csvFilePath = "products.csv";
            var csvContent = "Name,Description,Price,Stock,Category\n" +
                             "Product1,Description1,10.0,100,1\n" +
                             "Product2,Description2,20.0,200,2\n";

            File.WriteAllText(csvFilePath, csvContent);
            _model.TrainModel();

            // Act
            var predictedPrice = _model.PredictPrice(productData);

            // Assert
            predictedPrice.Should().BeGreaterThan(0);
        }
    }
}