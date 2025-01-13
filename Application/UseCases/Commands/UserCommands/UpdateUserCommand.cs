using MediatR;

namespace Application.UseCases.Commands.UserCommands
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
