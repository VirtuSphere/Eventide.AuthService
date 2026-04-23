using Eventide.AuthService.Domain.Entities;

namespace Eventide.AuthService.Domain.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiryDate();
}