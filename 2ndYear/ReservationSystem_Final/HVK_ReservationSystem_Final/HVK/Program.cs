using HVK.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Formatting Service
builder.Services.AddTransient<FormattingService>();

// Add Session middleware
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(25);
    options.Cookie.Name = "HVKSession";
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HVKW24_Team7Context>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("MyHVKConnection"))
);

// May need to use sessions, but idk 
builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession(); // INSERTED, SHOULD BE LAST

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}");

app.Run();
