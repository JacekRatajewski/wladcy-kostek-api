using WładcyKostek.Core.Models;

namespace WładcyKostek.Core.Requests.Queries
{
    public class GetNewsQuery : IRequest<BaseResponse<NewsDTO>>
    {
        public int Id { get; set; }
    }
}
