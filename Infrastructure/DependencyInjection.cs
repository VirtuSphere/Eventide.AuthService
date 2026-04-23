using Eventide.AuthService.Domain.Interfaces;
using Eventide.AuthService.Infrastructure.Data;
using Eventide.AuthService.Infrastructure.Repositories;
using Eventide.AuthService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eventide.AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("AuthDb")));

        services.AddSingleton(new RedisCacheService(config.GetConnectionString("Redis")!));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}