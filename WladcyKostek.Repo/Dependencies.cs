﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Repo.Context;
using WladcyKostek.Repo.Repository;

namespace WladcyKostek.Repo
{
    public static class Dependencies
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IBonusesRepository, BonusesRepository>();
            services.AddScoped<IScrappedNewsRepository, ScrappedNewsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton(new OpenAIAPI(config["OpenAiKey"]));
            return services;
        }
    }
}
