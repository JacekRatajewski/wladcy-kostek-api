using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.Models
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? ImageBase64 { get; set; }
        public string? UserId { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
