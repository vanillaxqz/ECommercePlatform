using Application.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
namespace Application.UseCases.Authentication
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        public LoginUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<UserDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var task = mapper.Map<User>(request);
            var response = await repository.LoginUser(task);
            return mapper.Map<Result<UserDto>>(response);
        }
    }
}
