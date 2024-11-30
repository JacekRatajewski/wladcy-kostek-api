using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Interfaces
{
    public interface IBonusesRepository
    {
        public Task<int> GetBonusIdAsync(string name);
        public Task<BonusesDTO> GetBonusesAsync(int id);
    }
}
