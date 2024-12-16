using Microsoft.ML.Data;

namespace Application.AIML
{
    public class ProductDataPrediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}
