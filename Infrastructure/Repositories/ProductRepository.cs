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
            var toRemove = await context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (toRemove != null)
            {
                context.Products.Remove(toRemove);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetAllProductsAsync(Func<IQueryable<Product>, IQueryable<Product>>? query = null)
        {
            IQueryable<Product> productsQuery = context.Products;

            if (query != null)
            {
                productsQuery = query(productsQuery);
            }

            var products = await productsQuery.ToListAsync();
            if (products == null)
            {
                return Result<IEnumerable<Product>>.Failure($"Error retrieving all products");
            }
            return Result<IEnumerable<Product>>.Success(products);
        }

        public async Task<Result<Product>> GetProductByIdAsync(Guid productId)
        {
            var product = await context.Products.FindAsync(productId);
            if (product == null)
            {
                return Result<Product>.Failure($"Error retrieving product by ID");
            }
            return Result<Product>.Success(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
