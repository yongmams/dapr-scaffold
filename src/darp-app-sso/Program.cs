using DaprApp.SSO;
using IdentityServer4.Extensions;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddRazorPages();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IRedirectUriValidator, CustomRedirectUriValidator>();
var id4buiild = builder.Services
    .AddIdentityServer(options =>
    {
    })
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddTestUsers(TestUsers.Users)
    .AddDeveloperSigningCredential();

builder.Services.AddDataProtection()
    .SetApplicationName("DaprApp.SSO")
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "PersistKeys")));
// .PersistKeysToDbContext<ApplicationDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = ".DaprApp.SSO.Cookie";
    });

bool IsSubdomainOf(Uri subdomain, Uri domain)
{
    return subdomain.IsAbsoluteUri
        && domain.IsAbsoluteUri
        && subdomain.Scheme == domain.Scheme
        && subdomain.Port == domain.Port
        && subdomain.Host.EndsWith($".{domain.Host}", StringComparison.Ordinal);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AnyOrigin",
        policy =>
        {
            var corsUrls = builder.Configuration.GetSection("CorsUrls").Get<string[]>();

            if (corsUrls != null && corsUrls.Count() > 0)
            {
                policy.WithOrigins(corsUrls.ToArray());
            }

            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin =>
                {
                    if (corsUrls != null && corsUrls.Contains(origin))
                    {
                        return true;
                    }

                    if (corsUrls != null && Uri.TryCreate(origin, UriKind.Absolute, out var originUri))
                    {
                        if (originUri.Host == "127.0.0.1" || originUri.Host == "localhost")
                        {
                            return true;
                        }

                        return corsUrls
                            .Where(o => o.Contains($"://*"))
                            .Select(r => new Uri(r.Replace("*", string.Empty), UriKind.Absolute))
                            .Any(domain => IsSubdomainOf(originUri, domain));
                    }

                    return false;
                });
            //.SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});

var app = builder.Build();

app.UseForwardedHeaders();
var pathBase = Environment.GetEnvironmentVariable("ASPNETCORE_PATHBASE");
if (!string.IsNullOrEmpty(pathBase))
{
    app.UsePathBase(new PathString(pathBase));
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AnyOrigin");

app.Use(async (ctx, next) =>
{
    string host = ctx.Request.Headers["X-Forwarded-Host"].FirstOrDefault() ?? ctx.Request.Host.Value;
    string scheme = ctx.Request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? ctx.Request.Scheme;

    ctx.SetIdentityServerOrigin($"{scheme}://{host}");
    await next();
});

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
