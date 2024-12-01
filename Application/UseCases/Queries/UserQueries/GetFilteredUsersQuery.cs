using Application.DTOs;
using Application.Utils;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.UserQueries
{
    public class GetFilteredUsersQuery : IRequest<Result<PagedResult<UserDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
