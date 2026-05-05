namespace Eventide.AuthService.Contracts.DTOs;

public class TokenValidationResponse
{
    public bool IsValid { get; init; }
    public Guid? UserId { get; init; }
    public string? Username { get; init; }
    public string? Role { get; init; }
}