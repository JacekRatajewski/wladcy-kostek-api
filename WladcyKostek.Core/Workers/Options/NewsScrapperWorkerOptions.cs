using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WladcyKostek.Core.ScrapperFactory;

namespace WladcyKostek.Core.Workers.Options
{
    public class NewsScrapperWorkerOptions
    {
        public TimeSpan TimerIntervalHours { get; set; }
        public bool IsEnabled { get; set; }
        public string? Url { get; set; }
        public ScrapperCreator? Scrapper { get; set; }
    }
}
