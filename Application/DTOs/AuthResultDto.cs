namespace Eventide.AuthService.Application.DTOs;

public class AuthResultDto
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public UserDto User { get; init; } = null!;
}