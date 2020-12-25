using System;

namespace EasyCrawling.Models
{
    [Serializable]
    public class CrawlingInfo
    {
        [NonSerialized]
        public Crawling CrawlingPointer;
        public string CrawlingName { get; set; }
        public WordPointer UrlOption { get; set; }

        public CrawlingInfo()
        {
            UrlOption = new WordPointer();
            CrawlingName = "";
        }

        public CrawlingInfo(CrawlingInfo info)
        {
            CrawlingPointer = info.CrawlingPointer;
            CrawlingName = info.CrawlingName;
            UrlOption = info.UrlOption;
        }
    }
}
