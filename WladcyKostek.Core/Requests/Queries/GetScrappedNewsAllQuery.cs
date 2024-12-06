using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetScrappedNewsAllQuery : IRequest<BaseResponse<List<ScrappedNewsDTO>>>
    {
    }
}
