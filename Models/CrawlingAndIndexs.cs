using EasyCrawling.Helpers;
using System.Collections.Generic;

namespace EasyCrawling.Models
{
    public class CrawlingAndIndexs
    {
        public Crawling Crawling { get; set; }
        public List<string> indexs { get; set; }

        public CrawlingAndIndexs(string name)
        {
            Crawling = new Crawling();
            Crawling.Name = name;
            indexs = new List<string>();
        }

        public CrawlingAndIndexs(Crawling crawling)
        {
            Crawling = crawling;
            indexs = FileHelper.LoadIndexFile(crawling);
        }

        public CrawlingAndIndexs(Crawling crawling, string name)
        {
            Crawling = new Crawling(crawling);
            Crawling.Name = name;
            indexs = new List<string>();
        }
    }
}
