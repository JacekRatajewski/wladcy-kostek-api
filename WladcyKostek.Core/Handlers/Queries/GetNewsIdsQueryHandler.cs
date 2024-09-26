using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Requests;
using WladcyKostek.Core.Requests.Queries;

namespace WladcyKostek.Core.Handlers.Queries
{
    public class GetNewsIdsQueryHandler : IRequestHandler<GetNewsIdsQuery, BaseResponse<List<int>>>
    {
        private INewsRepository _news;

        public GetNewsIdsQueryHandler(INewsRepository news)
        {
            _news = news;
        }

        public async Task<BaseResponse<List<int>>> Handle(GetNewsIdsQuery request, CancellationToken cancellationToken)
        {
            var ids = await _news.GetNewsIdsAsync();

            if (ids is null)
            {
                return BaseResponse<List<int>>.CreateError("test");
            }

            return BaseResponse<List<int>>.CreateResult(ids);
        }
    }
}
