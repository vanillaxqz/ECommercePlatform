using Microsoft.ML;
using Microsoft.ML.Data;

namespace Application.AIML
{
    public class ProductPricePredictionModel
    {
        private readonly MLContext mlContext;
        private ITransformer model;
        public ProductPricePredictionModel() => mlContext = new MLContext();

        public void TrainModel()
        {
            string csvFilePath = "products.csv";
            var dataView = mlContext.Data.LoadFromTextFile<ProductData>(csvFilePath, hasHeader: true, separatorChar: ',');
            var pipeline = mlContext.Transforms.Text.FeaturizeText("NameFeaturized", nameof(ProductData.Name))
                .Append(mlContext.Transforms.Text.FeaturizeText("DescriptionFeaturized", nameof(ProductData.Description)))
                .Append(mlContext.Transforms.Conversion.ConvertType(nameof(ProductData.Stock), outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType(nameof(ProductData.Category), outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Concatenate("Features",
                    "NameFeaturized", "DescriptionFeaturized", nameof(ProductData.Stock), nameof(ProductData.Category)))
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: nameof(ProductData.Price), maximumNumberOfIterations: 200));
            model = pipeline.Fit(dataView);
        }

        public float PredictPrice(ProductData productData)
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductData, ProductDataPrediction>(model);
            var prediction = predictionEngine.Predict(productData);
            return prediction.Price;
        }
    }
}
