using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TaskServer.Interfaces;
using TaskServer.Entities;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace TaskServer.Controllers;

[Route("Public")]
public class UserInfoController : Controller
{
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public UserInfoController(
        IDistributedCache cache,
        IConfiguration configuration
    ) {
        _cache = cache;
        _configuration = configuration;
    }

    [HttpGet("UserInfo")]
    public ActionResult Index([FromQuery] int id)
    {
        var prefix = _configuration.GetValue<string>("CacheKeyPrefix") ?? "";
        var user = _cache.GetString(prefix + id);

        if (user == null)
        {
            return BadRequest(new ArgumentException("User not found"));
        }

        return View("User", JsonSerializer.Deserialize<User>(user));
    }
}

