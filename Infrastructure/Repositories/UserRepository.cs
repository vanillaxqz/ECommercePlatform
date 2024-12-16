using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<User>> GetUserByEmailAsync(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return Result<User>.Failure("User not found");
            }
            return Result<User>.Success(user);
        }

        public async Task<Result<User>> LoginUser(User user)
        {
            var userToLogin = await context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (userToLogin == null)
            {
                return Result<User>.Failure($"Invalid email or password");
            }

            bool isPasswordValid = Hasher.VerifyPassword(user.Password, userToLogin.Password);
            if (!isPasswordValid)
            {
                return Result<User>.Failure($"Invalid email or password");
            }

            return Result<User>.Success(userToLogin);
        }

        public async Task<Result<Guid>> AddUserAsync(User user)
        {
            try
            {
                user.Password = Hasher.HashPassword(user.Password);
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(user.UserId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Error adding user: {ex.Message}");
            }
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var toRemove = await context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (toRemove != null)
            {
                context.Users.Remove(toRemove);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Result<IEnumerable<User>>> GetAllUsersAsync()
        {
            var users = await context.Users.ToListAsync();
            if (users == null)
            {
                return Result<IEnumerable<User>>.Failure($"Error retrieving all users");
            }
            return Result<IEnumerable<User>>.Success(users);
        }

        public async Task<Result<User>> GetUserByIdAsync(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return Result<User>.Failure($"Error retrieving user by ID");
            }
            return Result<User>.Success(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public string GeneratePasswordResetToken(User user)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }
    }
}
