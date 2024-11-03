using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;
        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddProductAsync(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return Result<Guid>.Success(product.ProductId);
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var toRemove = context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (toRemove != null)
            {
                context.Products.Remove(toRemove);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var products = await context.Products.ToListAsync();
            return Result<IEnumerable<Product>>.Success(products);
        }

        public async Task<Result<Product>> GetProductByIdAsync(Guid productId)
        {
            var product = await context.Products.FindAsync(productId);
            return Result<Product>.Success(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
