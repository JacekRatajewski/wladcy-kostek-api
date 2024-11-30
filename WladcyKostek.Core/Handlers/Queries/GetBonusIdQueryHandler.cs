using WladcyKostek.Core.Interfaces;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetBonusIdQueryHandler : IRequestHandler<GetBonusIdQuery, BaseResponse<int>>
    {
        private readonly IBonusesRepository _bonusesRepository;

        public GetBonusIdQueryHandler(IBonusesRepository bonusesRepository)
        {
            _bonusesRepository = bonusesRepository;
        }

        public async Task<BaseResponse<int>> Handle(GetBonusIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var newsId = await _bonusesRepository.GetBonusIdAsync(request.Name);
                return BaseResponse<int>.CreateResult(newsId);
            }
            catch (Exception ex)
            {
                return BaseResponse<int>.CreateError(ex.Message);
            }
        }
    }
}
