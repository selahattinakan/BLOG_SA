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

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IArticleService, ArticleService>();
builder.Services.AddTransient<IArticleCommentService, ArticleCommentService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<ILogService, LogService>();
builder.Services.AddTransient<IService, Service>();
builder.Services.AddTransient<ISettingService, SettingService>();
builder.Services.AddTransient<ISubscriberService, SubscriberService>();

builder.Services.AddElastic(builder.Configuration);
builder.Services.AddScoped<ArticleRepository>();
builder.Services.AddScoped<IElasticsearch, ElasticsearchService>();

builder.Services.AddStackExchangeRedis(builder.Configuration);
builder.Services.AddSingleton<RedisRepository>();
builder.Services.AddSingleton<IRedisService, RedisService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IPrincipal>(
    provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(settings =>
    {
        settings.LoginPath = "/Login/Index";
    }
);

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
