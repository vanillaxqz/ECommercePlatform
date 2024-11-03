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
            try
            {
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(product.ProductId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Error adding product: {ex.Message}");
            }
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            try
            {
                var toRemove = await context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (toRemove != null)
                {
                    context.Products.Remove(toRemove);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error deleting product: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetAllProductsAsync()
        {
            try
            {
                var products = await context.Products.ToListAsync();
                return Result<IEnumerable<Product>>.Success(products);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Product>>.Failure($"Error retrieving all products: {ex.Message}");
            }
        }

        public async Task<Result<Product>> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var product = await context.Products.FindAsync(productId);
                return Result<Product>.Success(product);
            }
            catch (Exception ex)
            {
                return Result<Product>.Failure($"Error retrieving product by ID: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                context.Entry(product).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error updating product: {ex.Message}");
            }
        }
    }
}
