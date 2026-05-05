namespace Eventide.AuthService.Contracts.DTOs;

public class TokenValidationRequest
{
    public string AccessToken { get; init; } = string.Empty;
}