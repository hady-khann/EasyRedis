using StackExchange.Redis;

namespace EasyRedis;
public interface IBaseRedis
{
    IDatabase redis { get; set; }
    ConnectionMultiplexer redisConnection { get; set; }

    void Dispose();
    BaseRedis SetDbLifeTime(Dictionary<RedisDb, TimeSpan> dbs);
    BaseRedis SetDbLifeTime(RedisDb dbNumber, TimeSpan lifeTime);
    BaseRedis SetDefultDb(RedisDb dbNumber);
    BaseRedis SetDefultDbLifeTime();
    void SetRedisConfiguration(ConfigurationOptions config);
}