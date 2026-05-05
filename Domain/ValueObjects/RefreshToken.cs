using Eventide.AuthService.Domain.Exceptions;

namespace Eventide.AuthService.Domain.ValueObjects;

public class RefreshToken
{
    public string Token { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

    private RefreshToken() { }

    private RefreshToken(string token, DateTime expiryDate)
    {
        Token = token;
        ExpiryDate = expiryDate;
    }

    public static RefreshToken Create(string token, DateTime expiryDate)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException("Token cannot be empty");
        if (expiryDate <= DateTime.UtcNow)
            throw new DomainException("Expiry date must be in the future");

        return new RefreshToken(token, expiryDate);
    }
}