using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.UserQueries
{
    public class GetAllUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
    {
    }
}
