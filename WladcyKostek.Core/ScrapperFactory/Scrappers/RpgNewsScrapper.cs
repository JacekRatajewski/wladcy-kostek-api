using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WladcyKostek.Core.ScrapperFactory.Models;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace WladcyKostek.Core.ScrapperFactory.Scrappers
{
    public class RpgNewsScrapper : ScrapperCreator
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public override INews ScrapHtmlPage(HtmlDocument document)
        {
            var articles = document.DocumentNode.SelectNodes("//article");
            List<ScrappedNews> scrappedNews = [];

            foreach (var article in articles)
            {
                var pattern = @"&\d+;";
                var title = Regex.Replace(article.SelectSingleNode(".//h4[@class='entry-title title']/a")?.InnerText?.Trim() ?? "", pattern, "");
                var description = Regex.Replace(article.SelectSingleNode(".//div[@class='mg-content']/p")?.InnerText?.Trim() ?? "", pattern, "");
                var url = article.SelectSingleNode(".//h4[@class='entry-title title']/a")?.GetAttributeValue("href", null);
                var style = article.SelectSingleNode(".//div[@class='mg-post-thumb back-img md']")?.GetAttributeValue("style", null);
                var imageUrl = ExtractImageUrlFromStyle(style);

                var base64Image = imageUrl != null ? ConvertImageToBase64(imageUrl) : null;

                scrappedNews.Add(new ScrappedNews
                {
                    Title = title,
                    Description = description,
                    ImageUrl = imageUrl,
                    Url = url,
                    Base64Image = base64Image
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

        private string? ConvertImageToBase64(string imageUrl)
        {
            try
            {
                using (var response = _httpClient.GetAsync(imageUrl).Result)
                {
                    if (!response.IsSuccessStatusCode) return null;

                    using (var stream = response.Content.ReadAsStreamAsync().Result)
                    using (var image = Image.FromStream(stream))
                    {
                        // Zmniejsz obrazek 2x
                        int newWidth = image.Width / 2;
                        int newHeight = image.Height / 2;

                        using (var resizedImage = new Bitmap(newWidth, newHeight))
                        using (var graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

                            // Konwersja na Base64
                            using (var ms = new MemoryStream())
                            {
                                resizedImage.Save(ms, ImageFormat.Jpeg);
                                return Convert.ToBase64String(ms.ToArray());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image: {ex.Message}");
                return null;
            }
        }
    }
}
