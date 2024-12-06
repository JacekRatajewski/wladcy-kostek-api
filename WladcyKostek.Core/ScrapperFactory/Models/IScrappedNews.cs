using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.ScrapperFactory.Models
{
    public interface IScrappedNews
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
    }
}
