﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WladcyKostek.Core.ScrapperFactory.Models;

namespace WladcyKostek.Core.ScrapperFactory.Scrappers
{
    public class PenAndPaperScrapper : ScrapperCreator
    {
        public override INews ScrapHtmlPage(HtmlDocument document)
        {
            var articles = document.DocumentNode.SelectNodes("//article");
            List<ScrappedNews> scrappedNews = [];
            foreach (var article in articles)
            {
                var pattern = @"&\d+;";
                var title = Regex.Replace(article.SelectSingleNode(".//h3[@class='entry-title mh-loop-title']/a")?.InnerText?.Trim() ?? "", pattern, "");
                var description = Regex.Replace(article.SelectSingleNode(".//div[@class='mh-loop-excerpt']/div/p")?.InnerText?.Trim() ?? "", pattern, "");
                var imageUrl = article.SelectSingleNode(".//figure[@class='mh-loop-thumb']/a/img")?.GetAttributeValue("src", null);
                var url = article.SelectSingleNode(".//h3[@class='entry-title mh-loop-title']/a")?.GetAttributeValue("href", null);
                scrappedNews.Add(new ScrappedNews
                {
                    Title = title,
                    Description = description,
                    ImageUrl = imageUrl,
                    Url = url
                });
            }

            return new PenAndPaperNews(scrappedNews);
        }
    }
}
