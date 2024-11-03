using Application.DTOs;
using Application.UseCases.Queries.UserQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.UserQueryHandlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        public GetAllUsersQueryHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await repository.GetAllUsersAsync();
            return mapper.Map<Result<IEnumerable<UserDto>>>(users);
        }
    }
}
