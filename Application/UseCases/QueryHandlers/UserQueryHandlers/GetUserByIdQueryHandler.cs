using Application.DTOs;
using Application.UseCases.Queries.UserQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.UserQueryHandlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        public GetUserByIdQueryHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await repository.GetUserByIdAsync(request.Id);
            return mapper.Map<Result<UserDto>>(user);
        }
    }
}
