using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.Models
{
    public class UserDTO
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool? FromGoogle { get; set; }
        public DateTime? AccountCreationDate { get; set; }
        public string? Token { get; set; }
    }
}
