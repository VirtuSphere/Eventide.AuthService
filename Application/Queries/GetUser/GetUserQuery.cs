using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Application.DTOs;
using MediatR;

namespace Eventide.AuthService.Application.Queries.GetUser;

public class GetUserQuery : IRequest<Result<UserDto>>
{
    public Guid UserId { get; init; }
}