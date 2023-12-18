using StackExchange.Redis;

namespace EasyRedis;
public interface IRedisSrvc
{
    void Dispose();
    T? HashGet<T>(RedisDbEnum dbNumber, string key, string hashField);
    T? HashGetAll<T>(RedisDbEnum dbNumber, string key);
    Task<T?> HashGetAllAsync<T>(RedisDbEnum dbNumber, string key);
    Task<T?> HashGetAsync<T>(RedisDbEnum dbNumber, string key, string hashField);
    long HashLength(RedisDbEnum dbNumber, string key);
    Task<long> HashLengthAsync(RedisDbEnum dbNumber, string key);
    void HashSet(RedisDbEnum dbNumber, string key, HashEntry[] hashFields);
    Task HashSetAsync(RedisDbEnum dbNumber, string key, HashEntry[] hashFields);
    bool KeyCopy(RedisDbEnum srcDbNumber, RedisDbEnum destDbNumber, string srcKey, string destKey);
    bool KeyCopy(RedisDbEnum srcDbNumber, string srcKey, string destKey);
    Task<bool> KeyCopyAsync(RedisDbEnum srcDbNumber, RedisDbEnum destDbNumber, string srcKey, string destKey);
    Task<bool> KeyCopyAsync(RedisDbEnum srcDbNumber, string srcKey, string destKey);
    bool KeyDelete(RedisDbEnum dbNumber, string key);
    Task<bool> KeyDeleteAsync(RedisDbEnum dbNumber, string key);
    bool KeyExists(RedisDbEnum dbNumber, string key);
    Task<bool> KeyExistsAsync(RedisDbEnum dbNumber, string key);
    bool KeyExpire(RedisDbEnum dbNumber, string key, TimeSpan lifeTime);
    Task<bool> KeyExpireAsync(RedisDbEnum dbNumber, string key, TimeSpan lifeTime);
    DateTime? KeyExpireTime(RedisDbEnum dbNumber, string key);
    Task<DateTime?> KeyExpireTimeAsync(RedisDbEnum dbNumber, string key);
    bool KeyMove(RedisDbEnum srcDbNumber, RedisDbEnum destDbNumber, string key);
    Task<bool> KeyMoveAsync(RedisDbEnum srcDbNumber, RedisDbEnum destDbNumber, string key);
    TimeSpan? KeyTimeToLive(RedisDbEnum dbNumber, string key);
    Task<TimeSpan?> KeyTimeToLiveAsync(RedisDbEnum dbNumber, string key);
    bool SetAdd(RedisDbEnum dbNumber, string key, object value);
    Task<bool> SetAddAsync(RedisDbEnum dbNumber, string key, object value);
    T? SetAndGet<T>(RedisDbEnum dbNumber, string key, object value);
    Task<T?> SetAndGetAsync<T>(RedisDbEnum dbNumber, string key, object value);
    long SetLength(RedisDbEnum dbNumber, string key);
    Task<long> SetLengthAsync(RedisDbEnum dbNumber, string key);
    RedisValue[] SetMembers(RedisDbEnum dbNumber, string key, object value);
    Task<RedisValue[]> SetMembersAsync(RedisDbEnum dbNumber, string key, object value);
    bool SetMove(RedisDbEnum dbNumber, string srcKey, string destKey, object value);
    Task<bool> SetMoveAsync(RedisDbEnum dbNumber, string srcKey, string destKey, object value);
    string SetPop(RedisDbEnum dbNumber, string srcKey);
    Task<string> SetPopAsync(RedisDbEnum dbNumber, string srcKey);
    T? StringGet<T>(RedisDbEnum dbNumber, string key);
    Task<T?> StringGetAsync<T>(RedisDbEnum dbNumber, string key);
    bool StringSet(RedisDbEnum dbNumber, string key, object value);
    Task<bool> StringSetAsync(RedisDbEnum dbNumber, string key, object value);
}