using Domain.Common;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Task<Result<Guid>> AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Product>>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Product>> GetProductByIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
