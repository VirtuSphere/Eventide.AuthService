using Eventide.AuthService.Domain.Entities;
using Eventide.AuthService.Domain.Interfaces;
using Eventide.AuthService.Domain.ValueObjects;
using Eventide.AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eventide.AuthService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken ct)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email.Value, ct);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct)
        => await _context.Users.FirstOrDefaultAsync(u => u.Username == username.ToLower(), ct);

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct)
        => await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken != null && u.RefreshToken.Token == refreshToken, ct);

    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct)
        => await _context.Users.AnyAsync(u => u.Email.Value == email.Value, ct);

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct)
        => await _context.Users.AnyAsync(u => u.Username == username.ToLower(), ct);

    public async Task AddAsync(User user, CancellationToken ct)
        => await _context.Users.AddAsync(user, ct);

    public Task UpdateAsync(User user, CancellationToken ct)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
}