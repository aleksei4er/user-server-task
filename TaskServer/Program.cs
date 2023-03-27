using System.Configuration;
using TaskServer;
using TaskServer.Interfaces;
using TaskServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using TaskServer.Services;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);
Microsoft.Extensions.Configuration.ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.Configure<RazorViewEngineOptions>(options =>
 {
     options.ViewLocationExpanders.Add(new CustomViewLocator());
 });
builder.Services.AddMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IUsers, UserRepository>();

builder.Services.AddAuthentication("BasicAuthentication")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddDbContext<Context>(options =>
    options.UseMySQL(builder.Configuration["ConnectionStrings:MysqlContext"]));

builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["RedisCacheUrl"]; });
builder.Services.AddHostedService<TimedHostedService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var someContext = scope.ServiceProvider.GetRequiredService<Context>();
    someContext.Seed();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();
app.Run();
