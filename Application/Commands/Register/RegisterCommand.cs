using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Application.DTOs;
using MediatR;

namespace Eventide.AuthService.Application.Commands.Register;

public class RegisterCommand : IRequest<Result<AuthResultDto>>
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}