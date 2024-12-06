using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Requests.Commands
{
    public class LoginGoogleCommand : IRequest<BaseResponse<UserDTO?>>
    {
        public string? Token { get; set; }
    }
}
