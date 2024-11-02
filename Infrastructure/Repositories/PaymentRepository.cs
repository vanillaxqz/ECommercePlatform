using Domain.Common;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task<Result<Guid>> AddPaymentAsync(Payment payment)
        {
            throw new NotImplementedException();
        }

        public Task DeletePaymentAsync(Guid paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Payment>>> GetAllPaymentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Payment>> GetPaymentByIdAsync(Guid paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePaymentAsync(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
