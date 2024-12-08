using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Result<Product>> GetProductByIdAsync(Guid productId);
        Task<Result<IEnumerable<Product>>> GetAllProductsAsync(Func<IQueryable<Product>, IQueryable<Product>>? query = null);
        Task<Result<Guid>> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid productId);
    }
}