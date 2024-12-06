namespace WladcyKostek.Core.ScrapperFactory.Models
{
    internal class RpgNews : INews
    {
        private List<ScrappedNews> scrappedNews;

        public RpgNews(List<ScrappedNews> _scrappedNews)
        {
            scrappedNews = _scrappedNews;
        }

        public List<ScrappedNews> GetNews()
        {
            return scrappedNews;
        }
    }
}
