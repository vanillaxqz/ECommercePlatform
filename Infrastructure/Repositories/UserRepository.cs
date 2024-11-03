using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<Guid>> AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Result<Guid>.Success(user.UserId);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var toRemove = context.Users.FirstOrDefault(x => x.UserId == userId);
            if (toRemove != null)
            {
                context.Users.Remove(toRemove);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Result<IEnumerable<User>>> GetAllUsersAsync()
        {
            var users = await context.Users.ToListAsync();
            return Result<IEnumerable<User>>.Success(users);
        }

        public async Task<Result<User>> GetUserByIdAsync(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            return Result<User>.Success(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
