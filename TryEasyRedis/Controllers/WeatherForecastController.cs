using EasyRedis;
using Microsoft.AspNetCore.Mvc;

namespace TryEasyRedis.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IRedisSrvc _redis;
    public WeatherForecastController(IRedisSrvc redis)
    {
        _redis = redis;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        await _redis.StringSetAsync("123","12333");
        var rr = await _redis.StringGetAsync<string>("123");
        return default;
    }
}
