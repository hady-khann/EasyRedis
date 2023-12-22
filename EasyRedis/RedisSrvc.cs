using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EasyRedis;
public class RedisSrvc : BaseRedis, IRedisSrvc
{
    public RedisSrvc(IConfiguration configuration) : base(configuration) { }


    #region Basic Operstions

    #region StringSet

    /// <summary>
    /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The value to set.</param>
    /// <returns><see langword="true"/> if the string was set, <see langword="false"/> otherwise.</returns>
    public bool StringSet(string key, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        GetRedisLifetime(dbNumber);

        return redis.StringSet(key, JsonConvert.SerializeObject(value), _lifeTime);
    }

    /// <summary>
    /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The value to set.</param>
    /// <returns><see langword="true"/> if the string was set, <see langword="false"/> otherwise.</returns>
    public async Task<bool> StringSetAsync(string key, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        GetRedisLifetime(dbNumber);

        return await redis.StringSetAsync(key, JsonConvert.SerializeObject(value), _lifeTime);
    }

    #endregion

    #region StringGet

    /// <summary>
    /// Get the value of key. If the key does not exist the special value nil is returned.
    /// An error is returned if the value stored at key is not a string, because GET only handles string values.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key of the string.</param>
    /// <typeparam name="T">Type Of Object Stored In Db</typeparam>
    /// <returns>The value of key, or nil when key does not exist.</returns>
    public T StringGet<T>(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return JsonConvert.DeserializeObject<T>(redis.StringGet(key)!);
    }

    /// <summary>
    /// Get the value of key. If the key does not exist the special value nil is returned.
    /// An error is returned if the value stored at key is not a string, because GET only handles string values.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key of the string.</param>
    /// <typeparam name="T">Type Of Object Stored In Db</typeparam>
    /// <returns>The value of key, or nil when key does not exist.</returns>
    public async Task<T> StringGetAsync<T>(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return JsonConvert.DeserializeObject<T>((await redis.StringGetAsync(key))!);
    }

    #endregion

    #region SetAndGet

    /// <summary>
    /// Atomically sets key to value and returns the previous value (if any) stored at <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The value to set.</param>
    /// <typeparam name="T">Type Of Object Stored In Db</typeparam>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The previous value stored at <paramref name="key"/>, or nil when key did not exist.</returns>
    public async Task<T> SetAndGetAsync<T>(string key, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        GetRedisLifetime(dbNumber);

        return JsonConvert.DeserializeObject<T>(
            (await redis.StringSetAndGetAsync(key.ToString(), JsonConvert.SerializeObject(value), _lifeTime))!
            );
    }

    /// <summary>
    /// Atomically sets key to value and returns the previous value (if any) stored at <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The value to set.</param>
    /// <typeparam name="T">Type Of Object Stored In Db</typeparam>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The previous value stored at <paramref name="key"/>, or nil when key did not exist.</returns>
    public T SetAndGet<T>(string key, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        GetRedisLifetime(dbNumber);

        return JsonConvert.DeserializeObject<T>(
            redis.StringSetAndGet(key.ToString(), JsonConvert.SerializeObject(value), _lifeTime)!
            );
    }

    #endregion

    #endregion


    #region Key Operations

    #region KeyDelete

    /// <summary>
    /// Removes the specified key. A key is ignored if it does not exist.
    /// If UNLINK is available (Redis 4.0+), it will be used.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key to delete.</param>
    /// <returns><see langword="true"/> if the key was removed.</returns>
    public bool KeyDelete(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return redis.KeyDelete(key);
    }

    /// <summary>
    /// Removes the specified key. A key is ignored if it does not exist.
    /// If UNLINK is available (Redis 4.0+), it will be used.
    /// </summary>
    /// <param name="key">The key to delete.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns><see langword="true"/> if the key was removed.</returns>
    public async Task<bool> KeyDeleteAsync(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return await redis.KeyDeleteAsync(key);
    }

    #endregion

    #region KeyExpire

    /// <summary>
    /// Set a timeout on <paramref name="key"/>.
    /// After the timeout has expired, the key will automatically be deleted.
    /// A key with an associated timeout is said to be volatile in Redis terminology.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key to delete.</param>
    /// <param name="lifeTime">The timeout to set.</param>
    /// <returns><see langword="true"/> if the timeout was set. <see langword="false"/> if key does not exist or the timeout could not be set.</returns>
    public bool KeyExpire(string key, TimeSpan lifeTime, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return redis.KeyExpire(key, lifeTime);
    }

    /// <summary>
    /// Set a timeout on <paramref name="key"/>.
    /// After the timeout has expired, the key will automatically be deleted.
    /// A key with an associated timeout is said to be volatile in Redis terminology.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key to delete.</param>
    /// <param name="lifeTime">The timeout to set.</param>
    /// <returns><see langword="true"/> if the timeout was set. <see langword="false"/> if key does not exist or the timeout could not be set.</returns>
    public async Task<bool> KeyExpireAsync(string key, TimeSpan lifeTime, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return await redis.KeyExpireAsync(key, lifeTime);
    }

    #endregion

    #region KeyExists

    /// <summary>
    /// Returns if key exists.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns><see langword="true"/> if the key exists. <see langword="false"/> if the key does not exist.</returns>
    public bool KeyExists(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return redis.KeyExists(key);
    }

    /// <summary>
    /// Returns if key exists.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns><see langword="true"/> if the key exists. <see langword="false"/> if the key does not exist.</returns>
    public async Task<bool> KeyExistsAsync(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return await redis.KeyExistsAsync(key);
    }

    #endregion

    #region KeyExpireTime

    /// <summary>
    /// Returns the absolute time at which the given <paramref name="key"/> will expire, if it exists and has an expiration.
    /// </summary>
    /// <param name="key">The key to get the expiration for.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The time at which the given key will expire, or <see langword="null"/> if the key does not exist or has no associated expiration time.</returns>
    public DateTime? KeyExpireTime(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return redis.KeyExpireTime(key);
    }

    /// <summary>
    /// Returns the absolute time at which the given <paramref name="key"/> will expire, if it exists and has an expiration.
    /// </summary>
    /// <param name="key">The key to get the expiration for.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The time at which the given key will expire, or <see langword="null"/> if the key does not exist or has no associated expiration time.</returns>
    public async Task<DateTime?> KeyExpireTimeAsync(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return await redis.KeyExpireTimeAsync(key);
    }

    #endregion

    #region KeyTimeToLive

    /// <summary>
    /// Returns the remaining time to live of a key that has a timeout.
    /// This introspection capability allows a Redis client to check how many seconds a given key will continue to be part of the dataset.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key to check.</param>
    /// <returns>TTL, or nil when key does not exist or does not have a timeout.</returns>
    public TimeSpan? KeyTimeToLive(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return redis.KeyTimeToLive(key);
    }

    /// <summary>
    /// Returns the remaining time to live of a key that has a timeout.
    /// This introspection capability allows a Redis client to check how many seconds a given key will continue to be part of the dataset.
    /// </summary>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <param name="key">The key to check.</param>
    /// <returns>TTL, or nil when key does not exist or does not have a timeout.</returns>
    public async Task<TimeSpan?> KeyTimeToLiveAsync(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);
        return await redis.KeyTimeToLiveAsync(key);
    }

    #endregion

    #region KeyCopy

    /// <summary>
    /// Copies the value from the <paramref name="srcKey"/> to the specified <paramref name="destKey"/>.
    /// </summary>
    /// <param name="srcKey">The key of the source value to copy.</param>
    /// <param name="destKey">The destination key to copy the source to.</param>
    /// <param name="srcDbNumber">The database ID to Copy <paramref name="srcKey"/> From.</param>
    /// <param name="destDbNumber">The database ID to store <paramref name="destKey"/> in.</param>
    /// <returns><see langword="true"/> if key was copied. <see langword="false"/> if key was not copied.</returns>
    public bool KeyCopy(string srcKey, string destKey, RedisDb destDbNumber, RedisDb srcDbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(srcDbNumber);
        return redis.KeyCopy(srcKey, destKey, (int)destDbNumber);
    }

    /// <summary>
    /// Copies the value from the <paramref name="srcKey"/> to the specified <paramref name="destKey"/>.
    /// </summary>
    /// <param name="srcKey">The key of the source value to copy.</param>
    /// <param name="destKey">The destination key to copy the source to.</param>
    /// <param name="destDbNumber">The database ID to store <paramref name="destKey"/> in.</param>
    /// <returns><see langword="true"/> if key was copied. <see langword="false"/> if key was not copied.</returns>
    public bool KeyCopy(RedisDb srcDbNumber, string srcKey, string destKey)
    {
        GetRedisDatabase(srcDbNumber);
        return redis.KeyCopy(srcKey, destKey);
    }

    /// <summary>
    /// Copies the value from the <paramref name="srcKey"/> to the specified <paramref name="destKey"/>.
    /// </summary>
    /// <param name="srcKey">The key of the source value to copy.</param>
    /// <param name="destKey">The destination key to copy the source to.</param>
    /// <param name="srcDbNumber">The database ID to Copy <paramref name="srcKey"/> From.</param>
    /// <param name="destDbNumber">The database ID to store <paramref name="destKey"/> in.</param>
    /// <returns><see langword="true"/> if key was copied. <see langword="false"/> if key was not copied.</returns>
    public async Task<bool> KeyCopyAsync(RedisDb srcDbNumber, RedisDb destDbNumber, string srcKey, string destKey)
    {
        GetRedisDatabase(srcDbNumber);
        return await redis.KeyCopyAsync(srcKey, destKey, (int)destDbNumber);
    }

    /// <summary>
    /// Copies the value from the <paramref name="srcKey"/> to the specified <paramref name="destKey"/>.
    /// </summary>
    /// <param name="srcKey">The key of the source value to copy.</param>
    /// <param name="destKey">The destination key to copy the source to.</param>
    /// <param name="destDbNumber">The database ID to store <paramref name="destKey"/> in.</param>
    /// <returns><see langword="true"/> if key was copied. <see langword="false"/> if key was not copied.</returns>
    public async Task<bool> KeyCopyAsync(RedisDb srcDbNumber, string srcKey, string destKey)
    {
        GetRedisDatabase(srcDbNumber);
        return await redis.KeyCopyAsync(srcKey, destKey);
    }

    #endregion

    #region KeyMove

    /// <summary>
    /// Move key from the currently selected database (see SELECT) to the specified destination database.
    /// When key already exists in the destination database, or it does not exist in the source database, it does nothing.
    /// It is possible to use MOVE as a locking primitive because of this.
    /// </summary>
    /// <param name="key">The key to move.</param>
    /// <param name="destDbNumber">The database to move the key to.</param>
    /// <param name="srcDbNumber">The database ID to Copy <paramref name="srcKey"/> From.</param>
    /// <returns><see langword="true"/> if key was moved. <see langword="false"/> if key was not moved.</returns>
    public bool KeyMove(RedisDb srcDbNumber, RedisDb destDbNumber, string key)
    {
        GetRedisDatabase(srcDbNumber);
        return redis.KeyMove(key, (int)destDbNumber);
    }

    /// <summary>
    /// Move key from the currently selected database (see SELECT) to the specified destination database.
    /// When key already exists in the destination database, or it does not exist in the source database, it does nothing.
    /// It is possible to use MOVE as a locking primitive because of this.
    /// </summary>
    /// <param name="key">The key to move.</param>
    /// <param name="destDbNumber">The database to move the key to.</param>
    /// <param name="srcDbNumber">The database ID to Copy <paramref name="srcKey"/> From.</param>
    /// <returns><see langword="true"/> if key was moved. <see langword="false"/> if key was not moved.</returns>
    public Task<bool> KeyMoveAsync(RedisDb srcDbNumber, RedisDb destDbNumber, string key)
    {
        GetRedisDatabase(srcDbNumber);
        return redis.KeyMoveAsync(key, (int)destDbNumber);
    }

    #endregion

    #endregion


    #region Hash Operstions

    #region HashSet

    /// <summary>
    /// Sets the specified fields to their respective values in the hash stored at key.
    /// This command overwrites any specified fields that already exist in the hash, leaving other unspecified fields untouched.
    /// If key does not exist, a new key holding a hash is created.
    /// </summary>
    /// <param name="key">The key of the hash.</param>
    /// <param name="hashFields">The entries to set in the hash.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    public void HashSet(string key, HashEntry[] hashFields, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        redis.HashSet(key, hashFields);
    }

    /// <summary>
    /// Sets the specified fields to their respective values in the hash stored at key.
    /// This command overwrites any specified fields that already exist in the hash, leaving other unspecified fields untouched.
    /// If key does not exist, a new key holding a hash is created.
    /// </summary>
    /// <param name="key">The key of the hash.</param>
    /// <param name="hashFields">The entries to set in the hash.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    public async Task HashSetAsync(string key, HashEntry[] hashFields, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        await redis.HashSetAsync(key, hashFields);
    }

    #endregion

    #region HashGet

    /// <summary>
    /// Returns the value associated with field in the hash stored at key.
    /// </summary>
    /// <param name="key">The key of the hash.</param>
    /// <param name="hashField">The field in the hash to get.</param>
    /// <typeparam name="T">Type Of Object Stored In Db</typeparam>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The value associated with field, or nil when field is not present in the hash or key does not exist.</returns>
    public T HashGet<T>(string key, string hashField, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return (T)Convert.ChangeType(redis.HashGet(key, hashField)!, typeof(T));
    }

    /// <summary>
    /// Returns the value associated with field in the hash stored at key.
    /// </summary>
    /// <param name="key">The key of the hash.</param>
    /// <param name="hashField">The field in the hash to get.</param>
    /// <typeparam name="T">Type Of Object Stored In Db</typeparam>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The value associated with field, or nil when field is not present in the hash or key does not exist.</returns>
    public async Task<T> HashGetAsync<T>(string key, string hashField, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return (T)Convert.ChangeType(await redis.HashGetAsync(key, hashField)!, typeof(T));
    }

    #endregion

    #region HashGetAll

    /// <summary>
    /// Returns all fields and values of the hash stored at key.
    /// </summary>
    /// <param name="key">The key of the hash to get all entries from.</param>
    /// <typeparam name="T">Type Of Object Stored In Db</typeparam>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>List of fields and their values stored in the hash, or an empty list when key does not exist.</returns>
    public T HashGetAll<T>(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return (T)Convert.ChangeType(redis.HashGetAll(key)!, typeof(T));
    }

    /// <summary>
    /// Returns the value associated with field in the hash stored at key.
    /// </summary>
    /// <param name="key">The key of the hash.</param>
    /// <typeparam name="T">Type Of Object Stored In Db</typeparam>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The value associated with field, or nil when field is not present in the hash or key does not exist.</returns>
    public async Task<T> HashGetAllAsync<T>(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return (T)Convert.ChangeType(await redis.HashGetAllAsync(key)!, typeof(T));
    }

    #endregion

    #region HashLength

    /// <summary>
    /// Returns the number of fields contained in the hash stored at key.
    /// </summary>
    /// <param name="key">The key of the hash.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The number of fields in the hash, or 0 when key does not exist.</returns>
    public long HashLength(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return redis.HashLength(key);
    }

    /// <summary>
    /// Returns the number of fields contained in the hash stored at key.
    /// </summary>
    /// <param name="key">The key of the hash.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The number of fields in the hash, or 0 when key does not exist.</returns>
    public Task<long> HashLengthAsync(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return redis.HashLengthAsync(key);
    }

    #endregion

    #endregion


    #region Set Operstions

    #region SetAdd

    /// <summary>
    /// Add the specified member to the set stored at key.
    /// Specified members that are already a member of this set are ignored.
    /// If key does not exist, a new set is created before adding the specified members.
    /// </summary>
    /// <param name="key">The key of the set.</param>
    /// <param name="value">The value to add to the set.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns><see langword="true"/> if the specified member was not already present in the set, else <see langword="false"/>.</returns>
    public bool SetAdd(string key, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return redis.SetAdd(key, JsonConvert.SerializeObject(value));
    }

    /// <summary>
    /// Add the specified member to the set stored at key.
    /// Specified members that are already a member of this set are ignored.
    /// If key does not exist, a new set is created before adding the specified members.
    /// </summary>
    /// <param name="key">The key of the set.</param>
    /// <param name="value">The value to add to the set.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns><see langword="true"/> if the specified member was not already present in the set, else <see langword="false"/>.</returns>
    public Task<bool> SetAddAsync(string key, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return redis.SetAddAsync(key, JsonConvert.SerializeObject(value));
    }

    #endregion

    #region SetMembers

    /// <summary>
    /// Returns all the members of the set value stored at key.
    /// </summary>
    /// <param name="key">The key of the set.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>All elements of the set.</returns>
    public RedisValue[] SetMembers(string key, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return redis.SetMembers(key);
    }

    /// <summary>
    /// Returns all the members of the set value stored at key.
    /// </summary>
    /// <param name="key">The key of the set.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>All elements of the set.</returns>
    public async Task<RedisValue[]> SetMembersAsync(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return await redis.SetMembersAsync(key);
    }

    #endregion

    #region SetLength

    /// <summary>
    /// Returns all the members of the set value stored at key.
    /// </summary>
    /// <param name="key">The key of the set.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>All elements of the set.</returns>
    public long SetLength(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return redis.SetLength(key);
    }

    /// <summary>
    /// Returns all the members of the set value stored at key.
    /// </summary>
    /// <param name="key">The key of the set.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>All elements of the set.</returns>
    public async Task<long> SetLengthAsync(string key, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return await redis.SetLengthAsync(key);
    }

    #endregion

    #region SetMove

    /// <summary>
    /// Move member from the set at source to the set at destination.
    /// This operation is atomic. In every given moment the element will appear to be a member of source or destination for other clients.
    /// When the specified element already exists in the destination set, it is only removed from the source set.
    /// </summary>
    /// <param name="srcKey">The key of the source set.</param>
    /// <param name="destKey">The key of the destination set.</param>
    /// <param name="value">The value to move.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>
    /// <see langword="true"/> if the element is moved.
    /// <see langword="false"/> if the element is not a member of source and no operation was performed.
    /// </returns>
    public bool SetMove(string srcKey, string destKey, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return redis.SetMove(srcKey, destKey, JsonConvert.SerializeObject(value));
    }

    /// <summary>
    /// Move member from the set at source to the set at destination.
    /// This operation is atomic. In every given moment the element will appear to be a member of source or destination for other clients.
    /// When the specified element already exists in the destination set, it is only removed from the source set.
    /// </summary>
    /// <param name="srcKey">The key of the source set.</param>
    /// <param name="destKey">The key of the destination set.</param>
    /// <param name="value">The value to move.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>
    /// <see langword="true"/> if the element is moved.
    /// <see langword="false"/> if the element is not a member of source and no operation was performed.
    /// </returns>
    public async Task<bool> SetMoveAsync(string srcKey, string destKey, object value, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return await redis.SetMoveAsync(srcKey, destKey, JsonConvert.SerializeObject(value));
    }

    #endregion

    #region SetPop

    /// <summary>
    /// Removes and returns a random element from the set value stored at key.
    /// </summary>
    /// <param name="key">The key of the set.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The removed element, or nil when key does not exist.</returns>
    public string SetPop(string srcKey, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return redis.SetPop(srcKey);
    }

    /// <summary>
    /// Removes and returns a random element from the set value stored at key.
    /// </summary>
    /// <param name="key">The key of the set.</param>
    /// <param name="dbNumber">dbNumber Is Db 0 By Defult Except Change Defult In <see cref="ChangeDefultDb"/> </param>
    /// <returns>The removed element, or nil when key does not exist.</returns>
    public async Task<string> SetPopAsync(string srcKey, RedisDb dbNumber = RedisDb.DbDefult)
    {
        GetRedisDatabase(dbNumber);

        return await redis.SetPopAsync(srcKey);
    }

    #endregion

    #endregion

}
