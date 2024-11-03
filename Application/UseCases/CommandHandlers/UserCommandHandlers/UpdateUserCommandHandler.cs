using Application.UseCases.Commands.UserCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.UserCommandHandlers
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var task = mapper.Map<User>(request);
            await repository.UpdateUserAsync(task);
            return Unit.Value;
        }
    }
}
