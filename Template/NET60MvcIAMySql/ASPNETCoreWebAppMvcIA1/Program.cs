/*
 
DbContext Lifetime, Configuration, and Initialization
ref: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#dbcontext-in-dependency-injection-for-aspnet-core 
不同的 database provider 會提供(Use字頭的擴充函數). 例如:
// These Use* methods are extension methods implemented by the database provider. This means that the database provider NuGet package must be installed before the extension method can be used.
--------------------------- ------------------------------------------- ---------------------------------------
Database system,            Example configuration,                      NuGet package
SQL Server or Azure SQL,    .UseSqlServer(connectionString),            Microsoft.EntityFrameworkCore.SqlServer
Azure Cosmos DB,            .UseCosmos(connectionString, databaseName)  Microsoft.EntityFrameworkCore.Cosmos
SQLite,                     .UseSqlite(connectionString),               Microsoft.EntityFrameworkCore.Sqlite
EF Core in-memory database, .UseInMemoryDatabase(databaseName),         Microsoft.EntityFrameworkCore.InMemory
PostgreSQL*,                .UseNpgsql(connectionString),               Npgsql.EntityFrameworkCore.PostgreSQL
MySQL/MariaDB*,             .UseMySql(connectionString),                Pomelo.EntityFrameworkCore.MySql
Oracle*,                    .UseOracle(connectionString),               Oracle.EntityFrameworkCore

所以要先安裝 database provider 的元件後, 才會有對應的(Use字頭的擴充函數)可使用.

mysql:
Pomelo.EntityFrameworkCore.MySql is the Entity Framework Core (EF Core) provider for MySQL, MariaDB, Amazon Aurora, Azure Database for MySQL and other MySQL-compatible databases.
https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/
*/

using ASPNETCoreWebAppMvcIA1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Replace with your server version and type.
// Use 'MariaDbServerVersion' for MariaDB.
// Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseMySql(connectionString, serverVersion)
    // The following three options help with debugging, but should
    // be changed or removed for production.
    //.LogTo(Console.WriteLine, LogLevel.Information)
    //.EnableSensitiveDataLogging()
    //.EnableDetailedErrors()
    );

// Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore (6.0.16)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// .AddEntityFrameworkStores<ApplicationDbContext>(); in Microsoft.Identity.EntityFrameworkCore (6.0.16)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// ref: https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /// Processes requests to execute migrations operations. The middleware will listen for requests made to <see cref="MigrationsEndPointOptions.DefaultPath"/>.
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Individule Account 啟用驗證.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
