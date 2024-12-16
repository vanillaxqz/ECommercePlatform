using MediatR;
using Domain.Common;

namespace Application.UseCases.Commands.UserCommands
{
    public class ResetPasswordCommand : IRequest<Result<string>>
    {
        public string NewPassword { get; set; }
    }
}
