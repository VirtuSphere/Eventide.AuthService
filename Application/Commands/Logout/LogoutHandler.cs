using Eventide.AuthService.Application.Common;
using Eventide.AuthService.Domain.Interfaces;
using MediatR;

namespace Eventide.AuthService.Application.Commands.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public LogoutHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);

        if (user is null)
            return Result.Failure("User not found");

        user.RemoveRefreshToken();
        await _userRepository.SaveChangesAsync(ct);

        return Result.Success();
    }
}