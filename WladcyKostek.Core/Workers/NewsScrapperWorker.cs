using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.ScrapperFactory.Scrappers;
using WladcyKostek.Core.Workers.Options;

namespace WladcyKostek.Core.Workers
{
    public class NewsScrapperWorker : BackgroundService
    {
        private readonly ILogger<NewsScrapperWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly NewsScrapperWorkerOptions _options;
        private IScrappedNewsRepository _newsRepository;
        private Scrapper _scrapper;

        public NewsScrapperWorker(ILogger<NewsScrapperWorker> logger, IServiceScopeFactory scopeFactory, IConfiguration config, IOptions<NewsScrapperWorkerOptions> options)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.IsEnabled)
            {
                _logger.LogInformation("NewsScrapperWorker is disabled via configuration.");
                return;
            }
            _logger.LogInformation("NewsScrapperWorker is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    _newsRepository = scope.ServiceProvider.GetRequiredService<IScrappedNewsRepository>();
                    _scrapper = scope.ServiceProvider.GetRequiredService<Scrapper>();

                    try
                    {
                        await ProcessBackgroundTask(stoppingToken);
                        await Task.Delay(_options.TimerIntervalHours, stoppingToken);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "NewsScrapperWorkerError.");
                    }
                }

            }
            _logger.LogInformation("NewsScrapperWorker is stopping.");
        }

        private async Task ProcessBackgroundTask(CancellationToken stoppingToken)
        {
            if (_options.Scrapper is not null && _options.Url is not null)
            {
                var news = await _scrapper.Run(_options.Scrapper, _options.Url);
                if (news is not null)
                {
                    _logger.LogInformation($"NewsScrapperWorker foun {news.Count} new rpg news!");
                    await _newsRepository.AddNewsAsync(news);
                }
            }
        }
    }
}
