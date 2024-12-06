using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.Requests.Commands
{
    public class RegisterCommand : IRequest<BaseResponse<bool>>
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
