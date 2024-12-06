using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetScrappedNewsAllSkip6QueryHandler : IRequestHandler<GetScrappedNewsAllSkip6Query, BaseResponse<List<ScrappedNewsDTO>>>
    {
        private readonly IScrappedNewsRepository _scrappedNewsRepository;

        public GetScrappedNewsAllSkip6QueryHandler(IScrappedNewsRepository scrappedNewsRepository)
        {
            _scrappedNewsRepository = scrappedNewsRepository;
        }

        public async Task<BaseResponse<List<ScrappedNewsDTO>>> Handle(GetScrappedNewsAllSkip6Query request, CancellationToken cancellationToken)
        {
            try
            {
                var news = await _scrappedNewsRepository.GetAllSkip6Async();
                return BaseResponse<List<ScrappedNewsDTO>>.CreateResult(news);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<ScrappedNewsDTO>>.CreateError(ex.Message);
            }
        }
    }
}
