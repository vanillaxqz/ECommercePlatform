using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Authentication
{
    public class LoginUserCommand : IRequest<Result<UserDto>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
