using Azure.Identity;
using Serilog;
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
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200") // adres frontendowej aplikacji
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // AllowCredentials jest wymagane dla SignalR
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(CoreAssemblyMarker).Assembly));
builder.Services.AddOpenApi();
builder.Services.AddHostedService<NewsGeneratorWorker>();
builder.Services.AddHostedService<NewsSenderWorker>();
builder.Services.AddRepositoryServices(builder.Configuration);
builder.Services.AddSignalR();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.MapHub<NewsHub>("/newsHub");
app.MapControllers();
app.Run();
