﻿using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

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
            try
            {
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
    }
}
