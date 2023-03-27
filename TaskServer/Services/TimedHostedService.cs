using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TaskServer.Entities;
using TaskServer.Interfaces;

namespace TaskServer.Services;

public class TimedHostedService : BackgroundService
{
    private readonly ILogger<TimedHostedService> _logger;
    private int _executionCount;
    private readonly IDistributedCache _cache;
    private IServiceScopeFactory _scopeFactory;
    private IConfiguration _configuration;

    public TimedHostedService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<TimedHostedService> logger,
        IDistributedCache cache
    )
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
        _cache = cache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        DoWork();

        using PeriodicTimer timer = new(TimeSpan.FromMinutes(10));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                DoWork();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
        }
    }

    private async void DoWork()
    {
        int count = Interlocked.Increment(ref _executionCount);

        _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);

        using (var scope = _scopeFactory.CreateScope())
        {
            var users = scope.ServiceProvider.GetService<IUsers>();

            if (users == null || users.AllUsers().Count<User>() == 0)
            {
                return;
            }

            var prefix = _configuration.GetValue<string>("CacheKeyPrefix") ?? "";

            string cachedString;

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11));

            foreach (var user in users.AllUsers())
            {
                cachedString = JsonSerializer.Serialize(user);

                await _cache.SetStringAsync(
                    prefix + user.Id,
                    cachedString,
                    options
                );
            }
        }
    }
}
