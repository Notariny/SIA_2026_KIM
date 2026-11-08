using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIA_2026_KIM.Shared.Services;
using SIA_2026_KIM.Web;
using SIA_2026_KIM.Web.Components;
using SIA_2026_KIM.Web.Data;
using SIA_2026_KIM.Web.Data.Entities;
using SIA_2026_KIM.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add device-specific services used by the SIA_2026_KIM.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();


builder.Services.AddDbContextFactory<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(connectionString);
});

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(WebConstants.WebAuthScheme)
    .AddCookie(WebConstants.WebAuthScheme,
    options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.Cookie.Name = WebConstants.WebAuthScheme;
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, WebAuthStateProvider>();

var app = builder.Build();

//Auto db migration
AutoMigrationDb(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(SIA_2026_KIM.Shared._Imports).Assembly);

app.Run();

static void AutoMigrationDb(IServiceProvider sp)
{
    using var scope = sp.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>().CreateDbContext();

    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}
