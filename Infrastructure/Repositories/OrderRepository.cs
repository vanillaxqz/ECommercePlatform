using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddOrderAsync(Order order)
        {
            try
            {
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(order.OrderId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Error adding order: {ex.Message}");
            }
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            try
            {
                var toRemove = await context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
                if (toRemove != null)
                {
                    context.Orders.Remove(toRemove);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error deleting order: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<Order>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await context.Orders.ToListAsync();
                if(orders == null)
                {
                    throw new Exception();
                }
                return Result<IEnumerable<Order>>.Success(orders);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Order>>.Failure($"Error retrieving all orders: {ex.Message}");
            }
        }

        public async Task<Result<Order>> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                var order = await context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    throw new Exception();
                }
                return Result<Order>.Success(order);
            }
            catch (Exception ex)
            {
                return Result<Order>.Failure($"Error retrieving order by ID: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<Order>>> GetOrdersByUserIdAsync(Guid userId)
        {
            try
            {
                var orders = await context.Orders.Where(o => o.UserId == userId).ToListAsync();
                return Result<IEnumerable<Order>>.Success(orders);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Order>>.Failure($"Error retrieving orders by user ID: {ex.Message}");
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            try
            {
                context.Entry(order).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error updating order: {ex.Message}");
            }
        }
    }
}
