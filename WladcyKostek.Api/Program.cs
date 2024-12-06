using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using WladcyKostek.Core;
using WladcyKostek.Core.Hubs;
using WladcyKostek.Core.ScrapperFactory.Scrappers;
using WladcyKostek.Core.Workers;
using WladcyKostek.Core.Workers.Options;
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Jwt", options =>
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
builder.Services.AddSingleton<Scrapper>();
builder.Services.AddHostedService<NewsGeneratorWorker>();
builder.Services.AddHostedService<NewsSenderWorker>();

builder.Services.Configure<NewsScrapperWorkerOptions>("RpgNewsScrapper", options =>
{
    options.TimerIntervalHours = TimeSpan.FromHours(int.Parse(builder.Configuration.GetSection("Workers").GetSection("NewsScrapperWorkerTimer").Value));
    options.IsEnabled = bool.Parse(builder.Configuration.GetSection("Workers").GetSection("IsNewsScrapperWorkerEnabled").Value);
    options.Url = "http://rpgnews.com/";
    options.Scrapper = new RpgNewsScrapper();
});

builder.Services.Configure<NewsScrapperWorkerOptions>("PenAndPaperScrapper", options =>
{
    options.TimerIntervalHours = TimeSpan.FromHours(int.Parse(builder.Configuration.GetSection("Workers").GetSection("NewsScrapperWorkerTimer").Value));
    options.IsEnabled = bool.Parse(builder.Configuration.GetSection("Workers").GetSection("IsNewsScrapperWorkerEnabled").Value);
    options.Url = "https://penandpaper.news/";
    options.Scrapper = new PenAndPaperScrapper();
});

builder.Services.AddHostedService<NewsScrapperWorker>(sp =>
{
    var options = sp.GetRequiredService<IOptionsMonitor<NewsScrapperWorkerOptions>>().Get("RpgNewsScrapper");
    return ActivatorUtilities.CreateInstance<NewsScrapperWorker>(sp, Options.Create(options));
});

builder.Services.AddHostedService<NewsScrapperWorker>(sp =>
{
    var options = sp.GetRequiredService<IOptionsMonitor<NewsScrapperWorkerOptions>>().Get("PenAndPaperScrapper");
    return ActivatorUtilities.CreateInstance<NewsScrapperWorker>(sp, Options.Create(options));
});

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
