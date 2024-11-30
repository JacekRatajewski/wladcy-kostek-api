using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetBonusesQueryHandler : IRequestHandler<GetBonusesQuery, BaseResponse<BonusesDTO>>
    {
        private readonly IBonusesRepository _bonusesRepository;

        public GetBonusesQueryHandler(IBonusesRepository bonusesRepository)
        {
            _bonusesRepository = bonusesRepository;
        }

        public async Task<BaseResponse<BonusesDTO>> Handle(GetBonusesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bonus = await _bonusesRepository.GetBonusesAsync(request.Id);
                return BaseResponse<BonusesDTO>.CreateResult(bonus);
            }
            catch (Exception ex)
            {
                return BaseResponse<BonusesDTO>.CreateError(ex.Message);
            }
        }
    }
}
