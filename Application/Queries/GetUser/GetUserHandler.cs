using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Application.DTOs;
using Eventide.AuthService.Domain.Interfaces;
using MediatR;

namespace Eventide.AuthService.Application.Queries.GetUser;

public class GetUserHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);

        if (user is null)
            return Result<UserDto>.Failure("User not found");

        return Result<UserDto>.Success(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email.Value,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        });
    }
}