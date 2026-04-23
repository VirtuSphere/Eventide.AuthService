using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Application.DTOs;
using Eventide.AuthService.Domain.Interfaces;
using MediatR;

namespace Eventide.AuthService.Application.Commands.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResultDto>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, ct);

        if (user is null)
            return Result<AuthResultDto>.Failure("Invalid refresh token");

        if (user.RefreshToken is null || user.RefreshToken.IsExpired)
        {
            user.RemoveRefreshToken();
            await _userRepository.SaveChangesAsync(ct);
            return Result<AuthResultDto>.Failure("Refresh token expired. Please login again");
        }

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.SetRefreshToken(newRefreshToken, _tokenService.GetRefreshTokenExpiryDate());

        await _userRepository.SaveChangesAsync(ct);

        var accessToken = _tokenService.GenerateAccessToken(user);

        return Result<AuthResultDto>.Success(new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email.Value,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            }
        });
    }
}