﻿using Microsoft.Extensions.Configuration;

namespace EasyRedis;
internal class DynamicLifeTime
{
    private readonly IConfiguration _configuration;
    private readonly IFormatProvider formatProvider = null;
    internal Dictionary<RedisDb, TimeSpan> dbs = new();

    internal DynamicLifeTime(IConfiguration configuration)
    {
        _configuration = configuration;
        Init();
    }

    internal void Init()
    {
        foreach (RedisDb dbEnum in Enum.GetValues(typeof(RedisDb)))
        {
            if ((int)dbEnum >= 0)
            {
                string configValue = _configuration.GetSection($"EasyRedis:LifeTime:{Enum.GetName(typeof(RedisDb), dbEnum)}").Value;

                if (!string.IsNullOrEmpty(configValue) && TimeSpan.TryParseExact(configValue, "dd\\.hh\\:mm\\:ss", formatProvider, out TimeSpan timeSpan))
                {
                    dbs.Add(dbEnum, timeSpan);
                }
            }
        }
    }

    internal TimeSpan Get(RedisDb dbNumber)
    {
        return dbs[dbNumber];
    }
}
