using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Result<Order>> GetOrderByIdAsync(Guid orderId);
        Task<Result<IEnumerable<Order>>> GetAllOrdersAsync();
        Task<Result<IEnumerable<Order>>> GetOrdersByUserIdAsync(Guid userId);
        Task<Result<Guid>> AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Guid orderId);
    }
}
