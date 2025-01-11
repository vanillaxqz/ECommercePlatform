using Application.DTOs;
using Application.UseCases.Authentication;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.UseCases.Authentication
{
    public class LoginUserCommandHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new LoginUserCommandHandler(_userRepository, _mapper);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldReturnSuccessResult()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "testuser@gmail.com",
                Password = "password123"
            };

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = "testuser@gmail.com",
                Password = "password123"
            };

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Password = user.Password
            };

            var result = Result<User>.Success(user);

            _mapper.Map<User>(command).Returns(user);
            _userRepository.LoginUser(user).Returns(result);
            _mapper.Map<Result<UserDto>>(result).Returns(Result<UserDto>.Success(userDto));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Data.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task Handle_GivenInvalidRequest_ShouldReturnFailureResult()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "invaliduser@gmail.com",
                Password = "wrongpassword"
            };

            var user = new User
            {
                Email = "invaliduser@gmail.com",
                Password = "wrongpassword"
            };

            var result = Result<User>.Failure("Invalid credentials");

            _mapper.Map<User>(command).Returns(user);
            _userRepository.LoginUser(user).Returns(result);
            _mapper.Map<Result<UserDto>>(result).Returns(Result<UserDto>.Failure("Invalid credentials"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.ErrorMessage.Should().Be("Invalid credentials");
        }
    }
}