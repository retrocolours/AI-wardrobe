using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register ApplicationDbContext and AiwardrobeContext with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<AiwardrobeContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity Configuration (Supports Roles)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add Authorization & MVC Controllers (NO API Controllers)
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register Repositories
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<UserRoleRepo>();
builder.Services.AddScoped<RoleRepo>();
builder.Services.AddScoped<ShopAllRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<TransactionRepo>();
builder.Services.AddScoped<OrderRepo>();


// Register Services
builder.Services.AddTransient<IEmailService, EmailService>();

// Configure Cookies
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieRepository>();

// Configure Cookie Policy for Security
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always; // Ensures cookies are sent over HTTPS
});

// Add Session Services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Enable session in middleware
app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Default HSTS (30 days)
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(); // Ensures cookies follow the configured policy

app.UseAuthentication();
app.UseAuthorization();

// Register MVC and Razor Pages (NO API Controllers)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
