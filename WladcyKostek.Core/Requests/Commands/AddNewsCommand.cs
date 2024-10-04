using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.Requests.Commands
{
    public class AddNewsCommand : IRequest<BaseResponse<int>>
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? ImageBase64 { get; set; }
        public string? UserId { get; set; }
        public string? VideoUrl { get; set; }
    }
}
