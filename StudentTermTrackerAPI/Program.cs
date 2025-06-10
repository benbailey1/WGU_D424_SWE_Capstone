using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StudentTermTrackerAPI.Data;
using StudentTermTrackerAPI.Auth.Services;
using StudentTermTrackerAPI.Auth.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your JWT Access Token in the text input below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// Register database connection service
builder.Services.AddScoped<IDatabaseConnectionService, DatabaseConnectionService>();

// Register repositories
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();

// Register services
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

builder.Services.AddScoped<JWTService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // For Docker/Azure, the SSL is handled externally
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // Add clock skew tolerance for mobile apps
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MauiPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // Local development - more permissive
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            // Production
            policy.WithOrigins(
                "https://localhost",
                "https://studenttermtrackerapi-hdgkgkcwa7ffamat.westcentralus-01.azurewebsites.net", // Fixed: Added https://
                "ms-appx-web://",
                "file://")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

// Configure forwarded headers for Azure proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Azure App Service terminates SSL and adds these headers
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// TEMPORARY FOR DEBUGGING - REMOVE AFTER FIXING ISSUE!
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   
}
else
{
    // app.UseHttpsRedirection(); NOT NEEDED FOR AZUE
}

app.UseForwardedHeaders();

app.UseCors("MauiPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint for testing
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    environment = app.Environment.EnvironmentName,
    timestamp = DateTime.UtcNow
})).AllowAnonymous();

// Test if your controllers are registered
app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
{
    var endpoints = endpointSources.SelectMany(source => source.Endpoints);
    return Results.Ok(endpoints.Select(endpoint =>
    {
        var routeEndpoint = endpoint as RouteEndpoint;
        return new
        {
            displayName = endpoint.DisplayName,
            pattern = routeEndpoint?.RoutePattern?.RawText,
            httpMethods = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods
        };
    }));
}).AllowAnonymous();

// Test JWT configuration
app.MapGet("/debug/config", () => Results.Ok(new
{
    environment = app.Environment.EnvironmentName,
    hasJwtIssuer = !string.IsNullOrEmpty(builder.Configuration["JwtConfig:Issuer"]),
    hasJwtAudience = !string.IsNullOrEmpty(builder.Configuration["JwtConfig:Audience"]),
    hasJwtKey = !string.IsNullOrEmpty(builder.Configuration["JwtConfig:Key"]),
    jwtKeyLength = builder.Configuration["JwtConfig:Key"]?.Length ?? 0
})).AllowAnonymous();

// Test CORS configuration
app.MapGet("/debug/cors", () => Results.Ok("CORS test successful"))
    .AllowAnonymous()
    .RequireCors("MauiPolicy");

app.Run();

public partial class Program { }