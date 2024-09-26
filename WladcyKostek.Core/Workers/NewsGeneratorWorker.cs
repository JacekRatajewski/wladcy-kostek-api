using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WladcyKostek.Core.Interfaces;

namespace WladcyKostek.Core.Workers
{
    public class NewsGeneratorWorker : BackgroundService
    {
        private readonly ILogger<NewsGeneratorWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval;
        private readonly bool _isEnabled;
        private INewsRepository _newsRepository;

        public NewsGeneratorWorker(ILogger<NewsGeneratorWorker> logger, IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _interval = TimeSpan.FromMinutes(int.Parse(config.GetSection("Workers").GetSection("NewsGeneratorWorkersTimer").Value));
            _isEnabled = bool.Parse(config.GetSection("Workers").GetSection("IsNewsGeneratorWorkerEnabled").Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_isEnabled)
            {
                _logger.LogInformation("NewsGeneratorWorker is disabled via configuration.");
                return;
            }
            _logger.LogInformation("NewsGeneratorWorker is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    _newsRepository = scope.ServiceProvider.GetRequiredService<INewsRepository>();

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
                        _logger.LogError(ex, "NewsGeneratorWorkerError.");
                    }
                }

            }
            _logger.LogInformation("NewsGeneratorWorker is stopping.");
        }

        private async Task ProcessBackgroundTask(CancellationToken stoppingToken)
        {
            var news = await _newsRepository.GenerateNewsAsync();
            await _newsRepository.AddNewsAsync(news);
        }
    }
}
