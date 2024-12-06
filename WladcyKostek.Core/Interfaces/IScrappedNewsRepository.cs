using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WladcyKostek.Core.Models;
using WladcyKostek.Core.ScrapperFactory.Models;

namespace WladcyKostek.Core.Interfaces
{
    public interface IScrappedNewsRepository
    {
        public Task AddNewsAsync(List<IScrappedNews> news);
        public Task<List<ScrappedNewsDTO>> GetTop6Async();
        public Task<List<ScrappedNewsDTO>> GetAllSkip6Async();
        public Task<List<ScrappedNewsDTO>> GetAllAsync();
    }
}
