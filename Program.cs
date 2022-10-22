using MyCRM_Online.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyCRM_Online;
using Microsoft.AspNetCore.Identity;
using System.Data.SQLite;
using MyCRM_Online.Models.Entities;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets<Program>();
}
#endif

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddLogging(loggingBuilder => {
    var loggingSection = builder.Configuration.GetSection("Logging");
    loggingBuilder.AddFile(loggingSection);
});

builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.TryAddScoped<DataContext>();
builder.Services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddHttpClient("apiClient", c => {
    c.BaseAddress = new System.Uri(builder.Configuration.GetValue<string>("Api:Uri"));
    c.DefaultRequestHeaders.Add("X-Auth-Token", builder.Configuration.GetValue<string>("Api:Token"));
});

builder.Services
    .AddDbContext<DataContext>(options => {
        var connectionString = builder.Configuration.GetConnectionString("DataDbConnection");
        var connectionBuilder = new SQLiteConnectionStringBuilder(connectionString);
        connectionBuilder.DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, connectionBuilder.DataSource);
        options.UseSqlite(connectionBuilder.ToString());
    });

builder.Services
    .AddDbContext<UsersContext>(options => {
        var connectionString = builder.Configuration.GetConnectionString("UsersDbConnection");
        var connectionBuilder = new SQLiteConnectionStringBuilder(connectionString);
        connectionBuilder.DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, connectionBuilder.DataSource);
        options.UseSqlite(connectionBuilder.ToString());
    });

builder.Services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<UsersContext>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                });

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("ru")
    };

    options.DefaultRequestCulture = new RequestCulture("ru");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Index");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRequestLocalization();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
