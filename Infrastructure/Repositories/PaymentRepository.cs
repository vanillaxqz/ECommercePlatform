using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext context;
        public PaymentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<Guid>> AddPaymentAsync(Payment payment)
        {
            await context.Payments.AddAsync(payment);
            await context.SaveChangesAsync();
            return Result<Guid>.Success(payment.PaymentId);
        }

        public async Task DeletePaymentAsync(Guid paymentId)
        {
            var toRemove = context.Payments.FirstOrDefault(x => x.PaymentId == paymentId);
            if (toRemove != null)
            {
                context.Payments.Remove(toRemove);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Result<IEnumerable<Payment>>> GetAllPaymentsAsync()
        {
            var payments = await context.Payments.ToListAsync();
            return Result<IEnumerable<Payment>>.Success(payments);
        }

        public async Task<Result<Payment>> GetPaymentByIdAsync(Guid paymentId)
        {
            var payments = await context.Payments.FindAsync(paymentId);
            return Result<Payment>.Success(payments);
        }

        public async Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(Guid userId)
        {
            var payments = await context.Payments.Where(o => o.UserId == userId).ToListAsync();
            return Result<IEnumerable<Payment>>.Success(payments);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            context.Entry(payment).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
