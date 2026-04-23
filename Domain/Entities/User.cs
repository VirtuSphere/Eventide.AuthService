using Eventide.AuthService.Domain.Enums;
using Eventide.AuthService.Domain.Exceptions;
using Eventide.AuthService.Domain.ValueObjects;

namespace Eventide.AuthService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public RefreshToken? RefreshToken { get; private set; }

    private User() { }

    public static User Create(string username, string email, string passwordHash, UserRole role = UserRole.Player)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty");
        if (username.Length < 3)
            throw new DomainException("Username must be at least 3 characters");
        if (username.Length > 30)
            throw new DomainException("Username must be at most 30 characters");

        return new User
        {
            Id = Guid.NewGuid(),
            Username = username.ToLower().Trim(),
            Email = Email.Create(email),
            PasswordHash = passwordHash,
            Role = role,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void SetRefreshToken(string token, DateTime expiryDate)
        => RefreshToken = RefreshToken.Create(token, expiryDate);

    public void RemoveRefreshToken()
        => RefreshToken = null;

    public void Deactivate()
    {
        if (!IsActive) throw new DomainException("User already deactivated");
        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive) throw new DomainException("User already active");
        IsActive = true;
    }
}