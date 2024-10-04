using Microsoft.AspNetCore.SignalR;
using WladcyKostek.Core.Hubs;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;
using WladcyKostek.Core.Requests;
using WladcyKostek.Core.Requests.Commands;

namespace WladcyKostek.Core.Handlers.Commands
{
    internal class AddNewsCommandHandler : IRequestHandler<AddNewsCommand, BaseResponse<int>>
    {
        private readonly INewsRepository _newsRepository;
        private readonly IHubContext<NewsHub> _newsHub;

        public AddNewsCommandHandler(INewsRepository newsRepository, IHubContext<NewsHub> newsHub)
        {
            _newsRepository = newsRepository;
            _newsHub = newsHub;
        }

        public async Task<BaseResponse<int>> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            var newsDto = new NewsDTO
            {
                Title = request.Title,
                Message = request.Message,
                UserId = request.UserId,
                DateTime = DateTime.UtcNow,
                VideoUrl = request.VideoUrl,
                ImageBase64 = request.ImageBase64
            };
            try
            {
                var newsId = await _newsRepository.AddSingleNewsAsync(newsDto);
                newsDto.Id = newsId;
                await _newsHub.Clients.All.SendAsync("NewPost", newsDto);
                await _newsRepository.SetNewsAsSentAsync(newsId);
                return BaseResponse<int>.CreateResult(newsId);
            }
            catch (Exception ex)
            {
                return BaseResponse<int>.CreateError(ex.Message);
            }
        }
    }
}
