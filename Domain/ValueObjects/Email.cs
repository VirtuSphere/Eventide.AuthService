using Eventide.AuthService.Domain.Exceptions;

namespace Eventide.AuthService.Domain.ValueObjects;

public class Email
{
    public string Value { get; private set; }

    private Email() { }

    private Email(string value)
    {
        Value = value.ToLower().Trim();
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty");
        if (!email.Contains('@'))
            throw new DomainException("Invalid email format");

        return new Email(email);
    }

    public override bool Equals(object? obj)
        => obj is Email other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}