using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Application.DTOs;
using Eventide.AuthService.Contracts.Events;
using Eventide.AuthService.Domain.Entities;
using Eventide.AuthService.Domain.Enums;
using Eventide.AuthService.Domain.Interfaces;
using Eventide.AuthService.Domain.ValueObjects;
using MassTransit;
using MediatR;

namespace Eventide.AuthService.Application.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, Result<AuthResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPublishEndpoint _publishEndpoint;

    public RegisterHandler(IUserRepository userRepository, ITokenService tokenService, IPublishEndpoint publishEndpoint)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result<AuthResultDto>> Handle(RegisterCommand request, CancellationToken ct)
    {
        var emailVO = Email.Create(request.Email);

        if (await _userRepository.ExistsByEmailAsync(emailVO, ct))
            return Result<AuthResultDto>.Failure("User with this email already exists");

        if (await _userRepository.ExistsByUsernameAsync(request.Username, ct))
            return Result<AuthResultDto>.Failure("This username is already taken");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.Create(request.Username, request.Email, passwordHash, UserRole.Player);

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.SetRefreshToken(refreshToken, _tokenService.GetRefreshTokenExpiryDate());

        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        // Публикуем событие
        await _publishEndpoint.Publish(new UserRegisteredEvent
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email.Value,
            CreatedAt = user.CreatedAt
        }, ct);

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