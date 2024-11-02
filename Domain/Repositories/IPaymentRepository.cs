using Domain.Entities;
using Domain.Common;


namespace Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Result<Payment>> GetPaymentByIdAsync(Guid paymentId);
        Task<Result<IEnumerable<Payment>>> GetAllPaymentsAsync();
        Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(Guid userId);
        Task<Result<Guid>> AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(Guid paymentId);

    }
}
