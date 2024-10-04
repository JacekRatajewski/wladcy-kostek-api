using Microsoft.EntityFrameworkCore;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;
using WladcyKostek.Repo.Context;
using WladcyKostek.Repo.Entities;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Text.Json;
using OpenAI_API.Images;

namespace WladcyKostek.Repo.Repository
{
    internal class NewsRepository : INewsRepository
    {
        private DatabaseContext _database;
        private OpenAIAPI _http;
        private string? _userCount;
        private string? _city;

        public NewsRepository(DatabaseContext database, IConfiguration config, OpenAIAPI openAIAPI)
        {
            _database = database;
            _http = openAIAPI;
            _userCount = config.GetSection("News").GetSection("UserCount").Value;
            _city = config.GetSection("News").GetSection("City").Value;
        }

        public async Task<List<int>> GetNewsIdsAsync()
        {
            return await _database.News
                            .OrderBy(n => n.DateTime)
                            .Where(n => n.Sent == false)
                            .Take(10)
                            .Select(n => n.Id)
                            .ToListAsync();
        }

        public async Task<List<NewsDTO>> GenerateNewsAsync()
        {
            var response = await _http.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.GPT4_Turbo,
                Temperature = 0.9,
                Messages = new ChatMessage[] {
                    new ChatMessage(ChatMessageRole.User, $"Wygeneruj {_userCount} nazw użytkowników portalu społecznościowego. W formacie: nazwa1;nazwa2;")
                }
            });
            var choices = response.Choices.FirstOrDefault();
            if (choices is null)
            {
                throw new Exception("No choices!");
            }
            var userNames = choices.Message.TextContent.Split(';').Where(i => !string.IsNullOrWhiteSpace(i));
            var posts = new List<NewsDTO>();
            foreach (var user in userNames)
            {
                var post = await _http.Chat.CreateChatCompletionAsync(new ChatRequest()
                {
                    Model = Model.GPT4_Turbo,
                    Temperature = 0.9,
                    Messages = new ChatMessage[] {
                        new ChatMessage(ChatMessageRole.User, $"Wszystko po polsku. Wygeneruj króciutki post użytkownika {user} podczas apokalipsy zombie na portalu społecznościowym Watch The World Burn, wybierz ulicę na której dzieje się akcja w mieście {_city} w Polsce. Jest godzina: {DateTime.Now.ToString("HH:mm")}. W formacie json z parametrami: Title, Message")
                    }
                });
                var image = await _http.ImageGenerations.CreateImageAsync(new ImageGenerationRequest($"A detailed and textured science-fiction avatar for the user '{user}', featuring a zombie-like characte. Grey, Red, Black color palette. The character is standing in an exaggerated, powerful pose. The textures should give a sense of depth and complexity. The style should evoke a sense of fantasy and science fiction, with strong, dynamic lines and rich shading.", 1, ImageSize._256));
                if (post.Choices.Count > 0)
                {
                    var json = post.Choices[0].Message.TextContent.Split("json")[1].Split("```")[0];
                    var _post = JsonSerializer.Deserialize<NewsDTO>(json);
                    _post.DateTime = DateTime.Now;
                    _post.UserId = user;
                    _post.ImageBase64 = image.Data[0].Url;
                    posts.Add(_post);
                }
            }
            return posts;
        }

        public async Task AddNewsAsync(List<NewsDTO> news)
        {
            var _news = news.Select(i => new News
            {
                DateTime = i.DateTime,
                Message = i.Message,
                Title = i.Title,
                UserId = i.UserId,
                ImageBase64 = i.ImageBase64,
                Sent = false
            });
            await _database.News.AddRangeAsync(_news);
            await _database.SaveChangesAsync();
        }

        public async Task<NewsDTO> GetSingleNewsAsync(int id)
        {
            var _news = await _database.News.FirstOrDefaultAsync(x => x.Id == id);
            if (_news is null)
            {
                throw new Exception("");
            }
            return new NewsDTO
            {
                Id = _news.Id,
                DateTime = _news.DateTime,
                Message = _news.Message,
                Title = _news.Title,
                UserId = _news.UserId,
                ImageBase64 = _news.ImageBase64,
                VideoUrl = _news.VideoUrl
            };
        }

        public async Task SetNewsAsSentAsync(int id)
        {
            var _news = await _database.News.FirstOrDefaultAsync(x => x.Id == id);
            if (_news is null)
            {
                throw new Exception("");
            }
            _news.Sent = true;
            await _database.SaveChangesAsync();
        }

        public async Task<int> AddSingleNewsAsync(NewsDTO news)
        {
            var _news = new News
            {
                DateTime = news.DateTime,
                Message = news.Message,
                Title = news.Title,
                UserId = news.UserId,
                ImageBase64 = news.ImageBase64,
                VideoUrl = news.VideoUrl,
                Sent = false
            };
            await _database.News.AddAsync(_news);
            await _database.SaveChangesAsync();
            return _news.Id;
        }
    }
}
