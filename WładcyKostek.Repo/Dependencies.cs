using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;
using WładcyKostek.Core.Interfaces;
using WładcyKostek.Repo.Context;
using WładcyKostek.Repo.Repository;

namespace WładcyKostek.Repo
{
    public static class Dependencies
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddSingleton(new OpenAIAPI(config["OpenAiKey"]));
            return services;
        }
    }
}
