using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.UserCommands
{
    public class CreateUserCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
