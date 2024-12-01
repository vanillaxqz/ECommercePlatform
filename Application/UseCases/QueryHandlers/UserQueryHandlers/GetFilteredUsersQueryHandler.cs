using Application.DTOs;
using Application.UseCases.Queries.UserQueries;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using Gridify;

namespace Application.UseCases.QueryHandlers.UserQueryHandlers
{
    public class GetFilteredUserQueryHandler(IUserRepository repository, IMapper mapper) :
        IRequestHandler<GetFilteredUsersQuery, Result<PagedResult<UserDto>>>
    {
        public async Task<Result<PagedResult<UserDto>>> Handle(GetFilteredUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await repository.GetAllUsersAsync();
            var query = users.Data.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(u => u.Name.Contains(request.Name));
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                query = query.Where(u => u.Email.Contains(request.Email));
            }
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                query = query.Where(u => u.PhoneNumber.Contains(request.PhoneNumber));
            }

            // Apply paging
            var pagedUsers = query.ApplyPaging(request.Page, request.PageSize);
            var usersDtos = mapper.Map<List<UserDto>>(pagedUsers);
            var pagedResult = new PagedResult<UserDto>(usersDtos, query.Count());
            return Result<PagedResult<UserDto>>.Success(pagedResult);
        }
    }
}
