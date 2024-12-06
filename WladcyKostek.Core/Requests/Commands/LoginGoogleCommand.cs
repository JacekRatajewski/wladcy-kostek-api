﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.Requests.Commands
{
    public class LoginGoogleCommand : IRequest<BaseResponse<bool>>
    {
        public string? Token { get; set; }
    }
}
