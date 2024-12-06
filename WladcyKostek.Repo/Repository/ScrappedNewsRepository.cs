using Microsoft.EntityFrameworkCore;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Repo.Context;
using WladcyKostek.Repo.Entities;
using WladcyKostek.Core.ScrapperFactory.Models;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Repo.Repository
{
    internal class ScrappedNewsRepository : IScrappedNewsRepository
    {
        private DatabaseContext _database;

        public ScrappedNewsRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task AddNewsAsync(List<IScrappedNews> news)
        {
            var existingTitles = await _database.ScrappedNews
                .Select(n => n.Title)
                .ToListAsync();

            var newNews = news
                .Where(n => !existingTitles.Contains(n.Title))
                .Select(i => new ScrappedNews
                {
                    Description = i.Description,
                    ImageUrl = i.ImageUrl,
                    Title = i.Title,
                    Url = i.Url,
                    ScrappedTime = DateTime.Now,
                })
                .ToList();

            if (newNews.Any())
            {
                await _database.ScrappedNews.AddRangeAsync(newNews);
                await _database.SaveChangesAsync();
            }
        }

        public async Task<List<ScrappedNewsDTO>> GetAllAsync()
        {
            return await _database.ScrappedNews
                .OrderByDescending(news => news.ScrappedTime)
                .Select(news => new ScrappedNewsDTO
                {
                    Id = news.Id,
                    Title = news.Title,
                    Description = news.Description,
                    ImageUrl = news.ImageUrl,
                    Url = news.Url
                })
                .ToListAsync();
        }

        public async Task<List<ScrappedNewsDTO>> GetAllSkip6Async()
        {
            return await _database.ScrappedNews
                .OrderByDescending(news => news.ScrappedTime)
                .Select(news => new ScrappedNewsDTO
                {
                    Id = news.Id,
                    Title = news.Title,
                    Description = news.Description,
                    ImageUrl = news.ImageUrl,
                    Url = news.Url
                })
                .ToListAsync();
        }

        public async Task<List<ScrappedNewsDTO>> GetTop6Async()
        {
            return await _database.ScrappedNews.OrderByDescending(news => news.ScrappedTime)
                .Take(6)
                .Select(news => new ScrappedNewsDTO
                {
                    Id = news.Id,
                    Title = news.Title,
                    Description = news.Description,
                    ImageUrl = news.ImageUrl,
                    Url = news.Url
                })
                .ToListAsync();
        }
    }
}
