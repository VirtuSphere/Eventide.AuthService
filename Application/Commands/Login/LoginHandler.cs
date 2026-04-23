using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Application.DTOs;
using Eventide.AuthService.Domain.Interfaces;
using Eventide.AuthService.Domain.ValueObjects;
using MediatR;

namespace Eventide.AuthService.Application.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResultDto>> Handle(LoginCommand request, CancellationToken ct)
    {
        var emailVO = Email.Create(request.Email);
        var user = await _userRepository.GetByEmailAsync(emailVO, ct);

        if (user is null)
            return Result<AuthResultDto>.Failure("Invalid email or password");

        if (!user.IsActive)
            return Result<AuthResultDto>.Failure("Your account is blocked");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
            return Result<AuthResultDto>.Failure("Invalid email or password");

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.SetRefreshToken(refreshToken, _tokenService.GetRefreshTokenExpiryDate());

        await _userRepository.UpdateAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        var accessToken = _tokenService.GenerateAccessToken(user);

        return Result<AuthResultDto>.Success(new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
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