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
            try
            {
                await context.Payments.AddAsync(payment);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(payment.PaymentId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Error adding payment: {ex.Message}");
            }
        }

        public async Task DeletePaymentAsync(Guid paymentId)
        {
            var toRemove = await context.Payments.FirstOrDefaultAsync(x => x.PaymentId == paymentId);
            if (toRemove != null)
            {
                context.Payments.Remove(toRemove);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Result<IEnumerable<Payment>>> GetAllPaymentsAsync()
        {
            var payments = await context.Payments.ToListAsync();
            if (payments == null)
            {
                return Result<IEnumerable<Payment>>.Failure($"Error retrieving all payments");
            }
            return Result<IEnumerable<Payment>>.Success(payments);
        }

        public async Task<Result<Payment>> GetPaymentByIdAsync(Guid paymentId)
        {
            var payment = await context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                return Result<Payment>.Failure($"Error retrieving payment by ID");
            }
            return Result<Payment>.Success(payment);
        }

        public async Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(Guid userId)
        {
            var payments = await context.Payments.Where(o => o.UserId == userId).ToListAsync();
            if (payments == null)
            {
                return Result<IEnumerable<Payment>>.Failure($"Error retrieving payments by user ID");
            }
            return Result<IEnumerable<Payment>>.Success(payments);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            context.Entry(payment).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
