using StackExchange.Redis;

namespace EasyRedis;
public interface IRedisSrvc : IBaseRedis
{

    T HashGet<T>(string key, string hashField, RedisDb dbNumber = RedisDb.DbDefult);
    T HashGetAll<T>(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<T> HashGetAllAsync<T>(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<T> HashGetAsync<T>(string key, string hashField, RedisDb dbNumber = RedisDb.DbDefult);
    long HashLength(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<long> HashLengthAsync(string key, RedisDb dbNumber = RedisDb.DbDefult);
    void HashSet(string key, HashEntry[] hashFields, RedisDb dbNumber = RedisDb.DbDefult);
    Task HashSetAsync(string key, HashEntry[] hashFields, RedisDb dbNumber = RedisDb.DbDefult);
    bool KeyCopy(RedisDb srcDbNumber, string srcKey, string destKey);
    bool KeyCopy(string srcKey, string destKey, RedisDb destDbNumber, RedisDb srcDbNumber = RedisDb.DbDefult);
    Task<bool> KeyCopyAsync(RedisDb srcDbNumber, RedisDb destDbNumber, string srcKey, string destKey);
    Task<bool> KeyCopyAsync(RedisDb srcDbNumber, string srcKey, string destKey);
    bool KeyDelete(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<bool> KeyDeleteAsync(string key, RedisDb dbNumber = RedisDb.DbDefult);
    bool KeyExists(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<bool> KeyExistsAsync(string key, RedisDb dbNumber = RedisDb.DbDefult);
    bool KeyExpire(string key, TimeSpan lifeTime, RedisDb dbNumber = RedisDb.DbDefult);
    Task<bool> KeyExpireAsync(string key, TimeSpan lifeTime, RedisDb dbNumber = RedisDb.DbDefult);
    DateTime? KeyExpireTime(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<DateTime?> KeyExpireTimeAsync(string key, RedisDb dbNumber = RedisDb.DbDefult);
    bool KeyMove(RedisDb srcDbNumber, RedisDb destDbNumber, string key);
    Task<bool> KeyMoveAsync(RedisDb srcDbNumber, RedisDb destDbNumber, string key);
    TimeSpan? KeyTimeToLive(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<TimeSpan?> KeyTimeToLiveAsync(string key, RedisDb dbNumber = RedisDb.DbDefult);
    bool SetAdd(string key, object value, RedisDb dbNumber = RedisDb.DbDefult);
    Task<bool> SetAddAsync(string key, object value, RedisDb dbNumber = RedisDb.DbDefult);
    T SetAndGet<T>(string key, object value, RedisDb dbNumber = RedisDb.DbDefult);
    Task<T> SetAndGetAsync<T>(string key, object value, RedisDb dbNumber = RedisDb.DbDefult);
    long SetLength(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<long> SetLengthAsync(string key, RedisDb dbNumber = RedisDb.DbDefult);
    RedisValue[] SetMembers(string key, object value, RedisDb dbNumber = RedisDb.DbDefult);
    Task<RedisValue[]> SetMembersAsync(string key, RedisDb dbNumber = RedisDb.DbDefult);
    bool SetMove(string srcKey, string destKey, object value, RedisDb dbNumber = RedisDb.DbDefult);
    Task<bool> SetMoveAsync(string srcKey, string destKey, object value, RedisDb dbNumber = RedisDb.DbDefult);
    string SetPop(string srcKey, RedisDb dbNumber = RedisDb.DbDefult);
    Task<string> SetPopAsync(string srcKey, RedisDb dbNumber = RedisDb.DbDefult);
    T StringGet<T>(string key, RedisDb dbNumber = RedisDb.DbDefult);
    Task<T> StringGetAsync<T>(string key, RedisDb dbNumber = RedisDb.DbDefult);
    bool StringSet(string key, object value, RedisDb dbNumber = RedisDb.DbDefult);
    Task<bool> StringSetAsync(string key, object value, RedisDb dbNumber = RedisDb.DbDefult);
}