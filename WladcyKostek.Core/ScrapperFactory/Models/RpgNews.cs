namespace WladcyKostek.Core.ScrapperFactory.Models
{
    internal class RpgNews : INews
    {
        private List<dynamic> scrappedNews;

        public RpgNews(List<dynamic> _scrappedNews)
        {
            scrappedNews = _scrappedNews;
        }

        public List<IScrappedNews> MapNewsData()
        {
            List<IScrappedNews> _scrappedNews = [];
            foreach (var news in scrappedNews)
            {
                _scrappedNews.Add(news);
            }
            return _scrappedNews;
        }
    }
}
