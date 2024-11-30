using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.Models
{
    public class BonusesDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? SessionCount { get; set; }
        public int? BonusCount { get; set; }
        public int? MoneySupported { get; set; }
    }
}
