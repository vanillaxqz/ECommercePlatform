using Domain.Common;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Result<Guid>> AddOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Order>>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Order>> GetOrderByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Order>>> GetOrdersByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
