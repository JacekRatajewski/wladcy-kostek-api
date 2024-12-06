using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetScrappedNewsAllQueryHandler : IRequestHandler<GetScrappedNewsAllQuery, BaseResponse<List<ScrappedNewsDTO>>>
    {
        private readonly IScrappedNewsRepository _scrappedNewsRepository;

        public GetScrappedNewsAllQueryHandler(IScrappedNewsRepository scrappedNewsRepository)
        {
            _scrappedNewsRepository = scrappedNewsRepository;
        }

        public async Task<BaseResponse<List<ScrappedNewsDTO>>> Handle(GetScrappedNewsAllQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var news = await _scrappedNewsRepository.GetAllAsync();
                return BaseResponse<List<ScrappedNewsDTO>>.CreateResult(news);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<ScrappedNewsDTO>>.CreateError(ex.Message);
            }
        }
    }
}
