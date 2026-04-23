using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Application.DTOs;
using MediatR;

namespace Eventide.AuthService.Application.Commands.Login;

public class LoginCommand : IRequest<Result<AuthResultDto>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}