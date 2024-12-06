namespace WladcyKostek.Core.ScrapperFactory.Models
{
    internal class PenAndPaperNews : INews
    {
        public List<ScrappedNews> scrappedNews;

        public PenAndPaperNews(List<ScrappedNews> _scrappedNews)
        {
            scrappedNews = _scrappedNews;
        }

        public List<ScrappedNews> GetNews()
        {
            return scrappedNews;
        }
    }
}
