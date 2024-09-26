using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetNewsQuery : IRequest<BaseResponse<NewsDTO>>
    {
        public int Id { get; set; }
    }
}
