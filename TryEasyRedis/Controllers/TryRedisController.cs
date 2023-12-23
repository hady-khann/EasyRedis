using EasyRedis;
using Microsoft.AspNetCore.Mvc;

namespace TryEasyRedis.Controllers;

[ApiController]
[Route("[controller]")]
public class TryRedisController : ControllerBase
{
    private readonly IRedisSrvc _redis;
    public TryRedisController(IRedisSrvc redis)
    {
        _redis = redis;
    }

    [HttpGet(Name = "TryRedis")]
    public async Task<IActionResult> Get()
    {
        await _redis.StringSetAsync("123","12333");
        var redis = _redis.UseRedis(RedisDb.Db9);

        await redis.StringSetAsync("22","sadf");

        return default;
    }
}
