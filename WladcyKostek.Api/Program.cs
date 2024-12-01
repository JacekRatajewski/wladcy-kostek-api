using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using WladcyKostek.Core;
using WladcyKostek.Core.Hubs;
using WladcyKostek.Core.Workers;
using WladcyKostek.Repo;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
if (builder.Environment.IsProduction())
{
    Log.Logger.Information("Running in Production!");
    var keyVaultEndpoint = new Uri(builder.Configuration["KeyVault:Uri"] ?? throw new InvalidOperationException("Key Vault Uri is missing"));

    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
}
else
{
    Log.Logger.Information("Running in Development!");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
        };
    });
builder.Services.AddControllers();
if (builder.Environment.IsProduction())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("https://wtwb.xyz")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("https://wladcykostek.pl")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
    });
}
else
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
    });
}




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(CoreAssemblyMarker).Assembly));
builder.Services.AddOpenApi();
builder.Services.AddHostedService<NewsGeneratorWorker>();
builder.Services.AddHostedService<NewsSenderWorker>();
builder.Services.AddRepositoryServices(builder.Configuration);
builder.Services.AddSignalR();
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.MapOpenApi();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.UseAuthorization();
app.UseCors();
app.MapHub<NewsHub>("/newsHub");
app.MapControllers();
app.Run();
