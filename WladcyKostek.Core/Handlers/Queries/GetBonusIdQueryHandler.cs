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
                var bonusId = await _bonusesRepository.GetBonusIdAsync(request.Name);
                if (bonusId is not null)
                {
                    return BaseResponse<int>.CreateResult((int)bonusId);
                }
                else
                {
                    return BaseResponse<int>.CreateError("Bonus Id not found.");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse<int>.CreateError(ex.Message);
            }
        }
    }
}
