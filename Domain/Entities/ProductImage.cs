namespace Domain.Entities
{
    public class ProductImage
    {
        public string ImagePath { get; set; }
        public Guid ProductImageId { get; set; }
        public Guid ProductId { get; set; }
    }
}
