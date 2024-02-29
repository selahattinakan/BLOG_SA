using Business.Interfaces;
using Business.Services;
using DB_EFCore.DataAccessLayer;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
