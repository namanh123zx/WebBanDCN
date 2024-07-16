using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebBH_Shopping.Data;

using WebBH_Shopping.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebBH_ShoppingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebBH_ShoppingContext") ?? throw new InvalidOperationException("Connection string 'WebBH_ShoppingContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();




builder.Services.AddSession(options =>
{
	options.Cookie.Name = ".AdventureWorks.Session";
	options.IdleTimeout = TimeSpan.FromSeconds(10);
	options.Cookie.IsEssential = true;
});


builder.Services.AddIdentity<AppUserModel,IdentityRole>()
    .AddEntityFrameworkStores<WebBH_ShoppingContext>().AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;

   
    options.User.RequireUniqueEmail = true;
});



var app = builder.Build();
app.UseSession();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");
app.MapControllerRoute(
	name: "Areas",
	pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "category",
    pattern: "/category/{Slug?}",
	defaults: new {controller="Category", action="Index"});
app.MapControllerRoute(
    name: "brand",
    pattern: "/brand/{Slug?}",
    defaults: new { controller = "Brand", action = "Index" });

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");


var _context = app.Services.CreateScope().ServiceProvider.GetRequiredService<WebBH_ShoppingContext>();
SeedData.SeedingData(_context);
app.Run();
