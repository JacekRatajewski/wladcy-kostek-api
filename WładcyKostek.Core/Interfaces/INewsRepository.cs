using WładcyKostek.Core.Models;

namespace WładcyKostek.Core.Interfaces
{
    public interface INewsRepository
    {
        public Task<List<int>> GetNewsIdsAsync();
        public Task<NewsDTO> GetSingleNewsAsync(int id);
        public Task SetNewsAsSentAsync(int id);
        public Task<List<NewsDTO>> GenerateNewsAsync();
        public Task AddNewsAsync(List<NewsDTO> news);
    }
}
