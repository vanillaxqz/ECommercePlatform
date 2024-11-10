using Application.UseCases.Commands.UserCommands;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.UserCommandHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository repository;
        public DeleteUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteUserAsync(request.Id);
            return Unit.Value;
        }
    }
}
