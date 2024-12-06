using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetScrappedNewsTop6QueryHandler : IRequestHandler<GetScrappedNewsTop6Query, BaseResponse<List<ScrappedNewsDTO>>>
    {
        private readonly IScrappedNewsRepository _scrappedNewsRepository;

        public GetScrappedNewsTop6QueryHandler(IScrappedNewsRepository scrappedNewsRepository)
        {
            _scrappedNewsRepository = scrappedNewsRepository;
        }

        public async Task<BaseResponse<List<ScrappedNewsDTO>>> Handle(GetScrappedNewsTop6Query request, CancellationToken cancellationToken)
        {
            try
            {
                var news = await _scrappedNewsRepository.GetTop6Async();
                return BaseResponse<List<ScrappedNewsDTO>>.CreateResult(news);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<ScrappedNewsDTO>>.CreateError(ex.Message);
            }
        }
    }
}
