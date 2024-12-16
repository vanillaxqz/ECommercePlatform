using Domain.Entities;

namespace Application.AIML
{
    public static class ProductDataGenerator
    {
        public static List<ProductData> GetProductData()
        {
            return new List<ProductData>
            {
                new()
                {
                    Name = "Shirt",
                    Description = "A palain black shirt",
                    Price = 5.0f,
                    Stock = 100,
                    Category = 2
                },
                new()
                {
                    Name = "Terraria",
                    Description = "A sand-box survival 2D classic",
                    Price = 10.0f,
                    Stock = 200,
                    Category = 7
                },
                new()
                {
                    Name = "Nintendo Switch",
                    Description = "The 2017 hit handheld console by Nintendo",
                    Price = 300.0f,
                    Stock = 150,
                    Category = 1
                }
            };
        }
    }
}
