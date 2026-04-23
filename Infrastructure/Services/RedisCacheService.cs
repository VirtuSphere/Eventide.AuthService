using StackExchange.Redis;
using System.Text.Json;

namespace Eventide.AuthService.Infrastructure.Services;

public class RedisCacheService
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var db = _redis.GetDatabase();
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var json = await db.StringGetAsync(key);
        return json.HasValue ? JsonSerializer.Deserialize<T>(json!) : default;
    }

    public async Task RemoveAsync(string key)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }
}