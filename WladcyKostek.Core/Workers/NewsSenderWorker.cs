using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WladcyKostek.Core.Hubs;
using WladcyKostek.Core.Interfaces;

namespace WladcyKostek.Core.Workers
{
    public class NewsSenderWorker : BackgroundService
    {
        private readonly ILogger<NewsSenderWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval;
        private readonly bool _isEnabled;
        private IHubContext<NewsHub> _hubContext;
        private INewsRepository _newsRepository;

        public NewsSenderWorker(ILogger<NewsSenderWorker> logger, IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _interval = TimeSpan.FromMinutes(int.Parse(config.GetSection("Workers").GetSection("NewsSenderWorkersTimer").Value));
            _isEnabled = bool.Parse(config.GetSection("Workers").GetSection("IsNewsSenderWorkerEnabled").Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_isEnabled)
            {
                _logger.LogInformation("NewsSenderWorker is disabled via configuration.");
                return;
            }
            _logger.LogInformation("NewsSenderWorker is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    _newsRepository = scope.ServiceProvider.GetRequiredService<INewsRepository>();
                    _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NewsHub>>();

                    try
                    {
                        await ProcessBackgroundTask(stoppingToken);
                        await Task.Delay(_interval, stoppingToken);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "NewsSenderWorker Error.");
                    }
                }

            }
            _logger.LogInformation("NewsSenderWorker is stopping.");
        }

        private async Task ProcessBackgroundTask(CancellationToken stoppingToken)
        {
            var newsIds = await _newsRepository.GetNewsIdsAsync();
            foreach (var newsId in newsIds)
            {
                var news = await _newsRepository.GetSingleNewsAsync(newsId);
                await _newsRepository.SetNewsAsSentAsync(newsId);
                await _hubContext.Clients.All.SendAsync("NewPost", news);
                _logger.LogInformation($"NewsSenderWorker sent: [{newsId}].");
                await Task.Delay(TimeSpan.FromSeconds(new Random().Next(30, 120)));
            }
        }
    }
}
