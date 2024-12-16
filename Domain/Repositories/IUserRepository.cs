using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> GetUserByIdAsync(Guid userId);
        Task<Result<IEnumerable<User>>> GetAllUsersAsync();
        Task<Result<Guid>> AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
        Task<Result<User>> LoginUser(User user);
        Task<Result<User>> GetUserByEmailAsync(string email);
    }
}
