using Business.Interfaces;
using Business.Services;
using DB_EFCore.DataAccessLayer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using System.Security.Principal;
using Elasticsearch.Extensions;
using Elasticsearch.Repositories;
using Redis.Extensions;
using Redis.Repositories;
using BLOG_SA.Hubs;
using Serilog;
using DB_EFCore.Repositories.Interfaces;
using DB_EFCore.Repositories;
using Business.Decorators;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>();

//services
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IArticleCommentService, ArticleCommentService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<ISubscriberService, SubscriberService>();

//repositories
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IArticleRepository, DB_EFCore.Repositories.ArticleRepository>();
builder.Services.AddScoped<IArticleCommentRepository, ArticleCommentRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();


//elasticsearch
builder.Services.AddElastic(builder.Configuration);
builder.Services.AddScoped<Elasticsearch.Repositories.ArticleRepository>();
builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

//redis
builder.Services.AddStackExchangeRedis(builder.Configuration);
builder.Services.AddSingleton<RedisRepository>();
builder.Services.AddSingleton<IRedisService, RedisService>();

//decorator design pattern
builder.Services.AddScoped<ISettingSave>(sp =>
{
    var settingService = sp.GetRequiredService<ISettingService>();
    var redisService = sp.GetRequiredService<IRedisService>();

    var redisDecorator = new SettingRedisDecorator(settingService, redisService);

    return redisDecorator;
});

builder.Services.AddScoped<IArticleIUD>(sp =>
{
    var settingService = sp.GetRequiredService<ISettingService>();
    var articleService = sp.GetRequiredService<IArticleService>();
    var elasticService = sp.GetRequiredService<IElasticsearchService>();
    
    var elasticDecorator = new ArticleElasticsearchDecorator(articleService, elasticService, settingService);

    return elasticDecorator;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IPrincipal>(
    provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(settings =>
    {
        settings.LoginPath = "/Login/Index";
    }
);

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chatHub");

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "rss",
    pattern: "rss",
    defaults: new { controller = "Rss", action = "Index" });

app.Run();
