using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EasyRedis;
public abstract class BaseRedis : IBaseRedis,IDisposable
{
    private readonly IConfiguration _configuration;
    private RedisDb _defultDb = RedisDb.Db0;
    private readonly DynamicLifeTime _dynamicLifeTime;
    private RedisDb? _currentDb;
    private bool disposed = false;

    internal TimeSpan? _lifeTime;

    /// <summary>
    /// Can Use redisConnection As You Wish
    /// Also Can Change Its Setting Using <see cref="SetRedisConfiguration"/>
    /// </summary>
    public ConnectionMultiplexer redisConnection { get; set; }

    /// <summary>
    /// Can Access Redis Defults
    /// </summary>
    public IDatabase redis { get; set; }

    protected BaseRedis(IConfiguration configuration)
    {
        _configuration = configuration;
        redisConnection = ConnectionMultiplexer.Connect(GetRedisConfiguration());
        GetRedisDatabase(RedisDb.DbDefult);
        _dynamicLifeTime = new(configuration);
    }

    private ConfigurationOptions GetRedisConfiguration()
    {
        return new ConfigurationOptions
        {
            EndPoints = { _configuration["EasyRedis:EndPoints"] },
            Password = _configuration["EasyRedis:Password"],
            ConnectTimeout = int.Parse(_configuration["EasyRedis:ConnectTimeout"]),
            AbortOnConnectFail = bool.Parse(_configuration["EasyRedis:AbortOnConnectFail"]),            
            AllowAdmin = bool.Parse(_configuration["EasyRedis:AllowAdmin"])
        };
    }

    #region Settings

    /// <summary>
    /// Change ConnectionMultiplexer Connection Config If Dont Want To Use AppSettings Defult Settings
    /// Also Can Use <see cref="redisConnection"/> As You Wish
    /// </summary>
    /// <param name="config"></param>
    public void SetRedisConfiguration(ConfigurationOptions config)
    {
        redisConnection = ConnectionMultiplexer.Connect(config);
    }

    /// <summary>
    /// To Set A Defult Db And Other Methods Will Use This Db By Defult Except Choose Db While Using Them
    /// </summary>
    /// <param name="dbNumber">database number Enum RedisDbEnum 0-15</param>
    public BaseRedis SetDefultDb(RedisDb dbNumber)
    {
        _defultDb = dbNumber;
        GetRedisDatabase(dbNumber);
        return default;
    }

    /// <summary>
    /// Changes Defult LifeTime In AppSettings
    /// Can Reset The Defult Lifetime Calling <see cref="SetDbLifeTime"/>
    /// </summary>
    /// <param name="dbNumber">database number Enum RedisDbEnum 0-15</param>
    /// <param name="lifeTime">Db LifeTime</param>
    /// <returns></returns>
    public BaseRedis SetDbLifeTime(RedisDb dbNumber, TimeSpan lifeTime)
    {
        _dynamicLifeTime.dbs[dbNumber] = lifeTime;
        return default;
    }

    /// <summary>
    /// Changes Defult LifeTime In AppSettings
    /// Can Reset The Defult Lifetime Calling <see cref="SetDbLifeTime"/>
    /// </summary>
    /// <param name="dbNumber">database number Enum RedisDbEnum 0-15</param>
    /// <param name="lifeTime">Db LifeTime</param>
    /// <returns></returns>
    public BaseRedis SetDbLifeTime(Dictionary<RedisDb, TimeSpan> dbs)
    {
        _dynamicLifeTime.dbs = dbs;
        return default;
    }

    /// <summary>
    /// After Calling <see cref="SetDbLifeTime"/> Can Call This Method To Use Th Defults Again
    /// </summary>
    /// <returns></returns>
    public BaseRedis SetDefultDbLifeTime()
    {
        _dynamicLifeTime.Init();
        return default;
    }

    #endregion

    #region internals

    internal void GetRedisDatabase(RedisDb dbNumber)
    {
        if (_currentDb != dbNumber)
        {
            if (dbNumber == RedisDb.DbDefult)
            {
                // defultChosenDb Is Db 0 By Defult Except User Changes It
                redis = redisConnection.GetDatabase((int)_defultDb);
            }
            else if (dbNumber != RedisDb.DbDefult)
            {
                //If User Chooses Any Db Then Use It !
                redis = redisConnection.GetDatabase((int)dbNumber);
            }

            _currentDb = dbNumber;
        }
    }

    internal void GetRedisLifetime(RedisDb dbNumber = RedisDb.DbDefult)
    {
        if (dbNumber == RedisDb.DbDefult)
        {
            _lifeTime = _dynamicLifeTime.Get(_defultDb);
        }
        else if (dbNumber != RedisDb.DbDefult)
        {
            _lifeTime = _dynamicLifeTime.Get(dbNumber);
        }
    }

    #endregion

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                redis = null;
                _dynamicLifeTime.dbs.Clear();
                redisConnection?.Dispose();
            }
            disposed = true;
        }
    }

    ~BaseRedis()
    {
        Dispose(false);
    }

}
