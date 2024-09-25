using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using W³adcyKostek.Core;
using W³adcyKostek.Core.Hubs;
using W³adcyKostek.Core.Interfaces;
using W³adcyKostek.Core.Workers;
using W³adcyKostek.Repo;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.MapHub<NewsHub>("/newsHub");
app.MapControllers();
app.Run();
