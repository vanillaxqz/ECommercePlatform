using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure;

namespace InfrastructureTests
{
    public class JwtTokenGeneratorTests
    {
        private readonly string _jwtSecret = "supersecretkeysupersecretkeysupersecretkey";
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public JwtTokenGeneratorTests()
        {
            _jwtTokenGenerator = new JwtTokenGenerator(_jwtSecret);
        }

        [Fact]
        public void GivenValidUserIdAndEmail_WhenGeneratingAccessToken_ThenShouldReturnValidToken()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "testemail@gmail.com";

            // Act
            var token = _jwtTokenGenerator.GenerateAccessToken(userId, email);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            // Assert
            tokenString.Should().NotBeNullOrEmpty();
            token.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString());
            token.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == email);
        }

        [Fact]
        public void GivenValidUserId_WhenGeneratingPasswordResetToken_ThenShouldReturnValidToken()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var token = _jwtTokenGenerator.GeneratePasswordResetToken(userId);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            // Assert
            tokenString.Should().NotBeNullOrEmpty();
            token.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString());
            token.Claims.Should().Contain(c => c.Type == "TokenType" && c.Value == "PasswordReset");
        }
    }
}
