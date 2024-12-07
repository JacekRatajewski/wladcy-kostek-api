using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WladcyKostek.Core.ScrapperFactory.Models;

namespace WladcyKostek.Core.ScrapperFactory.Scrappers
{
    public class RpgNewsScrapper : ScrapperCreator
    {
        public override INews ScrapHtmlPage(HtmlDocument document)
        {
            var articles = document.DocumentNode.SelectNodes("//article");
            List<ScrappedNews> scrappedNews = [];
            foreach (var article in articles)
            {
                var pattern = @"&#\d+;";
                var title = Regex.Replace(article.SelectSingleNode(".//h4[@class='entry-title title']/a")?.InnerText?.Trim() ?? "", pattern, "");
                var description = Regex.Replace(article.SelectSingleNode(".//div[@class='mg-content']/p")?.InnerText?.Trim() ?? "", pattern, "");
                var url = article.SelectSingleNode(".//h4[@class='entry-title title']/a")?.GetAttributeValue("href", null);
                var style = article.SelectSingleNode(".//div[@class='mg-post-thumb back-img md']")?.GetAttributeValue("style", null);
                var imageUrl = ExtractImageUrlFromStyle(style);
                scrappedNews.Add(new ScrappedNews
                {
                    Title = title,
                    Description = description,
                    ImageUrl = imageUrl,
                    Url = url
                });
            }
            scrappedNews = scrappedNews.Where(x => x.ImageUrl != null && x.Title != null && x.Url != null && x.Description != null).ToList();

            return new RpgNews(scrappedNews);
        }

        private string? ExtractImageUrlFromStyle(string? style)
        {
            if (string.IsNullOrEmpty(style)) return null;

            var startIndex = style.IndexOf("url('") + 5;
            var endIndex = style.IndexOf("')");

            return (startIndex > 4 && endIndex > startIndex)
                ? style.Substring(startIndex, endIndex - startIndex)
                : null;
        }
    }
}
