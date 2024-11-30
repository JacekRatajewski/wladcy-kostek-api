using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Requests.Queries
{
    public class GetBonusesQuery : IRequest<BaseResponse<BonusesDTO>>
    {
        public int Id { get; set; }
    }
}
