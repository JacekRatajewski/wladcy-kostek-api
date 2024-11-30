using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetBonusIdQuery : IRequest<BaseResponse<int>>
    {
        public string Name { get; set; }
    }
}
