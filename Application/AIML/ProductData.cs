using Domain.Entities;
using Microsoft.ML.Data;

namespace Application.AIML
{
    public class ProductData
    {
        [LoadColumn(0)]
        public string? Name { get; set; }
        [LoadColumn(1)]
        public string? Description { get; set; }
        [LoadColumn(2)]
        public float Price { get; set; }
        [LoadColumn(3)]
        public int Stock { get; set; }
        [LoadColumn(4)]
        public int Category { get; set; }
    }
}
