using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Application.DTOs;
using MediatR;

namespace Eventide.AuthService.Application.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<AuthResultDto>>
{
    public string RefreshToken { get; init; } = string.Empty;
}