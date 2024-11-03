using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

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
            try
            {
                var toRemove = await context.Payments.FirstOrDefaultAsync(x => x.PaymentId == paymentId);
                if (toRemove != null)
                {
                    context.Payments.Remove(toRemove);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error deleting payment: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<Payment>>> GetAllPaymentsAsync()
        {
            try
            {
                var payments = await context.Payments.ToListAsync();
                return Result<IEnumerable<Payment>>.Success(payments);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Payment>>.Failure($"Error retrieving all payments: {ex.Message}");
            }
        }

        public async Task<Result<Payment>> GetPaymentByIdAsync(Guid paymentId)
        {
            try
            {
                var payment = await context.Payments.FindAsync(paymentId);
                if(payment == null)
                {
                    throw new Exception();
                }
                return Result<Payment>.Success(payment);
            }
            catch (Exception ex)
            {
                return Result<Payment>.Failure($"Error retrieving payment by ID: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(Guid userId)
        {
            try
            {
                var payments = await context.Payments.Where(o => o.UserId == userId).ToListAsync();
                return Result<IEnumerable<Payment>>.Success(payments);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Payment>>.Failure($"Error retrieving payments by user ID: {ex.Message}");
            }
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            try
            {
                context.Entry(payment).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error updating payment: {ex.Message}");
            }
        }
    }
}
