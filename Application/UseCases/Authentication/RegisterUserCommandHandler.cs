﻿using Application.UseCases.Commands.UserCommands;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.UserCommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        public RegisterUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(request);
            if (user == null)
            {
                return Result<Guid>.Failure("Failure");
            }
            return await repository.AddUserAsync(user);
        }
    }
}