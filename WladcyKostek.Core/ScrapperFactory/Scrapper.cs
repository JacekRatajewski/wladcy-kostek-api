using WladcyKostek.Core.ScrapperFactory.Models;

namespace WladcyKostek.Core.ScrapperFactory.Scrappers
{
    public class Scrapper
    {
        public async Task<List<ScrappedNews>?> Run(ScrapperCreator creator, string url)
        {
            creator.Connect(url);
            var news = await creator.RunScrapper();
            if (news is not null)
                return news.GetNews();
            else
                return null;
        }
    }
}
