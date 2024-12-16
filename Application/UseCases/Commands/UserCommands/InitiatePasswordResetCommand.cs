using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.UserCommands
{
    public class InitiatePasswordResetCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }
    }
}
