using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WladcyKostek.Core.ScrapperFactory.Models;

namespace WladcyKostek.Core.ScrapperFactory
{
    public abstract class ScrapperCreator
    {
        private string url;
        internal HttpClient? httpClient;

        public void Connect(string _url)
        {
            url = _url;
            httpClient = new HttpClient();
        }
        public abstract INews ScrapHtmlPage(HtmlDocument document);

        public async Task<INews?> RunScrapper()
        {
            if (httpClient is not null)
            {
                string html = await httpClient.GetStringAsync(url);
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                return ScrapHtmlPage(htmlDoc);
            }
            return null;
        }
    }
}
