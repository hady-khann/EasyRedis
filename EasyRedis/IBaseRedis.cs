using StackExchange.Redis;

namespace EasyRedis;
public interface IBaseRedis
{
    ConnectionMultiplexer redisConnection { get; set; }

    void Dispose();
    RedisSrvc SetDbLifeTime(Dictionary<RedisDb, TimeSpan> dbs);
    RedisSrvc SetDbLifeTime(RedisDb dbNumber, TimeSpan lifeTime);
    RedisSrvc SetDefultDb(RedisDb dbNumber);
    RedisSrvc SetDefultDbLifeTime();
    void SetRedisConfiguration(ConfigurationOptions config);
    IDatabase UseRedis(RedisDb dbNumber);
}