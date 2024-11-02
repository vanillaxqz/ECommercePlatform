using Domain.Common;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        public Task<Result<Guid>> AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<User>>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
