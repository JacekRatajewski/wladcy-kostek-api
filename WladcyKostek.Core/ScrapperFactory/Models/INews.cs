using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WladcyKostek.Core.ScrapperFactory.Models
{
    public interface INews
    {
        public List<ScrappedNews> GetNews();
    }
}
