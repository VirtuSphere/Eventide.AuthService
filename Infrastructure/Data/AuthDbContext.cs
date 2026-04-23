using Eventide.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eventide.AuthService.Infrastructure.Data;

public class AuthDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).IsRequired().HasMaxLength(30);
            builder.HasIndex(u => u.Username).IsUnique();

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value).HasColumnName("Email").IsRequired().HasMaxLength(100);
                email.HasIndex(e => e.Value).IsUnique();
            });

            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).HasConversion<string>().IsRequired();

            builder.OwnsOne(u => u.RefreshToken, rt =>
            {
                rt.Property(r => r.Token).HasColumnName("RefreshToken").HasMaxLength(500);
                rt.Property(r => r.ExpiryDate).HasColumnName("RefreshTokenExpiryDate");
            });

            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();
        });
    }
}