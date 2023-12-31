using DapApp.Admin.API;
using DapApp.Admin.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Http;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.secret.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;
var isDevelopment = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development";

// Add services to the container.
if (configuration != null
    && !string.IsNullOrWhiteSpace(configuration.GetValue<string>("S3FileStorage:url"))
    && !string.IsNullOrWhiteSpace(configuration.GetValue<string>("S3FileStorage:accessKey"))
    && !string.IsNullOrWhiteSpace(configuration.GetValue<string>("S3FileStorage:secretKey")))
{
    builder.Services.AddScoped<IFileService>(sp =>
    {
        return new S3FileService(configuration.GetValue<string>("S3FileStorage:url"), configuration.GetValue<string>("S3FileStorage:accessKey"), configuration.GetValue<string>("S3FileStorage:secretKey"));
    });
}

builder.Services.AddTransient<AuthHeaderInterceptor>();
builder.Services
    .AddHttpClient("AuthHttpClient")
    .AddHttpMessageHandler<AuthHeaderInterceptor>();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = configuration.GetValue<string>("SSO:Authority"); ;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !isDevelopment,
            ValidateAudience = false,
            ValidIssuer = configuration.GetValue<string>("SSO:ValidIssuer"),
            NameClaimType = ClaimTypes.NameIdentifier
        };

        options.Events = new JwtBearerEvents();

        options.Events.OnAuthenticationFailed = cxt =>
        {
            Debug.WriteLine(cxt.Exception.Message);
            return Task.CompletedTask;
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("http://localhost/sso/connect/authorize"),
                TokenUrl = new Uri("http://localhost/sso/connect/token"),
            }
        }
    });

    opt.OperationFilter<AuthResponsesOperationFilter>();
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
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.OAuthClientId("swagger");
        opt.OAuthClientSecret("secret");
        opt.OAuthScopes("openid");
        opt.OAuthUsePkce();
    });
    IdentityModelEventSource.ShowPII = true;
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
