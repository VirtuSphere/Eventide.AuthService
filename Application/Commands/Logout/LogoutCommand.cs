using Eventide.AuthService.Application.Common;
using MediatR;

namespace Eventide.AuthService.Application.Commands.Logout;

public class LogoutCommand : IRequest<Result>
{
    public Guid UserId { get; init; }
}