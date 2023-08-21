using DapApp.Admin.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.secret.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;

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

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "http://host.docker.internal/sso/";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidIssuer = "http://localhost/sso",
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
builder.Services.AddSwaggerGen();

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
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
